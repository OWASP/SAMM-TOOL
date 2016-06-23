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
using System.Text.RegularExpressions;

namespace OwaspSAMM.Web.Controllers
{
    public class UserDataController : Controller
    {
        private OwaspSAMMContext db = new OwaspSAMMContext();
        UserDataBL userDataBL = new UserDataBL();

        // GET: /UserData/
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            
            var UserDatas = from u in db.UserDatas
                           select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                UserDatas = UserDatas.Where(u => u.LastName.ToUpper().Contains(searchString.ToUpper()));
            }

            UserDatas = UserDatas.OrderBy(u => u.LastName);

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();

            
            ViewBag.UserName = userData.FullName;


            int pageSize = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["PageSize"].ToString());
            int pageNumber = (page ?? 1);
            return View(UserDatas.ToPagedList(pageNumber, pageSize));

           
        }

        // GET: /UserData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userdata = db.UserDatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(userdata);
        }

        [HttpPost, ActionName("Details")]
        // POST: UserData/Details
        public ActionResult DetailsEdit(int id)
        {
            return RedirectToAction("Edit", new { id });
        }

        // GET: /UserData/Create
        public ActionResult Create()
        {

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }


            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View();
        }

        // POST: /UserData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserNTID,UserDomain,LastName,FirstName,ManagerID,MgrLastName,MgrFirstName,ManagerEID,OrgName,LastLoginDate,Administrator,BusinessUnit,BUOwner")] UserData userdata)
        {
            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            if (ModelState.IsValid)
            {

                string whitelist = System.Web.Configuration.WebConfigurationManager.AppSettings["WhiteListNTID"];
                string UserNTID = userdata.UserNTID.ToString();
                Regex pattern = new Regex(whitelist);
                if (!pattern.IsMatch(UserNTID))
                {
                    ModelState.AddModelError("UserNTID", "NT ID contains an invalid value");
                    return View(userdata);
                }
                
                whitelist = System.Web.Configuration.WebConfigurationManager.AppSettings["WhiteListDomain"];
                string UserDomain = userdata.UserDomain.ToString();
                pattern = new Regex(whitelist);
                if (!pattern.IsMatch(UserDomain)) 
                {
                    ModelState.AddModelError("UserDomain", "Domain contains an invalid value");
                    return View(userdata);
                }

                               
                var userExists = userDataBL.GetUserData(userdata.UserDomain + "\\" + userdata.UserNTID);
                if (userExists != null)
                {
                    ModelState.AddModelError("UserNTID", "NT ID and Domain combination already exists");
                    ModelState.AddModelError("UserDomain", "NT ID and Domain combination already exists");
                    return View(userdata);
                }

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPEnabled"] == "true")
                {
                    if (userDataBL.UserinLDAP(userdata) == false)
                    {
                        ModelState.AddModelError("UserNTID", "NT ID and Domain combination not found in LDAP");
                        ModelState.AddModelError("UserDomain", "NT ID and Domain combination not found in LDAP");
                        return View(userdata);
                    }
                }

                userdata.UserDomain = userdata.UserDomain.ToUpper();
                db.UserDatas.Add(userdata);
                db.SaveChanges();
                // update the user's LDAP information if LDAP is being used
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPEnabled"] == "true")
                {
                    userDataBL.LogUserLogin(userdata, false);
                }
                return RedirectToAction("Index");
            }

            return View(userdata);
        }

        // GET: /UserData/Edit/5
        public ActionResult Transfer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userdata = db.UserDatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }

            ViewBag.OwnerID = new SelectList(db.UserDatas.OrderBy(o => o.UserNTID).Where(o => o.UserID != id.Value && o.BusinessUnit == userdata.BusinessUnit), "UserID", "UserNTID");

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(userdata);
        }

        // POST: /UserData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer([Bind(Include = "UserID,UserNTID,UserDomain,LastName,FirstName,ManagerID,MgrLastName,MgrFirstName,ManagerEID,OrgName,LastLoginDate,Administrator,BusinessUnit,BUOwner")] UserData userdata, int? OwnerID)
        {
            if (OwnerID.HasValue)
            {
                var myUserData = userDataBL.FindUser(User.Identity.Name);

                new AssessmentBL().UpdateAssessmentOwner(userdata.UserID, OwnerID.Value, myUserData.UserID);
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.UserDatas.OrderBy(o => o.UserNTID).Where(o => o.UserID != userdata.UserID), "UserID", "UserNTID");
            userdata = db.UserDatas.Find(userdata.UserID);  //Refresh data
            return View(userdata);
        }

        // GET: /UserData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userdata = db.UserDatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(userdata);
        }

        // POST: /UserData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserNTID,UserDomain,LastName,FirstName,ManagerID,MgrLastName,MgrFirstName,ManagerEID,OrgName,LastLoginDate,Administrator,BusinessUnit,BUOwner")] UserData userdata)
        {
                        
            if (ModelState.IsValid)
            {
                db.Entry(userdata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userdata);
        }

        // GET: /UserData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userdata = db.UserDatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for user maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(userdata);
        }

        // POST: /UserData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserData userdata = db.UserDatas.Find(id);
            db.UserDatas.Remove(userdata);
            db.SaveChanges();
            return RedirectToAction("Index");
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
