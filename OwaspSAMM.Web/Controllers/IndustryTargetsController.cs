using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OwaspSAMM.DAL;

namespace OwaspSAMM.Web
{
    public class IndustryTargetsController : Controller
    {
        private OwaspSAMMContext db = new OwaspSAMMContext();
        UserDataBL userDataBL = new UserDataBL();

        // GET: IndustryTargets
        public ActionResult Index()
        {
            var industryTargets = db.IndustryTargets.Include(i => i.Category).Include(i => i.Industry).Include(i => i.Section).Include(i => i.IndustryTargetScoreValue);

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for industry target maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(industryTargets.ToList());
        }

        // GET: IndustryTargets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndustryTarget industryTarget = db.IndustryTargets.Find(id);
            if (industryTarget == null)
            {
                return HttpNotFound();
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for industry target maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(industryTarget);
        }

        // GET: IndustryTargets/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName");
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName");
            ViewBag.Score = new SelectList(db.IndustryTargetScoreValues, "Score", "Score");

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for industry target maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View();
        }

        // POST: IndustryTargets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryID,SectionID,IndustryID,Score")] IndustryTarget industryTarget)
        {
            if (ModelState.IsValid)
            {
                db.IndustryTargets.Add(industryTarget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", industryTarget.CategoryID);
            ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName", industryTarget.IndustryID);
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", industryTarget.SectionID);
            ViewBag.Score = new SelectList(db.IndustryTargetScoreValues, "Score", "Score", industryTarget.Score);
            return View(industryTarget);
        }

        // GET: IndustryTargets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndustryTarget industryTarget = db.IndustryTargets.Find(id);
            if (industryTarget == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", industryTarget.CategoryID);
            ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName", industryTarget.IndustryID);
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", industryTarget.SectionID);
            ViewBag.Score = new SelectList(db.IndustryTargetScoreValues, "Score", "Score", industryTarget.Score);

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for industry target maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(industryTarget);
        }

        // POST: IndustryTargets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryID,SectionID,IndustryID,Score")] IndustryTarget industryTarget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(industryTarget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", industryTarget.CategoryID);
            ViewBag.IndustryID = new SelectList(db.Industries, "IndustryID", "IndustryName", industryTarget.IndustryID);
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", industryTarget.SectionID);
            ViewBag.Score = new SelectList(db.IndustryTargetScoreValues, "Score", "Score", industryTarget.Score);
            return View(industryTarget);
        }

        // GET: IndustryTargets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndustryTarget industryTarget = db.IndustryTargets.Find(id);
            if (industryTarget == null)
            {
                return HttpNotFound();
            }

            var userData = userDataBL.FindUser(User.Identity.Name);

            if (userData.IsAdministrator() != true)
            {
                return RedirectToAction("NotAuthorized", "Home", new { message = "You are not authorized for industry target maintenance screens!!" });
            }

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;


            return View(industryTarget);
        }

        // POST: IndustryTargets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IndustryTarget industryTarget = db.IndustryTargets.Find(id);
            db.IndustryTargets.Remove(industryTarget);
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
