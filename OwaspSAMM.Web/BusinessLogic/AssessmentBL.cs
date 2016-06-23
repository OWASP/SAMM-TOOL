using OwaspSAMM.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OwaspSAMM.Web
{
    public class AssessmentBL
    {
        private OwaspSAMMRepository DAL = new OwaspSAMMRepository();

        /// <summary>
        /// Business Logic associated with managing Assessments
        /// </summary>
        public AssessmentBL() { }

        /// <summary>
        /// CreateAssessment - Create an Assessment based on an AssessmentTemplate
        /// </summary>
        /// <param name="ownerID">Int - UserID of Assessment owner</param>
        /// <param name="templateVersion">Int - TemplateID of template used to create the assessment</param>
        /// <param name="orgName">String - Organization name that Assessment belongs to.  Usually Organization of the person creating the assessment</param>
        /// <param name="productName">String - name of the product being assessed</param>
        /// <param name="createID">Int - UserID of the person creating the Assessment</param>
        /// <param name="industryID">Int - IndustryID associated with the assessment.  Currently defaults to 1st IndustryID</param>
        /// <returns></returns>
        public async Task<int> CreateAssessment(int ownerID, int templateVersion, string buName, string orgName, string productName, int createID, int industryID)
        {
            int results = 0;

            try
            {
                results = await DAL.CreateAssessment(ownerID, templateVersion, buName, orgName, productName, createID, industryID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: AssessmentBL.CreateAssessment", ex);
            }

            return results;
        }

        /// <summary>
        /// UpdateAssessmentOwner updates the owner of all assessments for a user to a new user.  It is called from the 
        /// User maintenance screen.
        /// </summary>
        /// <param name="ownerID">Int - identity of current owner</param>
        /// <param name="newOwnerID">Int - identity of new owner</param>
        /// <param name="updateID">Int - identity of user performing the update</param>
        public bool UpdateAssessmentOwner(int ownerID, int newOwnerID, int updateID)
        {
            bool results = false;

            try
            {
                results = DAL.UpdateAssessmentOwner(ownerID, newOwnerID, updateID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: AssessmentBL.UpdateAssessmentOwner", ex);
            }

            return results;
        }

        /// <summary>
        /// UpdateAssessmentData updates the assessment question data with the changes made by the user.  It is called from the Update action
        /// method in the AssessmentController when an assessment is saved.
        /// </summary>
        /// <param name="assessmentToUpdate">int? - AssessmentID of assessment being updated</param>
        /// <param name="assessmentResponse">string[] of QuestionIDs which are generated in the Update view for the AssessmentController</param>
        /// <param name="updateIdentity">int - Identity of user making updates to the assessment</param>
        internal void UpdateAssessmentData(int? assessmentID, string[] assessmentResponse, int updateIdentity)
        {
            var responseHS = new HashSet<string>();
            Assessment assessmentToUpdate = new Assessment();

            if (assessmentID != null)
            {
                assessmentToUpdate = DAL.GetAssessment(assessmentID.Value);

                if (assessmentResponse != null)
                {
                    responseHS = new HashSet<string>(assessmentResponse);
                }
                // Spin through all the questions and update Answer with any changes
                foreach (var o in assessmentToUpdate.CategoryDatas)
                {
                    foreach (var p in o.SectionDatas)
                    {
                        foreach (var q in p.GroupDatas)
                        {
                            foreach (var r in q.QuestionDatas)
                            {
                                if (responseHS.Contains(r.QuestionID.ToString()))
                                    r.Answer = true;
                                else
                                    r.Answer = false;
                            }
                        }
                    }
                }

                assessmentToUpdate.CalculateScore();                        // Calculate the score of the updated Assessment
                assessmentToUpdate.LastUpdated = DateTime.Now;              // Added to update the lastupdate time
                assessmentToUpdate.LastUpdateBy = updateIdentity;
                assessmentToUpdate.Finalized = false;
                DAL.SaveContextChanges();
            }
        }

        /// <summary>
        /// GetAssessmentByAssessmentID - Retrieves an Assessment object by AssessmentID
        /// </summary>
        /// <param name="id">int? - AssessmentID of Assessment to be retrieved</param>
        /// <returns></returns>
        internal Assessment GetAssessmentByAssessmentID(int? id)
        {
            return DAL.GetAssessment(id.Value);
        }


        internal Assessment GetAssessmentDetail(int? id)
        {
            return DAL.GetAssessment(id.Value, AssessmentLoadType.Detail);
        }

        internal Assessment GetAsessmentScorecard(int? id)
        {
            return DAL.GetAssessment(id.Value, AssessmentLoadType.Scorecard);
        }


        internal Assessment GetAsessmentScorecardAverage(string BusinessUnit)
        {

            //Retrieve list of Assessments for BusinessUnit
            var BUAssessments = new List<Assessment>();
            BUAssessments = DAL.GetAssessments(BusinessUnit, AssessmentLoadType.Scorecard);

            //Create Assessment object to return
            var categoryData = new CategoryData();
            var sectionData = new SectionData();

            Assessment BUAverageAssessment = new Assessment();

            AssessmentTemplate template = DAL.GetTemplate(BUAssessments[0].TemplateVersion);

            BUAverageAssessment.TemplateVersion = BUAssessments[0].TemplateVersion;
            BUAverageAssessment.OrganizationName = "";
            BUAverageAssessment.ApplicationName = "BU " + BusinessUnit + " Average";
            BUAverageAssessment.LastUpdated = DateTime.Now;
            BUAverageAssessment.CreateDate = DateTime.Now;
            BUAverageAssessment.IndustryID = 1;
            BUAverageAssessment.Industry = DAL.GetIndustry(1);


            // Spin through the template and create the assessment
            foreach (var j in template.TemplateCategories)
            {
                categoryData = new CategoryData();
                categoryData.CategoryID = j.CategoryID;
                categoryData.Category = DAL.GetCategory(j.CategoryID);
                categoryData.CategoryOrder = j.CategoryOrder;

                BUAverageAssessment.CategoryDatas.Add(categoryData);

                foreach (var k in j.TemplateSections)
                {
                    sectionData = new SectionData();
                    sectionData.SectionID = k.SectionID;
                    sectionData.Section = DAL.GetSection(k.SectionID);
                    sectionData.SectionOrder = k.SectionOrder;
                    sectionData.SectionScore = 0;
                    sectionData.SectionScorePartial = 0;
                    categoryData.SectionDatas.Add(sectionData);

                }
            }


            // Spin through all the assessments for the business unit, calculate the average score, build the return Assessment object
            foreach (var BUAssessment in BUAssessments)
            {

                foreach (var o in BUAssessment.CategoryDatas)
                {
                    foreach (var p in o.SectionDatas)
                    {

                        // Spin thru the return assessment object and accumulate the scores 
                        foreach (var oo in BUAverageAssessment.CategoryDatas)
                        {
                            foreach (var pp in oo.SectionDatas)
                            {
                                if (pp.SectionID == p.SectionID)
                                {
                                    pp.SectionScore = pp.SectionScore + p.SectionScore;
                                    pp.SectionScorePartial = pp.SectionScorePartial + p.SectionScorePartial;
                                }

                            }
                        }
                    }
                }

            }

            //Spins thru the Section data in the return object and calculate average score and set SectionScore and SectionScorePartial 

            foreach (var oo in BUAverageAssessment.CategoryDatas)
            {
                foreach (var pp in oo.SectionDatas)
                {

                    float TotalScore = 0f;
                    float Remainder = 0f;

                    //Accumulate total score for SectionScore

                    if ((int)pp.SectionScore > 0)
                    {
                        TotalScore = TotalScore + (int)pp.SectionScore;
                    }

                    //Accumulate total score for SectionScorePartial by multiplying value by .5

                    if ((int)pp.SectionScorePartial > 0)
                    {
                        TotalScore = TotalScore + ((int)pp.SectionScorePartial * .5f);
                    }

                    //Calculate TotalScore Average

                    TotalScore = TotalScore / BUAssessments.Count;

                    //Round total score to 1 decimal place

                    Math.Round((Decimal)TotalScore, 1, MidpointRounding.AwayFromZero);

                    //Set SectionScore to the truncated average value

                    pp.SectionScore = (int)Math.Truncate(TotalScore);

                    //Set remainder = to the value right of the decimal by subtracting truncated value from average value

                    Remainder = TotalScore - (float)pp.SectionScore;

                    //If the Remainder is >= .3 and < .8 (meaning it would round to .5), SectionScorePartial = 1(+), otherwise SectionScorePartial = 0 

                    if (Remainder >= .3 && Remainder < .8)
                    {
                        pp.SectionScorePartial = 1;
                    }
                    else
                    {
                        pp.SectionScorePartial = 0;
                    }

                    //If the Remainder is >= .8 (meaning we should round the SectionScore up to the next whole number, Add +1 to SectionScore 

                    if (Remainder >= .8)
                    {
                        pp.SectionScore = pp.SectionScore + 1;
                    }


                }
            }


            return BUAverageAssessment;
        }

        /// <summary>
        /// DeleteAssessment - Deletes an assessment including its children
        /// </summary>
        /// <param name="id">int - AssessmentID to be deleted</param>
        internal void DeleteAssessment(int id)
        {
            DAL.DeleteAssessment(id);
        }


        /// <summary>
        /// UpdateAssessmentScore - This method is called to recalculate the section score after a checkbox is clicked.  The method is
        /// called from the Assessment controller as it is responding to the Ajax call to update the checkbox
        /// </summary>
        /// <param name="sessionAssessment">Assessment object passed from the controller</param>
        /// <param name="questionID">Integer - ID of question checked on the screen</param>
        /// <param name="questionValue">boolean - new value of checkbox that was checked on the screen</param>
        /// <returns></returns>
        internal Assessment UpdateAssessmentScore(Assessment sessionAssessment, int questionID, bool questionValue)
        {
            // Select the question object from the Assessment
            var question = from o in sessionAssessment.CategoryDatas
                           from p in o.SectionDatas
                           from q in p.GroupDatas
                           from r in q.QuestionDatas
                           where r.QuestionID == questionID
                           select r;

            // Update the Answer value with the new value from the screen
            question.Single().Answer = questionValue;

            // Recompute the Section scores
            sessionAssessment.CalculateScore();

            return sessionAssessment;
        }

        /// <summary>
        /// Update an assessment and set the Finalized field to true on the assessment table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateIdentity"></param>
        internal void FinalizeAssessment(int id, int updateIdentity)
        {
            Assessment assessment = DAL.GetAssessment(id, AssessmentLoadType.TableOnly);
            assessment.Finalized = true;
            assessment.LastUpdated = DateTime.Now;              // Added to update the lastupdate time
            assessment.LastUpdateBy = updateIdentity;
            DAL.SaveContextChanges();

        }

        /// <summary>
        /// Retrieves a list of Assessment objects (table only) where BusinessUnit == the passed value
        /// </summary>
        /// <param name="bu">string - Business Unit name</param>
        /// <returns>List of Assessment object (table only)</returns>
        internal List<Assessment> GetAssessmentsByBU(string bu)
        {
            return DAL.GetAssessments(bu, AssessmentLoadType.TableOnly);
        }

        /// <summary>
        /// Retrieves a list of Assessment objects containing all Assessments where (Finalized == true) 
        /// </summary>
        /// <returns></returns>
        internal List<Assessment> GetFinalizedAssessments()
        {
            return DAL.GetAssessments(0, AssessmentLoadType.TableOnly).Where(o => o.Finalized == true).ToList();
        }

        /// <summary>
        /// Update Assessment detail (assessment table only) with updates from the Edit screen
        /// </summary>
        /// <param name="assessment"></param>
        /// <param name="userData"></param>
        internal void UpdateAssessmentDetail(Assessment assessment, UserData userData)
        {
            var a = DAL.GetAssessment(assessment.AssessmentID, AssessmentLoadType.TableOnly);

            // Removed from OS version
            //if (a.OwnerID != assessment.AssessmentID)
            //{
            //    // If the Owner is changed, update the assessment BU and Org with the new owner's BU and Org
            //    var newUser = DAL.GetUserData(assessment.OwnerID);
            //    a.BusinessUnit = newUser.BusinessUnit;
            //    a.OrganizationName = newUser.OrgName;
            //}

            a.OwnerID = assessment.OwnerID;
            a.BusinessUnit = assessment.BusinessUnit;
            a.ApplicationName = assessment.ApplicationName;
            a.IndustryID = assessment.IndustryID;
            a.TemplateVersion = assessment.TemplateVersion;
            a.LastUpdated = DateTime.Now;              // Added to update the lastupdate time
            a.LastUpdateBy = userData.UserID;
            DAL.SaveContextChanges();

        }

        internal MemoryStream Export2Excel(Assessment assessment)
        {
            // The worksheet will eventually get pushed to the browser, so creating the worksheet in a memory stream will enable us to 
            // push out the worksheet.
            MemoryStream memStream = new MemoryStream();

            OpenXMLExcelHelper oxmlDoc = new OpenXMLExcelHelper();
            var sheetname = assessment.ApplicationName + " vs " + assessment.Industry.IndustryName;

            //Excel limits worksheet names to 31 chars - truncate if needed
            if (sheetname.Length > 31)
            {
                sheetname = sheetname.Substring(0, 31);
            }
            oxmlDoc.CreateWorkbook(memStream, sheetname);

            // This List is used to hold Column Names.  First two columns will always be ID and Requirements.  Remaining column
            // names are dynamic and can vary by template.
            List<string> columnNames = new List<string>();
            columnNames.Add("Category");
            columnNames.Add("Current");
            columnNames.Add(assessment.Industry.IndustryName);

            uint activeColumn = 1;              // Set the column pointer to Column "A"
            uint activeRow = 1;                 // Set the row pointer to Row 1

            // Write the column headings into Row 1
            foreach (var s in columnNames)
            {
                oxmlDoc.WriteStringToCell(s, activeColumn, activeRow, (uint)OxmlCellFormat.Bold);
                activeColumn++;
            }
            activeRow++;

            //** Start traversing sections in the assessment and write into the worksheet **//
            foreach (var o in assessment.CategoryDatas)
            {
                foreach (var p in o.SectionDatas)
                {
                    activeColumn = 1;
                    oxmlDoc.WriteStringToCell(p.Section.SectionName, activeColumn, activeRow, (uint)OxmlCellFormat.Default);

                    activeColumn++;
                    var score = p.SectionScore.ToString();
                    if (p.SectionScorePartial > 0) score += ".5";
                    oxmlDoc.WriteNumberToCell(score, activeColumn, activeRow, (uint)OxmlCellFormat.Default);

                    activeColumn++;
                    var targetScore = p.Section.IndustryTargets.Where(p1 => p1.IndustryID == assessment.IndustryID).Select(q => q.Score).Single();
                    oxmlDoc.WriteNumberToCell(targetScore.Replace("+", ".5"), activeColumn, activeRow, (uint)OxmlCellFormat.Default);

                    activeRow++;

                }

            }

            // Set column widths - first column is ID and narrow
            oxmlDoc.SetColumnWidth(1u, 25d);
            // Set remaining columns to 50 column width units 
            for (uint i = 2; i <= columnNames.Count; i++)
                oxmlDoc.SetColumnWidth(i, 10d);

            // Save the new worksheet.
            oxmlDoc.Save();
            oxmlDoc.Close();
            return memStream;
        }
    }
}
