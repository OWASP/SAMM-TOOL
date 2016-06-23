using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OwaspSAMM.DAL;
using PagedList;
using System.Threading.Tasks;

namespace OwaspSAMM.Web.Controllers
{
    public class AssessmentsController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private OwaspSAMMContext db = new OwaspSAMMContext();
        UserDataBL userDataBL = new UserDataBL();
        AssessmentBL assessmentBL = new AssessmentBL();

        // GET: Assessments
        public ActionResult Index(int? page)
        {
            // The User.Identity.Name object is used to identify the user.  

            var userData = userDataBL.FindUser(User.Identity.Name);
            var assessments = userData.MyAssessments();

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;
            ViewBag.Peer = userData.UserType() == UserDataRoleType.Individual;

            // Get the page size value from web.config
            int pageSize = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["PageSize"].ToString());
            int pageNumber = page ?? 1;

            // Adjust the page size depending on how many assessments are in the top section.  If the upper sections gets really big, then 
            // make sure that at least 5 rows are visible in the paged section
            int usablePageSize = Math.Max(pageSize - assessments.Count(), 5);

            switch (userData.UserType())
            {
                case UserDataRoleType.Administrator:
                    ViewBag.AssessmentTitle = "All Other";
                    ViewBag.OtherAssessments = userData.AdminAssessments().ToPagedList(pageNumber, usablePageSize);
                    break;
                case UserDataRoleType.BusinessUnitOwner:
                    break;
                case UserDataRoleType.Manager:
                    ViewBag.AssessmentTitle = "Team";
                    ViewBag.OtherAssessments = userData.TeamAssessments().ToPagedList(pageNumber, usablePageSize);
                    break;
                case UserDataRoleType.Individual:
                    ViewBag.AssessmentTitle = "Peer";
                    ViewBag.OtherAssessments = userData.PeerAssessments().ToPagedList(pageNumber, usablePageSize);
                    break;
            }

