﻿using System;
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
    [Authorize]
    public class MedicinesController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            var medicines = db.Medicines.Include(m => m.Supplier).Include(m => m.MedicineType);
            return View(medicines.ToList());
        }

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

        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MedicineID,TypeID,SupplierID,MedicineName,ShortDescription,Composition,Usage,Quantity,OldUnitPrice,UnitPrice,Thumbnail,Status,ImageFile")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                if (imgProvider.Validate(medicine.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(medicine.ImageFile);
                    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
                    ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
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

                medicine.UnitInStock = 0;
                medicine.UnitOnOrder = 0;

                db.Medicines.Add(medicine);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");

            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
            return View(medicine);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            Session["OLD_MEDICINE_IMAGE"] = medicine.Thumbnail;
            if (medicine == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
            return View(medicine);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MedicineID,TypeID,SupplierID,MedicineName,ShortDescription,Composition,Usage,Quantity,OldUnitPrice,UnitPrice,Thumbnail,Status,ImageFile")] Medicine medicine, String medicineOldImage)
        {
            if (ModelState.IsValid)
            {
                if (medicine.ImageFile == null)
                {
                    medicine.Thumbnail = medicineOldImage;
                }
                else
                {
                    if (imgProvider.Validate(medicine.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(medicine.ImageFile);
                        ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
                        ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
                        return View(medicine);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(medicine.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(medicine.ImageFile.FileName);

                    medicine.Thumbnail = "~/public/uploadedFiles/medicinePictures/" + fileName;

                    string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/medicinePictures/");

                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    if (medicineOldImage != null)
                    {
                        System.IO.File.Delete(Server.MapPath(medicineOldImage));
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    medicine.ImageFile.SaveAs(fileName);
                }
                

                db.Entry(medicine).State = EntityState.Modified;
                db.Entry(medicine).Property(x => x.UnitInStock).IsModified = false;
                db.Entry(medicine).Property(x => x.UnitOnOrder).IsModified = false;
                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OLD_MEDICINE_IMAGE");
                    TempData["Notice_Save_Success"] = true;
                }
                    
                return RedirectToAction("Index");
            }
            
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", medicine.SupplierID);
            ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", medicine.TypeID);
            return View(medicine);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Medicine medicine = db.Medicines.Find(id);
                db.Medicines.Remove(medicine);
                if (db.SaveChanges() > 0)
                {
                    if (medicine.Thumbnail != null)
                    {
                        System.IO.File.Delete(Server.MapPath(medicine.Thumbnail));
                    }
                    TempData["Notice_Delete_Success"] = true;
                }
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Image(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            ViewBag.Medicine = medicine;

            var model = db.MedicineImages.Where(x => x.MedicineID == id);

            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(model.ToList());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Image([Bind(Include = "ImageID,FileName,MedicineID,ImageFile")] MedicineImage medicineImage)
        {
            if (ModelState.IsValid)
            {
                if (imgProvider.Validate(medicineImage.ImageFile) != null)
                {
                    TempData["Error"] = imgProvider.Validate(medicineImage.ImageFile);
                    return RedirectToAction("Image", new { id = medicineImage.MedicineID });
                }

                string fileName = Path.GetFileNameWithoutExtension(medicineImage.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(medicineImage.ImageFile.FileName);

                medicineImage.FileName = "~/public/uploadedFiles/medicineImages/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/medicineImages/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                medicineImage.ImageFile.SaveAs(fileName);

                db.MedicineImages.Add(medicineImage);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;

                return RedirectToAction("Image", new { id = medicineImage.MedicineID });

            }

            return RedirectToAction("Image", new { id = medicineImage.MedicineID });
        }


        public ActionResult DeleteImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineImage medicineImage = db.MedicineImages.Find(id);
            var medicineID = medicineImage.MedicineID;
            try
            {
                db.MedicineImages.Remove(medicineImage);
                if (db.SaveChanges() > 0)
                {
                    if (medicineImage.FileName != null)
                    {
                        System.IO.File.Delete(Server.MapPath(medicineImage.FileName));
                    }
                    TempData["Notice_Delete_Success"] = true;
                }
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }

            return RedirectToAction("Image", new { id = medicineID });
        }

        [HttpPost]
        public ActionResult StockIn(int unitInStock, int medicineID)
        {
            var medicine = db.Medicines.Find(medicineID);
            medicine.UnitInStock += unitInStock;
            if (db.SaveChanges() > 0)
                TempData["Notice_Save_Success"] = true;
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
