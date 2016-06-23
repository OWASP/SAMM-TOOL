using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace OwaspSAMM.DAL
{
    public enum AssessmentLoadType { All, Scorecard, TableOnly, Detail }

    public class OwaspSAMMRepository : IDisposable
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private OwaspSAMMContext context = new OwaspSAMMContext();

        //
        // GENERAL NOTE:  The data model is using Eager Loading to retrieve data (Lazy Loading has been turned off).  The AssessmentTemplate
        // and Assessment object contain several layers of child elements.  To help with the task of retrieving all the child elements, an
        // extention method (Load) was created to simplify the code that retrieves the child objects.  The extention methods are located in
        // ExtentionMethods.cs in this project.
        // 

        public OwaspSAMMRepository()
        {
        }

        #region Dispose Method
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Template Methods

        /// <summary>
        /// GetTemplate returns an object of type AssessmentTemplatetakes.  It can be called with an optional argument to specify a 
        /// template version.  If omitted, the newest default template is returned.
        /// </summary>
        /// <param name="templateVersion">Optional, Integer - specifies template version to return</param>
        /// <returns></returns>
        public AssessmentTemplate GetTemplate(int templateVersion = 0)
        {
            var result = new AssessmentTemplate();

            if (templateVersion == 0)
            {
                templateVersion = GetDefaultTemplateVersion();
            }

            try
            {
                result = context.AssessmentTemplates.Fill()
                            .Where(a => a.TemplateVersion == templateVersion).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.GetTemplate", ex);
            }

            return result;
        }

        /// <summary>
        /// GetDefaultTemplateVersion determines the version number of the default template.
        /// </summary>
        /// <returns></returns>
        public int GetDefaultTemplateVersion()
        {
            int result = 0;

            try
            {
                result = context.AssessmentTemplates
                                .Where(a => a.DefaultTemplate == true)
                                .OrderByDescending(a => a.TemplateDate)
                                .Select(a => a.TemplateVersion)
                                .FirstOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.GetDefaultTemplateVersion", ex);
            }
            return result;
        }
        #endregion

        #region UserData Methods

        /// <summary>
        /// GetPeerIdentityList - returns an array of identities that have the same ManagerID as the identity passed to the method
        /// </summary>
        /// <param name="id">Int - identity of the person to find peers of</param>
        /// <returns>Returns an array of integers which are the peer identities of the identity passed to the method</returns>
        public List<int> GetPeerIdentityList(int id)
        {
            List<int> peerIds = null;


            try
            {
                var ud = context.UserDatas.Find(id);

                // Peers will have the same manager.  Need to exclude the identity we're searching for.
                peerIds = context.UserDatas
                                    .Where(o => o.ManagerID == ud.ManagerID && o.UserID != ud.UserID)
                                    .Select(o => o.UserID).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.GetPeerIdentityList", ex);
            }

            return peerIds;
        }

        /// <summary>
        /// GetTeamIdentityList - Retrieves a list of int, representing the UserIDs of a manager's team members.  The method checks to see that
        /// the id is a manager.  If true, it searches for all UserIDs that have the user as a manager.
        /// </summary>
        /// <param name="id">int - UserID of a manager.</param>
        /// <returns></returns>
        public List<int> GetTeamIdentityList(int id)
        {
            List<int> teamIds = new List<int>();

            try
            {
                var ud = context.UserDatas.Find(id);

                // Only attempt to retrieve team IDs if the user is a manager
                if (ud.IsManager())
                {
                    var account = ud.UserDomain + "\\" + ud.UserNTID;
                    teamIds = context.UserDatas
                                        .Where(o => o.ManagerID == account)
                                        .Select(o => o.UserID).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.GetTeamIdentityList", ex);
            }

            return teamIds;
        }


        /// <summary>
        /// GetAssessmentCount - Returns a count of assessments owned by the UserID
        /// </summary>
        /// <param name="id">int - UserID of person to check</param>
        /// <returns>int - count of assessments owned by UserID</returns>
        public int GetAssessmentCount(int id)
        {
            int AssessmentCount = 0;

            try
            {
                AssessmentCount = context.Assessments.Where(o => o.OwnerID == id).Count();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.GetAssessmentCount", ex);
            }

            return AssessmentCount;
        }


        /// <summary>
        /// GetUserData retrieves a UserData object from the database by UserID
        /// </summary>
        /// <param name="id">Int - UserId (key value from UserData table)</param>
        /// <returns>UserData object</returns>
        public UserData GetUserData(int id)
        {
            var ud = new UserData();

            try
            {
                ud = (from o in context.UserDatas
                      where o.UserID == id
                      select o).FirstOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepisitory.GetUserData(int)", ex);
            }
            return ud;
        }

        /// <summary>
        /// GetUserData - this overload retrieves a UserData object by Account (userDomain\userAccount)
        /// </summary>
        /// <param name="account">string - Account name in the format - domain\account</param>
        /// <returns>UserData object</returns>
        public UserData GetUserData(string account)
        {
            UserData ud = new UserData();
            string userDomain = string.Empty;
            string userNTID = string.Empty;

            try
            {
                if (account.Contains('\\'))
                {
                    var xx = account.Split('\\');

                    switch (xx.Length)
                    {
                        case 1: userNTID = xx[0];
                            ud = (from o in context.UserDatas
                                  where o.UserNTID == userNTID
                                  select o).FirstOrDefault();
                            break;
                        case 2:
                            userDomain = xx[0];
                            userNTID = xx[1];
                            ud = (from o in context.UserDatas
                                  where o.UserNTID == userNTID && o.UserDomain == userDomain
                                  select o).FirstOrDefault();
                            break;
                        default: break;
                    }
                }
                else
                    ud = (from o in context.UserDatas
                          where o.UserNTID == account
                          select o).FirstOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.GetUserData(string)", ex);
            }

            return ud;
        }


        /// <summary>
        /// InsertUserData  - inserts data into the UserData table
        /// </summary>
        /// <param name="ud">UserData - data to be inserted into UserData table</param>
        /// <returns>bool - true if successful, otherwise false</returns>
        public bool InsertUserData(UserData ud)
        {
            bool results = false;

            try
            {
                var UD = (from o in context.UserDatas
                          where o.UserNTID == ud.UserNTID && o.UserDomain == ud.UserDomain
                          select o).FirstOrDefault();

                if (UD == null)
                {
                    context.UserDatas.Add(ud);
                    context.SaveChanges();
                    results = true;
                }
                else
                    results = false;

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.InsertUserData", ex);
            }
            return results;
        }


        /// <summary>
        /// UpdateUserData - Updates UserData table with data passed to the method
        /// </summary>
        /// <param name="ud">UserData - data to be updated</param>
        /// <returns>bool - True if successful, otherwise false</returns>
        public bool UpdateUserData(UserData ud)
        {
            bool results = false;

            try
            {
                var UD = (from o in context.UserDatas
                          where o.UserID == ud.UserID
                          select o).FirstOrDefault();

                if (UD == null)
                    results = false;
                else
                {
                    UD.LastName = ud.LastName;
                    UD.FirstName = ud.FirstName;
                    UD.ManagerID = ud.ManagerID;
                    UD.MgrFirstName = ud.MgrFirstName;
                    UD.MgrLastName = ud.MgrLastName;
                    UD.ManagerEID = ud.ManagerEID;
                    UD.LastLoginDate = ud.LastLoginDate;
                    UD.Administrator = ud.Administrator;
                    UD.OrgName = ud.OrgName;
                    UD.Manager = ud.Manager;
                    UD.BusinessUnit = ud.BusinessUnit;
                    UD.BUOwner = ud.BUOwner;

                    context.SaveChanges();
                    results = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.UpdateUserData", ex);
            }
            return results;
        }

        /// <summary>
        /// IsUserAManager - Determines if the UserID is a manager by looking in the UserData table to see if the user's account
        /// appears in the ManagerID field.  If it does, then the UserID is considered to be a manager.
        /// </summary>
        /// <param name="identity">int - UserID of person to check</param>
        /// <returns></returns>
        public bool IsUserAManager(int identity)
        {
            int managerCount = 0;

            try
            {
                // Retrieve the UserData object for the identity
                var ud = context.UserDatas.Where(o => o.UserID == identity).Single();

                // assemble the account string for the user
                string domainAccount = ud.UserDomain + "\\" + ud.UserNTID;

                // search for any users that have this person as a manager
                managerCount = context.UserDatas.Where(o => o.ManagerID == domainAccount).Count();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.IsUserManager(identity)", ex);
            }
            // If managerCount is > 0, then the person is a manager - return true otherwise return false
            return managerCount > 0;
        }


        #endregion

        #region Assessment Methods

        /// <summary>
        /// GetAssessments - returns a list of assessments meeting the specified criteria.  If no arguments are supplied, returns all 
        /// assessments in the database.  If an identity is passed as an argument, then only the assessments owned by the identity are
        /// returned.
        /// </summary>
        /// <param name="uid">Optional Integer - Identity to use when retrieving assessments. Default to 0.</param>
        /// <param name="alt">Optional AssessmentLoadType - used to limit the size of the child data retrieved. Defaults to TableOnly.</param>
        /// <returns>Returns a List of Assessment objects</returns>
        public List<Assessment> GetAssessments(int uid = 0, AssessmentLoadType alt = AssessmentLoadType.TableOnly)
        {
            // Recycle context to ensure we get updates made in other contexts
            context.Dispose();
            context = new OwaspSAMMContext();

            var results = new List<Assessment>();

            try
            {
                if (uid == 0)
                {
                    // Return all assessments
                    results = context.Assessments.Fill(alt)
                        .OrderBy(o => o.ApplicationName)
                        .ToList();
                }
                else
                {
                    // Filter Assessments by user ID
                    results = context.Assessments.Fill(alt)
                        .Where(o => o.OwnerID == uid)
                        .OrderBy(o => o.ApplicationName)
                        .ToList();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetAssessments", ex);
            }

            return results;
        }

        /// <summary>
        /// GetAssessmentsNotOwned - returns a list of assessments meeting the specified criteria.  If no arguments are supplied, returns all 
        /// assessments in the database.  If an identity is passed as an argument, then only the assessments not owned by the identity are
        /// returned.
        /// </summary>
        /// <param name="uid">Optional Integer - Identity to use when retrieving assessments</param>
        /// <returns>Returns a List of Assessment objects</returns>
        public List<Assessment> GetAssessmentsNotOwned(int uid = 0, AssessmentLoadType alt = AssessmentLoadType.TableOnly)
        {
            var results = new List<Assessment>();

            try
            {
                if (uid == 0)
                {
                    // Return all assessments
                    results = context.Assessments.Fill(alt)
                        .OrderBy(o => o.ApplicationName)
                        .ToList();
                }
                else
                {
                    // Filter Assessments by user ID
                    results = context.Assessments.Fill(alt)
                        .Where(o => o.OwnerID != uid)
                        .OrderBy(o => o.ApplicationName)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetAssessmentsNotOwned", ex);
            }

            return results;
        }


        /// <summary>
        /// GetAssessments - Overload gets assessments for a list of identities
        /// </summary>
        /// <param name="idList">List<Int>  of identities</param>
        /// <returns>Returns a list of Assessments owned by the list of Identities</returns>
        public List<Assessment> GetAssessments(List<int> idList, AssessmentLoadType alt = AssessmentLoadType.TableOnly)
        {
            var results = new List<Assessment>();

            try
            {
                results = context.Assessments.Fill(alt)
                    .Where(o => idList.Contains(o.OwnerID))
                    .OrderBy(o => o.ApplicationName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetAssessments<List<Int>>", ex);
            }

            return results;
        }

        /// <summary>
        /// GetAssessments - Overload gets assessments for a specific business unit
        /// </summary>
        /// <param name="BusinessUnit">string</param>
        /// <returns>Returns a list of Assessments for the Business Unit param</returns>
        public List<Assessment> GetAssessments(string BusinessUnit, AssessmentLoadType alt = AssessmentLoadType.TableOnly)
        {
            var results = new List<Assessment>();

            try
            {
                results = context.Assessments.Fill(alt)
                    .Where(o => o.BusinessUnit == BusinessUnit && o.Finalized == true)
                    .OrderBy(o => o.ApplicationName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetAssessments<string BusinessUnit>", ex);
            }

            return results;
        }

        /// <summary>
        /// Retrieves Assessment object by AssessmentID
        /// </summary>
        /// <param name="assessmentID">AssessmentID  - integer</param>
        /// <param name="loadType">enum that specifies which child object to populate in the data object</param>
        /// <returns>Assessment object</returns>
        public Assessment GetAssessment(int assessmentID, AssessmentLoadType loadType = AssessmentLoadType.All)
        {
            var assessment = new Assessment();
            try
            {
                assessment = context.Assessments.Fill(loadType)
                    .Where(o => o.AssessmentID == assessmentID)
                    .FirstOrDefault();

                // Sort all the child objects according to the corresponding "order" fields in the child objects
                if (loadType == AssessmentLoadType.All)
                {
                    foreach (var c in assessment.CategoryDatas)
                    {
                        foreach (var s in c.SectionDatas)
                        {
                            foreach (var g in s.GroupDatas)
                            {
                                g.QuestionDatas = g.QuestionDatas.OrderBy(o => o.QuestionOrder).ToList();
                            }
                            s.GroupDatas = s.GroupDatas.OrderBy(o => o.GroupOrder).ToList();
                        }
                        c.SectionDatas = c.SectionDatas.OrderBy(o => o.SectionOrder).ToList();
                    }
                    assessment.CategoryDatas = assessment.CategoryDatas.OrderBy(o => o.CategoryOrder).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetAssessment", ex);
            }

            return assessment;
        }


        /// <summary>
        /// CreateAssessment - uses the specified template to generate a new assessment and assign the owner to the identity provided
        /// </summary>
        /// <param name="userID">Int - UserID of Owner</param>
        /// <param name="templateVersion">Int - version number of template to use when generating assessment</param>
        /// <param name="orgName">string - Organization Name to assign to the Assessment</param>
        /// <param name="appName">string - Product Name to assign to the Assessment</param>
        /// <param name="createID">int - UserID of Creator</param>
        /// <param name="industryID">int = IndustryID that will be associated to the assessment.  Currently defaulting to first industry.</param>
        /// <returns></returns>
        public async Task<int> CreateAssessment(int ownerID, int templateVersion, string buName, string orgName, string appName, int createID, int industryID)
        {
            int results = 0;

            var categoryData = new CategoryData();
            var sectionData = new SectionData();
            var groupData = new GroupData();
            var questionData = new QuestionData();

            // Retrieve the template
            AssessmentTemplate template = GetTemplate(templateVersion);

            // Create a new Assessment and set the UserID as the owner, last update and creator.
            UserData ud = context.UserDatas.Find(ownerID);

            try
            {
                var newAssessment = new Assessment();

                newAssessment.TemplateVersion = templateVersion;
                newAssessment.OwnerID = ownerID;
                newAssessment.OrganizationName = orgName;
                newAssessment.BusinessUnit = buName;
                newAssessment.ApplicationName = appName;
                newAssessment.LastUpdated = DateTime.Now;
                newAssessment.CreateDate = DateTime.Now;
                newAssessment.LastUpdateBy = createID;
                newAssessment.CreateBy = createID;
                newAssessment.IndustryID = industryID;
                newAssessment.Finalized = false;

                ud.Assessments.Add(newAssessment);

                // Spin through the template and create the assessment
                foreach (var j in template.TemplateCategories)
                {
                    categoryData = new CategoryData();
                    categoryData.CategoryID = j.CategoryID;
                    categoryData.CategoryOrder = j.CategoryOrder;

                    newAssessment.CategoryDatas.Add(categoryData);

                    foreach (var k in j.TemplateSections)
                    {
                        sectionData = new SectionData();
                        sectionData.SectionID = k.SectionID;
                        sectionData.SectionOrder = k.SectionOrder;
                        sectionData.SectionScore = 0;
                        sectionData.SectionScorePartial = 0;

                        categoryData.SectionDatas.Add(sectionData);

                        foreach (var l in k.TemplateGroups)
                        {
                            groupData = new GroupData();
                            groupData.GroupID = l.GroupID;
                            groupData.GroupOrder = l.GroupOrder;
                            groupData.GroupScore = 0;

                            sectionData.GroupDatas.Add(groupData);

                            foreach (var m in l.TemplateQuestions)
                            {
                                questionData = new QuestionData();
                                questionData.Answer = false;
                                questionData.QuestionID = m.QuestionID;
                                questionData.QuestionOrder = m.QuestionOrder;

                                groupData.QuestionDatas.Add(questionData);
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();
                results = newAssessment.AssessmentID;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.CreateAssessment", ex);
            }

            return results;
        }

        /// <summary>
        /// UpdateAssessmentOwner - uses the specified ownerID and newOwnerID to transfer ownership of all assessments
        /// </summary>
        /// <param name="ownerID">Integer - identity of from ownerID</param>
        /// <param name="newOwnerID">Integer - identity of to ownerID</param>
        /// <param name="updateID">Integer - identity of user performing the update</param>
        /// <returns></returns>
        public bool UpdateAssessmentOwner(int ownerID, int newOwnerID, int updateID)
        {
            bool results = false;

            var newOwner = GetUserData(newOwnerID);
            var assessments = new List<Assessment>();

            assessments = GetAssessments(ownerID, AssessmentLoadType.TableOnly);

            try
            {
                foreach (var assessment in assessments)
                {
                    assessment.OwnerID = newOwnerID;
                    assessment.BusinessUnit = newOwner.BusinessUnit;
                    assessment.OrganizationName = newOwner.OrgName;
                    assessment.LastUpdated = DateTime.Now;
                    assessment.LastUpdateBy = updateID;
                }

                context.SaveChanges();
                results = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.UpdateAssessmentOwner", ex);
            }

            return results;
        }

        /// <summary>
        /// DeleteAssessment - deletes an assessment by AssessmentID along with all child data
        /// </summary>
        /// <param name="id">Integer - AssessmentID</param>
        public void DeleteAssessment(int id)
        {
            // EF has trouble deleting child elements from an object because of foreign key relationships.
            // Using SQL to delete child elements starting at the bottom of the assessment object and working up

            try
            {
                // Delete all QuestionData for the assessment
                context.Database.ExecuteSqlCommand("DELETE FROM QuestionData WHERE GroupID IN (" +
                                                            "SELECT GroID FROM GroupData WHERE SectionID IN (" +
                                                                    "SELECT SecID FROM SectionData WHERE CategoryID IN (" +
                                                                            "SELECT CatID FROM CategoryData WHERE AssessmentID = {0}" +
                                                                            ")))", id);

                // Delete all GroupData for the assessment
                context.Database.ExecuteSqlCommand("DELETE FROM GroupData WHERE SectionID IN (" +
                                                                    "SELECT SecID FROM SectionData WHERE CategoryID IN (" +
                                                                            "SELECT CatID FROM CategoryData WHERE AssessmentID = {0}" +
                                                                            "))", id);

                // Delete all SectionData for the assessment
                context.Database.ExecuteSqlCommand("DELETE FROM SectionData WHERE CategoryID IN (" +
                                                                            "SELECT CatID FROM CategoryData WHERE AssessmentID = {0}" +
                                                                            ")", id);

                // Delete all CategoryData for the assessment
                context.Database.ExecuteSqlCommand("DELETE FROM CategoryData WHERE AssessmentID = {0}", id);

                // Finally, delete the Assessment
                context.Database.ExecuteSqlCommand("DELETE FROM Assessment WHERE AssessmentID = {0}", id);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.DeleteAssessment", ex);
            }
        }

        /// <summary>
        /// GetIndustry - Retrieves the Industry Object for the id passed in.
        /// </summary>
        /// <param name="id">int - Industry ID.</param>
        /// <returns></returns>
        public Industry GetIndustry(int id)
        {
            Industry RetIndustry = new Industry();

            try
            {
                RetIndustry = context.Industries
                                        .Include(o=>o.IndustryTargets)
                                        .Where(o => o.IndustryID == id)
                                        .FirstOrDefault();
            
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetIndustry", ex);
            }

            return RetIndustry;
        }

        /// <summary>
        /// GetCategory - Retrieves the Category Object for the id passed in.
        /// </summary>
        /// <param name="id">int - Category ID.</param>
        /// <returns></returns>
        public Category GetCategory(int id)
        {
            Category RetCategory = new Category();

            try
            {
                RetCategory = context.Categories
                                        .Where(o => o.CategoryID == id)
                                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetCategory", ex);
            }

            return RetCategory;
        }

        /// <summary>
        /// GetSection - Retrieves the Section Object for the id passed in.
        /// </summary>
        /// <param name="id">int - Section ID.</param>
        /// <returns></returns>
        public Section GetSection(int id)
        {
            Section RetSection = new Section();

            try
            {
                RetSection = context.Sections
                                        .Where(o => o.SectionID == id)
                                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: OwaspSAMMRepository.GetSection", ex);
            }

            return RetSection;
        }


        #endregion

        /// <summary>
        /// SaveContextChanges allows business layer to save data changes from the BL
        /// </summary>
        public void SaveContextChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: SaveContextChanges", ex);
            }
        }
    }
}
