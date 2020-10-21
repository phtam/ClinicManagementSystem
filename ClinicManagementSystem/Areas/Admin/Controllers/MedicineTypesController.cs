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
    public class MedicineTypesController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();

        // GET: Admin/MedicineTypes
        public ActionResult Index()
        {
            return View(db.MedicineTypes.ToList());
        }

        // GET: Admin/MedicineTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineType medicineType = db.MedicineTypes.Find(id);
            if (medicineType == null)
            {
                return HttpNotFound();
            }
            return View(medicineType);
        }

        // GET: Admin/MedicineTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MedicineTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,Description")] MedicineType medicineType)
        {
            if (ModelState.IsValid)
            {
                db.MedicineTypes.Add(medicineType);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(medicineType);
        }

        // GET: Admin/MedicineTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineType medicineType = db.MedicineTypes.Find(id);
            if (medicineType == null)
            {
                return HttpNotFound();
            }
            return View(medicineType);
        }

        // POST: Admin/MedicineTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,Description")] MedicineType medicineType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicineType).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                    TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(medicineType);
        }

        // GET: Admin/MedicineTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineType medicineType = db.MedicineTypes.Find(id);
            if (medicineType == null)
            {
                return HttpNotFound();
            }
            return View(medicineType);
        }

        // POST: Admin/MedicineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                MedicineType medicineType = db.MedicineTypes.Find(id);
                db.MedicineTypes.Remove(medicineType);
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
