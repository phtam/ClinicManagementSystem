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

        public ActionResult Index()
        {
            var medicineFeedbacks = db.MedicineFeedbacks.Include(m => m.Customer).Include(m => m.Medicine);
            return View(medicineFeedbacks.ToList());
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                MedicineFeedback medicineFeedback = db.MedicineFeedbacks.Find(id);
                db.MedicineFeedbacks.Remove(medicineFeedback);
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
