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
    public class MedicineFeedbacksController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();

        // GET: Admin/MedicineFeedbacks
        public ActionResult Index()
        {
            var medicineFeedbacks = db.MedicineFeedbacks.Include(m => m.Customer).Include(m => m.Medicine);
            return View(medicineFeedbacks.ToList());
        }

        // GET: Admin/MedicineFeedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineFeedback medicineFeedback = db.MedicineFeedbacks.Find(id);
            if (medicineFeedback == null)
            {
                return HttpNotFound();
            }
            return View(medicineFeedback);
        }

        // GET: Admin/MedicineFeedbacks/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password");
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "MedicineName");
            return View();
        }

        // POST: Admin/MedicineFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,Username,MedicineID,Content,CreatedDate")] MedicineFeedback medicineFeedback)
        {
            if (ModelState.IsValid)
            {
                db.MedicineFeedbacks.Add(medicineFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", medicineFeedback.Username);
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "MedicineName", medicineFeedback.MedicineID);
            return View(medicineFeedback);
        }

        // GET: Admin/MedicineFeedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineFeedback medicineFeedback = db.MedicineFeedbacks.Find(id);
            if (medicineFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", medicineFeedback.Username);
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "MedicineName", medicineFeedback.MedicineID);
            return View(medicineFeedback);
        }

        // POST: Admin/MedicineFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,Username,MedicineID,Content,CreatedDate")] MedicineFeedback medicineFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicineFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", medicineFeedback.Username);
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "MedicineName", medicineFeedback.MedicineID);
            return View(medicineFeedback);
        }

        // GET: Admin/MedicineFeedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineFeedback medicineFeedback = db.MedicineFeedbacks.Find(id);
            if (medicineFeedback == null)
            {
                return HttpNotFound();
            }
            return View(medicineFeedback);
        }

        // POST: Admin/MedicineFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicineFeedback medicineFeedback = db.MedicineFeedbacks.Find(id);
            db.MedicineFeedbacks.Remove(medicineFeedback);
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
