using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.EF;
using ClinicManagementSystem.Provider;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    public class MedicinesController : Controller
    {
        private ClinicData db = new ClinicData();
        private ImageProvider imgProvider = new ImageProvider();

        // GET: Admin/Medicines
        public ActionResult Index()
        {
            var medicines = db.Medicines.Include(m => m.Supplier).Include(m => m.MedicineType);
            return View(medicines.ToList());
        }

        // GET: Admin/Medicines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // GET: Admin/Medicines/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: Admin/Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicineID,TypeID,SupplierID,MedicineName,ShortDescription,Composition,Usage,Quantity,OldUnitPrice,UnitPrice,Thumbnail,Status,ImageFile")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                //if (db.Medicines.Find(medicine.MedicineName) != null)
                //{
                //    ViewBag.Error = "Medicine Name already exists";
                //    return View("Create");
                //}

                if (imgProvider.Validate(medicine.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(medicine.ImageFile);
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(medicine.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(medicine.ImageFile.FileName);

                medicine.Thumbnail = "~/public/uploadedFiles/medicinePictures/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/medicinePictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                medicine.ImageFile.SaveAs(fileName);
                db.Medicines.Add(medicine);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");

            }


            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
            return View(medicine);
        }

        // GET: Admin/Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
            return View(medicine);
        }

        // POST: Admin/Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicineID,TypeID,SupplierID,MedicineName,ShortDescription,Composition,Usage,Quantity,OldUnitPrice,UnitPrice,Thumbnail,Status,ImageFile")] Medicine medicine, String userOldImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (medicine.ImageFile == null)
            {
                medicine.Thumbnail = userOldImage;
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(medicine.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(medicine.ImageFile.FileName);

                medicine.Thumbnail = "~/public/uploadedFiles/customerPictures/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/customerPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                if (userOldImage != null)
                {
                    System.IO.File.Delete(Server.MapPath(userOldImage));
                }

                medicine.ImageFile.SaveAs(fileName);

            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
            return View(medicine);
        }

        // GET: Admin/Medicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: Admin/Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicine medicine = db.Medicines.Find(id);
            db.Medicines.Remove(medicine);
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
