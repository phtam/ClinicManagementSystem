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
    public class ScientificApparatusController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            var scientificApparatus = db.ScientificApparatus.Include(s => s.Supplier).Include(s => s.ScientificApparatusType);
            return View(scientificApparatus.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatu scientificApparatu = db.ScientificApparatus.Find(id);
            if (scientificApparatu == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatu);
        }

        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.TypeID = new SelectList(db.ScientificApparatusTypes, "TypeID", "TypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ScientificApparatusID,TypeID,SupplierID,ScientificApparatusName,ShortDescription,Description,Specification,UnitInStock,UnitOnOrder,OldUnitPrice,UnitPrice,Thumbnail,Status,ImageFile")] ScientificApparatu scientificApparatu)
        {
            if (ModelState.IsValid)
            {
                if (imgProvider.Validate(scientificApparatu.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(scientificApparatu.ImageFile);
                    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", scientificApparatu.SupplierID);
                    ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", scientificApparatu.TypeID);
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(scientificApparatu.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(scientificApparatu.ImageFile.FileName);

                scientificApparatu.Thumbnail = "~/public/uploadedFiles/scientificApparatusPictures/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/scientificApparatusPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                scientificApparatu.ImageFile.SaveAs(fileName);

                scientificApparatu.UnitInStock = 0;
                scientificApparatu.UnitOnOrder = 0;

                db.ScientificApparatus.Add(scientificApparatu);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", scientificApparatu.SupplierID);
            ViewBag.TypeID = new SelectList(db.ScientificApparatusTypes, "TypeID", "TypeName", scientificApparatu.TypeID);
            return View(scientificApparatu);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatu scientificApparatu = db.ScientificApparatus.Find(id);
            Session["OLD_SCIENTIFIC_APPARATUS_IMAGE"] = scientificApparatu.Thumbnail;
            if (scientificApparatu == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", scientificApparatu.SupplierID);
            ViewBag.TypeID = new SelectList(db.ScientificApparatusTypes, "TypeID", "TypeName", scientificApparatu.TypeID);
            return View(scientificApparatu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ScientificApparatusID,TypeID,SupplierID,ScientificApparatusName,ShortDescription,Description,Specification,UnitInStock,UnitOnOrder,OldUnitPrice,UnitPrice,Thumbnail,Status,ImageFile")] ScientificApparatu scientificApparatu, String scientificApparatusOldImage)
        {
            if (ModelState.IsValid)
            {
                if (scientificApparatu.ImageFile == null)
                {
                    scientificApparatu.Thumbnail = scientificApparatusOldImage;
                }
                else
                {
                    if (imgProvider.Validate(scientificApparatu.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(scientificApparatu.ImageFile);
                        ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", scientificApparatu.SupplierID);
                        ViewBag.TypeID = new SelectList(db.MedicineTypes, "TypeID", "TypeName", scientificApparatu.TypeID);
                        return View(scientificApparatu);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(scientificApparatu.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(scientificApparatu.ImageFile.FileName);

                    scientificApparatu.Thumbnail = "~/public/uploadedFiles/scientificApparatusPictures/" + fileName;

                    string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/scientificApparatusPictures/");

                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    if (scientificApparatusOldImage != null)
                    {
                        System.IO.File.Delete(Server.MapPath(scientificApparatusOldImage));
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    scientificApparatu.ImageFile.SaveAs(fileName);
                }


                db.Entry(scientificApparatu).State = EntityState.Modified;
                db.Entry(scientificApparatu).Property(x => x.UnitInStock).IsModified = false;
                db.Entry(scientificApparatu).Property(x => x.UnitOnOrder).IsModified = false;
                if (db.SaveChanges() > 0)
                    TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", scientificApparatu.SupplierID);
            ViewBag.TypeID = new SelectList(db.ScientificApparatusTypes, "TypeID", "TypeName", scientificApparatu.TypeID);
            return View(scientificApparatu);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatu scientificApparatu = db.ScientificApparatus.Find(id);
            if (scientificApparatu == null)
            {
                return HttpNotFound();
            }
            return View(scientificApparatu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ScientificApparatu scientificApparatu = db.ScientificApparatus.Find(id);
                db.ScientificApparatus.Remove(scientificApparatu);
                if (db.SaveChanges() > 0)
                {
                    if (scientificApparatu.Thumbnail != null)
                    {
                        System.IO.File.Delete(Server.MapPath(scientificApparatu.Thumbnail));
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
            ScientificApparatu scientificApparatus = db.ScientificApparatus.Find(id);
            ViewBag.ScientificApparatus = scientificApparatus;

            var model = db.ScientificApparatusImages.Where(x => x.ScientificApparatusID == id);

            if (scientificApparatus == null)
            {
                return HttpNotFound();
            }
            return View(model.ToList());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Image([Bind(Include = "ImageID,FileName,ScientificApparatusID,ImageFile")] ScientificApparatusImage apparatusImage)
        {
            if (ModelState.IsValid)
            {
                if (imgProvider.Validate(apparatusImage.ImageFile) != null)
                {
                    TempData["Error"] = imgProvider.Validate(apparatusImage.ImageFile);
                    return RedirectToAction("Image", new { id = apparatusImage.ScientificApparatusID });
                }

                string fileName = Path.GetFileNameWithoutExtension(apparatusImage.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(apparatusImage.ImageFile.FileName);

                apparatusImage.FileName = "~/public/uploadedFiles/scientificApparatusImages/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/scientificApparatusImages/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                apparatusImage.ImageFile.SaveAs(fileName);

                db.ScientificApparatusImages.Add(apparatusImage);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;

                return RedirectToAction("Image", new { id = apparatusImage.ScientificApparatusID });

            }

            return RedirectToAction("Image", new { id = apparatusImage.ScientificApparatusID });
        }


        public ActionResult DeleteImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScientificApparatusImage scientificApparatusImages = db.ScientificApparatusImages.Find(id);
            var scientificApparatusID = scientificApparatusImages.ScientificApparatusID;
            try
            {
                db.ScientificApparatusImages.Remove(scientificApparatusImages);
                if (db.SaveChanges() > 0)
                {
                    if (scientificApparatusImages.FileName != null)
                    {
                        System.IO.File.Delete(Server.MapPath(scientificApparatusImages.FileName));
                    }
                    TempData["Notice_Delete_Success"] = true;
                }
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }

            return RedirectToAction("Image", new { id = scientificApparatusID });
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
