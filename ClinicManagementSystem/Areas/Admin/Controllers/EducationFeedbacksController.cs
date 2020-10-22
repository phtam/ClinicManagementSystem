using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.EF;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    public class EducationFeedbacksController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();

        // GET: Admin/EducationFeedbacks
        public ActionResult Index()
        {
            var educationFeedbacks = db.EducationFeedbacks.Include(e => e.Customer).Include(e => e.Education);
            return View(educationFeedbacks.ToList());
        }

        // GET: Admin/EducationFeedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationFeedback educationFeedback = db.EducationFeedbacks.Find(id);
            if (educationFeedback == null)
            {
                return HttpNotFound();
            }
            return View(educationFeedback);
        }

        // GET: Admin/EducationFeedbacks/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password");
            ViewBag.EducationID = new SelectList(db.Educations, "EducationID", "LessonName");
            return View();
        }

        // POST: Admin/EducationFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,Username,EducationID,Content,CreatedDate")] EducationFeedback educationFeedback)
        {
            if (ModelState.IsValid)
            {
                db.EducationFeedbacks.Add(educationFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", educationFeedback.Username);
            ViewBag.EducationID = new SelectList(db.Educations, "EducationID", "LessonName", educationFeedback.EducationID);
            return View(educationFeedback);
        }

        // GET: Admin/EducationFeedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationFeedback educationFeedback = db.EducationFeedbacks.Find(id);
            if (educationFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", educationFeedback.Username);
            ViewBag.EducationID = new SelectList(db.Educations, "EducationID", "LessonName", educationFeedback.EducationID);
            return View(educationFeedback);
        }

        // POST: Admin/EducationFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,Username,EducationID,Content,CreatedDate")] EducationFeedback educationFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(educationFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", educationFeedback.Username);
            ViewBag.EducationID = new SelectList(db.Educations, "EducationID", "LessonName", educationFeedback.EducationID);
            return View(educationFeedback);
        }

        // GET: Admin/EducationFeedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationFeedback educationFeedback = db.EducationFeedbacks.Find(id);
            if (educationFeedback == null)
            {
                return HttpNotFound();
            }
            return View(educationFeedback);
        }

        // POST: Admin/EducationFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EducationFeedback educationFeedback = db.EducationFeedbacks.Find(id);
            db.EducationFeedbacks.Remove(educationFeedback);
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
