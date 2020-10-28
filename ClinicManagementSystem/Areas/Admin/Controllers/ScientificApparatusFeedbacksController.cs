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
    public class ScientificApparatusFeedbacksController : BaseController
    {
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index()
        {
            var scientificApparatusFeedbacks = db.ScientificApparatusFeedbacks.Include(s => s.Customer).Include(s => s.ScientificApparatu);
            return View(scientificApparatusFeedbacks.ToList());
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ScientificApparatusFeedback scientificApparatusFeedback = db.ScientificApparatusFeedbacks.Find(id);
                db.ScientificApparatusFeedbacks.Remove(scientificApparatusFeedback);
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