            return View(assessments);
        }

        // GET: Assessments/Details/5
        public ActionResult Details(int? id, bool readOnly = false, string callBack = "index")
        {
            if (id == null)
            {
                logger.ErrorFormat("id is null");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;
            ViewBag.ReadOnly = readOnly;
            ViewBag.CallBack = callBack;

            Assessment assessment = assessmentBL.GetAssessmentDetail(id);

            if (assessment == null)
            {
                return HttpNotFound();
            }

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(id.Value, PeerAuthorizedType.PeerAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            // Show the Edit button only if the user is an Administrator, Manager or owner of the assessment.  Peers cannot
            // update the Assessment data, only the assessment questions.
            // - Phase 2 - BUOwners can view details from BUSummary screen.  If that is the path here then readOnly will be true
            var showEdit = userData.UserType() == UserDataRoleType.Administrator
                                || userData.UserType() == UserDataRoleType.Manager
                                || (userData.UserID == assessment.OwnerID);
            if (readOnly)
                ViewBag.ShowEdit = !readOnly;
            else
                ViewBag.ShowEdit = showEdit;

            return View(assessment);
        }

        [HttpPost, ActionName("Details")]
        // POST: Assessment/Details
        public ActionResult DetailsEdit(int id)
        {
            return RedirectToAction("Edit", new { id });
        }

        // GET: Assessments/Create
        public ActionResult Create()
        {
            logger.InfoFormat("User.Identity.Name: {0}", User.Identity.Name);
            var WhoAmI = User.Identity.Name;
            var userData = userDataBL.FindUser(WhoAmI);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();

            ViewBag.UserName = userData.FullName;

            List<int> fullTeam;
            // Build the OwnerID list based on the user's role
            switch (userData.UserType())
            {
                case UserDataRoleType.Administrator:
                    ViewBag.OwnerID = new SelectList(db.UserDatas.OrderBy(o => o.UserNTID), "UserID", "UserNTID", userData.UserID);
                    break;
                case UserDataRoleType.BusinessUnitOwner:
                    fullTeam = userData.myTeam();
                    fullTeam.Insert(0, userData.UserID);
                    ViewBag.OwnerID = new SelectList(db.UserDatas.Where(o => fullTeam.Contains(o.UserID)), "UserID", "UserNTID", userData.UserID);
                    break;
                case UserDataRoleType.Manager:
                    fullTeam = userData.myTeam();
                    fullTeam.Insert(0, userData.UserID);
                    ViewBag.OwnerID = new SelectList(db.UserDatas.Where(o => fullTeam.Contains(o.UserID)), "UserID", "UserNTID", userData.UserID);
                    break;
                case UserDataRoleType.Individual:
                    ViewBag.OwnerID = new SelectList(db.UserDatas.Where(o => o.UserID == userData.UserID), "UserID", "UserNTID");
                    break;
            }


            // Uncomment the following line to enable Template Version selection in the Create Assessment View
            //ViewBag.TemplateVersion = new SelectList(db.AssessmentTemplates, "TemplateID", "TemplateVersion");

            // Uncomment the following line to enable Industry Target selection in the Create Assessment View
            //ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName");

            ViewBag.UserOrgName = userData.OrgName;
            ViewBag.UserBuName = userData.BusinessUnit;

            return View();
        }

        // POST: Assessments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Create([Bind(Include = "AssessmentID,TemplateVersion,OwnerID,BusinessUnit,OrganizationName,ApplicationName,LastUpdated,LastUpdateBy,CreateDate,CreateBy,IndustryID")] Assessment assessment)
        {
            var WhoAmI = User.Identity.Name;
            var userData = userDataBL.FindUser(WhoAmI);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();

            ViewBag.UserName = userData.FullName;


            if (ModelState.IsValid)
            {

                //If the IndustryID drop list is commented out, the value will be null and needs to be set to the default value of 1
                if (assessment.IndustryID == null)
                {
                    assessment.IndustryID = 1;
                }

                var aid = await assessmentBL.CreateAssessment(assessment.OwnerID, assessment.TemplateVersion, assessment.BusinessUnit, assessment.OrganizationName, assessment.ApplicationName, userData.UserID, assessment.IndustryID.Value);
                if (aid > 0)
                {
                    return RedirectToAction("Update", new { id = aid });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }

            ViewBag.OwnerID = new SelectList(db.UserDatas, "UserID", "UserNTID", assessment.OwnerID);
            // Uncomment the following line to enable Industry Target selection
            //ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName");
            // Uncomment the following line to enable Template Version selection
            //ViewBag.TemplateVersion = new SelectList(db.AssessmentTemplates, "TemplateID", "TemplateVersion");

            return View(assessment);
        }

        // GET: Assessments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userData = userDataBL.FindUser(User.Identity.Name);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();

            ViewBag.UserName = userData.FullName;


            Assessment assessment = assessmentBL.GetAssessmentByAssessmentID(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(id.Value, PeerAuthorizedType.PeerNotAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            switch (userData.UserType())
            {
                case UserDataRoleType.Administrator:
                    //ViewBag.OwnerID = new SelectList(db.UserDatas.OrderBy(o => o.UserID).Where(o => o.BusinessUnit == assessment.BusinessUnit), "UserID", "UserNTID", assessment.OwnerID);
                    ViewBag.OwnerID = new SelectList(db.UserDatas.OrderBy(o => o.UserID), "UserID", "UserNTID", assessment.OwnerID);
                    break;
                case UserDataRoleType.BusinessUnitOwner:
                    ViewBag.OwnerID = new SelectList(db.UserDatas.Where(o => o.UserID == userData.UserID), "UserID", "UserNTID", assessment.OwnerID);
                    break;
                case UserDataRoleType.Manager:
                    List<int> fullTeam = userData.myTeam();
                    fullTeam.Add(userData.UserID);
                    ViewBag.OwnerID = new SelectList(db.UserDatas.OrderBy(o => o.UserID).Where(o => fullTeam.Contains(o.UserID)), "UserID", "UserNTID", assessment.OwnerID);
                    break;
                case UserDataRoleType.Individual:
                    ViewBag.OwnerID = new SelectList(db.UserDatas.Where(o => o.UserID == userData.UserID), "UserID", "UserNTID", assessment.OwnerID);
                    break;
            }

            ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName", assessment.IndustryID);
            return View(assessment);
        }

        // POST: Assessments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssessmentID,TemplateVersion,OwnerID,BusinessUnit,OrganizationName,ApplicationName,LastUpdated,LastUpdateBy,CreateDate,CreateBy,IndustryID")] Assessment assessment)
        {
            var userData = userDataBL.FindUser(User.Identity.Name);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(assessment.AssessmentID, PeerAuthorizedType.PeerNotAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });


            if (ModelState.IsValid)
            {
                assessmentBL.UpdateAssessmentDetail(assessment, userData);
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdateBy = new SelectList(db.UserDatas, "UserID", "UserNTID", assessment.LastUpdateBy);
            ViewBag.OwnerID = new SelectList(db.UserDatas, "UserID", "UserNTID", assessment.OwnerID);
            ViewBag.CreateBy = new SelectList(db.UserDatas, "UserID", "UserNTID", assessment.CreateBy);
            ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName", assessment.IndustryID);
            return View(assessment);
        }

        // GET: Assessments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userData = userDataBL.FindUser(User.Identity.Name);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            Assessment assessment = assessmentBL.GetAssessmentByAssessmentID(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(id.Value, PeerAuthorizedType.PeerNotAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            return View(assessment);
        }

        // POST: Assessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userData = userDataBL.FindUser(User.Identity.Name);
            Assessment assessment = assessmentBL.GetAssessmentDetail(id);

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(id, PeerAuthorizedType.PeerNotAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            assessmentBL.DeleteAssessment(id);
            return RedirectToAction("Index");
        }



        // UPDATE: Assessments/Update/5
        public ActionResult Update(int? id, bool readOnly = false, string callBack = "index")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userData = userDataBL.FindUser(User.Identity.Name);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;
            ViewBag.ReadOnly = readOnly;
            ViewBag.CallBack = callBack;

            // Fetch the assessment data
            Assessment assessment = assessmentBL.GetAssessmentByAssessmentID(id);

            if (assessment == null)
            {
                return HttpNotFound();
            }

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(assessment.AssessmentID, PeerAuthorizedType.PeerAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            Session["Assessment"] = assessment;

            return View(assessment);
        }

        // POST: Assessments/Update/5
        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int? id, string[] assessmentResponse, string btnclick)
        {
            var WhoAmI = User.Identity.Name;
            var userData = userDataBL.FindUser(WhoAmI);
            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Check that assessment ID is a team members assessment
            Assessment assessment = (Assessment)Session["Assessment"];
            // If the assessment number is changed or hacked, throw the error
            if (assessment.AssessmentID != id.Value)
            {
                Session["Assessment"] = null;
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });
            }

            if (!userData.AssessmentAuthorized(assessment.AssessmentID, PeerAuthorizedType.PeerAuthorized))
            {
                Session["Assessment"] = null;
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });
            }

            // Call UpdateAssessmentData to update the answers with the data from the checkboxes on the screen.  Checkbox
            // data is being passed in the string array "assessmentResponse".  The array elements contain the QuestionID associated with
            // each checkbox that was checked.  Unchecked checkboxes are not passed in the array.
            assessmentBL.UpdateAssessmentData(id, assessmentResponse, userData.UserID);

            if (btnclick == "Save & Submit")
            {
                assessmentBL.FinalizeAssessment(id.Value, userData.UserID);
            }

            Session["Assessment"] = null;

            return RedirectToAction("Index");
        }

        /// <summary>
        /// ToggleCheckBox - this method is called via Ajax from the Update Assessment page whenever the user clicks on a checkbox.  The
        /// method retrieves the copy of the Assessment that is stored in Session and uses it to update the score.  The AssessmentID is stored
        /// in the view.  When the user clicks on a checkbox, a JavaScript function makes an ajax call to pass the QuestionID, new checkbox value,
        /// and AssessmentID to this method.  The method recomputes the section score and returns the new values to the Update view to the ajax call.
        /// The JavaScript function finishes by applying the score update to the screen.
        /// 
        /// </summary>
        /// <param name="cbQuestionID">string</param>
        /// <param name="cbChecked">bool</param>
        /// <param name="AssessmentID">string</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ToggleCheckBox(string cbQuestionID, bool cbChecked, string AssessmentID)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    Assessment sessionAssessment = new Assessment();
                    int qVal, aVal = 0;

                    if (!int.TryParse(cbQuestionID, out qVal))
                    {
                        throw new ApplicationException("Error: AssessmentController.ToggleCheckBox: non numeric QuestionID " + cbQuestionID);
                    }

                    if (!int.TryParse(AssessmentID, out aVal))
                    {
                        throw new ApplicationException("Error: AssessmentController.ToggleCheckBox: non numeric AssessmentID " + AssessmentID);
                    }

                    if (Session["Assessment"] != null)
                    {
                        sessionAssessment = (Assessment)Session["Assessment"];
                        if (sessionAssessment.AssessmentID != aVal)
                            throw new ApplicationException("Error: AssessmentController.ToggleCheckBox: AssessmentID changed");
                    }
                    else
                        throw new ApplicationException("Error: AssessmentController.ToggleCheckBox: Cached Assessment is null");

                    // Update the Assessment score
                    sessionAssessment = assessmentBL.UpdateAssessmentScore(sessionAssessment, qVal, cbChecked);

                    var x = from o in sessionAssessment.CategoryDatas
                            from p in o.SectionDatas
                            from q in p.GroupDatas
                            from r in q.QuestionDatas
                            where r.QuestionID == qVal
                            select new { section = p.SectionID, sectionScore = p.SectionScore, sectionPartial = p.SectionScorePartial }
                            ;

                    char nbsp = (char)0xA0;
                    string scoreID = "Score" + x.Single().section.ToString();
                    string scoreValue = "Score:" + nbsp.ToString() + x.Single().sectionScore.ToString();
                    if (x.Single().sectionPartial > 0) scoreValue += "+";

                    // Update the session item with the updated object
                    Session["Assessment"] = sessionAssessment;

                    return Json(new { ScoreID = scoreID, ScoreValue = scoreValue });
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error: AssessmentController.ToggleCheckBox ", ex);
                }
            }

            return new JsonResult();
        }


        public ActionResult Scorecard(int? id, string callBack = "index")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;
            ViewBag.Callback = callBack;

            Assessment assessment = assessmentBL.GetAsessmentScorecard(id);

            // Check that assessment ID is a team members assessment
            if (!userData.AssessmentAuthorized(assessment.AssessmentID, PeerAuthorizedType.PeerAuthorized))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            if (assessment == null)
            {
                return HttpNotFound();
            }
            return View(assessment);
        }

        public ActionResult BUScorecard(string BusinessUnit)
        {
            //if (BusinessUnit == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            Assessment BUassessment = assessmentBL.GetAsessmentScorecardAverage(BusinessUnit);

            if (BUassessment == null)
            {
                return HttpNotFound();
            }

            return View(BUassessment);

        }

        public ActionResult BUSummary()
        {
            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            // Access to screen is limited to Admins and BU Owners
            if (!(userData.UserType() == UserDataRoleType.Administrator || userData.UserType() == UserDataRoleType.BusinessUnitOwner))
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for this action" });

            Models.BusinessUnitSummaryVM BUSummaryVM = new Models.BusinessUnitSummaryVM();
            BUSummaryVM.BUOwner = userData;

            if (userData.IsAdministrator())
            {
                // If the user is an admin, then build the viewmodel with assessments for the first business unit name
                BUSummaryVM.BUList = assessmentBL.GetFinalizedAssessments().OrderBy(o => o.BusinessUnit).Select(o => o.BusinessUnit).Distinct().ToList();
                BUSummaryVM.BUAssessments = assessmentBL.GetAssessmentsByBU(BUSummaryVM.BUList.FirstOrDefault());
            }
            else
            {
                // If the user is a business unit owner then build the viewmodel with the user's business unit
                BUSummaryVM.BUAssessments = assessmentBL.GetAssessmentsByBU(userData.BusinessUnit);
                BUSummaryVM.BUList = new List<string>() { userData.BusinessUnit };
            }

            return View(BUSummaryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BUSummary(string bulist)
        {
            return RedirectToAction("BUScorecard", new { BusinessUnit = bulist });
        }

        public ActionResult ChangeBUList(string buName)
        {
            var alist = assessmentBL.GetAssessmentsByBU(buName);
            return PartialView("_BUSummaryList", alist);
        }

        public ActionResult Export2Excel()
        {
            var assessment = Session["TempScorecard"] as Assessment;

            if (assessment == null)
            {
                return HttpNotFound();
            }
            var memoryStream = assessmentBL.Export2Excel(assessment);

            HttpResponseBase response = Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("content-disposition", "attachment;filename=\"" + assessment.ApplicationName + " vs " + assessment.Industry.IndustryName + ".xlsx\"");
            memoryStream.WriteTo(response.OutputStream);
            memoryStream.Close();
            response.End();
            return new EmptyResult();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
