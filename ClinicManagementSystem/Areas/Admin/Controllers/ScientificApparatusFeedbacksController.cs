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
    public class ScientificApparatusFeedbacksController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();

        // GET: Admin/ScientificApparatusFeedbacks
        public ActionResult Index()
        {
            var scientificApparatusFeedbacks = db.ScientificApparatusFeedbacks.Include(s => s.Customer).Include(s => s.ScientificApparatu);
            return View(scientificApparatusFeedbacks.ToList());
        }

        // GET: Admin/ScientificApparatusFeedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusFeedback scientificApparatusFeedback = db.ScientificApparatusFeedbacks.Find(id);
            if (scientificApparatusFeedback == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatusFeedback);
        }

        // GET: Admin/ScientificApparatusFeedbacks/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password");
            ViewBag.ScientificApparatusID = new SelectList(db.ScientificApparatus, "ScientificApparatusID", "ScientificApparatusName");
            return View();
        }

        // POST: Admin/ScientificApparatusFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,Username,ScientificApparatusID,Content,CreatedDate")] ScientificApparatusFeedback scientificApparatusFeedback)
        {
            if (ModelState.IsValid)
            {
                db.ScientificApparatusFeedbacks.Add(scientificApparatusFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", scientificApparatusFeedback.Username);
            ViewBag.ScientificApparatusID = new SelectList(db.ScientificApparatus, "ScientificApparatusID", "ScientificApparatusName", scientificApparatusFeedback.ScientificApparatusID);
            return View(scientificApparatusFeedback);
        }

        // GET: Admin/ScientificApparatusFeedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusFeedback scientificApparatusFeedback = db.ScientificApparatusFeedbacks.Find(id);
            if (scientificApparatusFeedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", scientificApparatusFeedback.Username);
            ViewBag.ScientificApparatusID = new SelectList(db.ScientificApparatus, "ScientificApparatusID", "ScientificApparatusName", scientificApparatusFeedback.ScientificApparatusID);
            return View(scientificApparatusFeedback);
        }

        // POST: Admin/ScientificApparatusFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,Username,ScientificApparatusID,Content,CreatedDate")] ScientificApparatusFeedback scientificApparatusFeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scientificApparatusFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", scientificApparatusFeedback.Username);
            ViewBag.ScientificApparatusID = new SelectList(db.ScientificApparatus, "ScientificApparatusID", "ScientificApparatusName", scientificApparatusFeedback.ScientificApparatusID);
            return View(scientificApparatusFeedback);
        }

        // GET: Admin/ScientificApparatusFeedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusFeedback scientificApparatusFeedback = db.ScientificApparatusFeedbacks.Find(id);
            if (scientificApparatusFeedback == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatusFeedback);
        }

        // POST: Admin/ScientificApparatusFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScientificApparatusFeedback scientificApparatusFeedback = db.ScientificApparatusFeedbacks.Find(id);
            db.ScientificApparatusFeedbacks.Remove(scientificApparatusFeedback);
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
