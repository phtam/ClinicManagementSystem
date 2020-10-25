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
    [Authorize]
    public class EducationsController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            var educations = db.Educations.Include(e => e.Activity).Include(e => e.Subject);
            return View(educations.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        public ActionResult Create()
        {
            ViewBag.ActivityID = new SelectList(db.Activities, "ActivityID", "ActivityName");
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "EducationID,LessonName,ActivityID,SubjectID,CreateDate,Content,Thumbnail,ImageFile")] Education education)
        {
            if (ModelState.IsValid)
            {
                if (imgProvider.Validate(education.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(education.ImageFile);

                    ViewBag.ActivityID = new SelectList(db.Activities, "ActivityID", "ActivityName", education.ActivityID);
                    ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "ActivityName", education.SubjectID);
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(education.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(education.ImageFile.FileName);

                education.Thumbnail = "~/public/uploadedFiles/educationPictures/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/educationPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                education.ImageFile.SaveAs(fileName);

                education.CreateDate = DateTime.Now;

                db.Educations.Add(education);
                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");

            }

            ViewBag.ActivityID = new SelectList(db.Activities, "ActivityID", "ActivityName", education.ActivityID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", education.SubjectID);
            return View(education);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            Session["OLD_EDUCATION_IMAGE"] = education.Thumbnail;
            if (education == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityID = new SelectList(db.Activities, "ActivityID", "ActivityName", education.ActivityID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", education.SubjectID);
            return View(education);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "EducationID,LessonName,ActivityID,SubjectID,CreateDate,Content,Thumbnail,ImageFile")] Education education, String educationOldImage)
        {
            if (ModelState.IsValid)
            {
                if (education.ImageFile == null)
                {
                    education.Thumbnail = educationOldImage;
                }
                else
                {
                    if (imgProvider.Validate(education.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(education.ImageFile);
                        ViewBag.ActivityID = new SelectList(db.Activities, "ActivityID", "ActivityName", education.ActivityID);
                        ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", education.SubjectID);
                        return View(education);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(education.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(education.ImageFile.FileName);

                    education.Thumbnail = "~/public/uploadedFiles/educationPictures/" + fileName;

                    string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/educationPictures/");

                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    if (educationOldImage != null)
                    {
                        System.IO.File.Delete(Server.MapPath(educationOldImage));
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    education.ImageFile.SaveAs(fileName);
                }

                db.Entry(education).State = EntityState.Modified;
                db.Entry(education).Property(x => x.CreateDate).IsModified = false;

                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OLD_EDUCATION_IMAGE");
                    TempData["Notice_Save_Success"] = true;
                }
                    
                return RedirectToAction("Index");
            }

            ViewBag.ActivityID = new SelectList(db.Activities, "ActivityID", "ActivityName", education.ActivityID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectName", education.SubjectID);
            return View(education);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
        try
        {
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
            if (db.SaveChanges() > 0)
            {
                if (education.Thumbnail != null)
                {
                    System.IO.File.Delete(Server.MapPath(education.Thumbnail));
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
