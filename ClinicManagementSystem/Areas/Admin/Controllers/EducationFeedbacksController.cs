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
    [Authorize]
    public class EducationFeedbacksController : BaseController
    {
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index()
        {
            var educationFeedbacks = db.EducationFeedbacks.Include(e => e.Customer).Include(e => e.Education);
            return View(educationFeedbacks.ToList());
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                EducationFeedback educationFeedback = db.EducationFeedbacks.Find(id);
                db.EducationFeedbacks.Remove(educationFeedback);
                if (db.SaveChanges() > 0)
                {
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
