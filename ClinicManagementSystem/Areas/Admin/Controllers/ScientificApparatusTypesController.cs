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
    public class ScientificApparatusTypesController : Controller
    {
        private ClinicData db = new ClinicData();

        // GET: Admin/ScientificApparatusTypes
        public ActionResult Index()
        {
            return View(db.ScientificApparatusTypes.ToList());
        }

        // GET: Admin/ScientificApparatusTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusType scientificApparatusType = db.ScientificApparatusTypes.Find(id);
            if (scientificApparatusType == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatusType);
        }

        // GET: Admin/ScientificApparatusTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ScientificApparatusTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,Description")] ScientificApparatusType scientificApparatusType)
        {
            if (ModelState.IsValid)
            {
                db.ScientificApparatusTypes.Add(scientificApparatusType);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(scientificApparatusType);
        }

        // GET: Admin/ScientificApparatusTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusType scientificApparatusType = db.ScientificApparatusTypes.Find(id);
            if (scientificApparatusType == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatusType);
        }

        // POST: Admin/ScientificApparatusTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,Description")] ScientificApparatusType scientificApparatusType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scientificApparatusType).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                    TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(scientificApparatusType);
        }

        // GET: Admin/ScientificApparatusTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusType scientificApparatusType = db.ScientificApparatusTypes.Find(id);
            if (scientificApparatusType == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatusType);
        }

        // POST: Admin/ScientificApparatusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ScientificApparatusType scientificApparatusType = db.ScientificApparatusTypes.Find(id);
                db.ScientificApparatusTypes.Remove(scientificApparatusType);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Delete_Success"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
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
