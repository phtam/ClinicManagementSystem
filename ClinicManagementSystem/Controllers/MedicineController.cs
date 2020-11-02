using ClinicManagementSystem.DAO;
using ClinicManagementSystem.EF;
using ClinicManagementSystem.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class MedicineController : Controller
    {
        private MedicineTypeDAO medicineTypeDAO = new MedicineTypeDAO();
        private MedicineDAO medicineDAO = new MedicineDAO();
        private ClinicSystemData db = new ClinicSystemData();
        private SupplierDAO supplierDAO = new SupplierDAO();

        public ActionResult Index(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            

            if (id != null)
            {
                var model = medicineDAO.SortByType(id).ToPagedList(pageNumber, pageSize);
                ViewBag.ID = id;
                ViewBag.Child = medicineTypeDAO.Get(id).TypeName;
                return View(model);
            }
            else
            {
                var model = medicineDAO.GetAll().ToPagedList(pageNumber, pageSize);
                return View(model);
            }
        }

        public ActionResult SearchBySupplier(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            if (id != null)
            {
                var model = medicineDAO.SortBySupplier(id).ToPagedList(pageNumber, pageSize);
                ViewBag.ID = id;
                ViewBag.Child = supplierDAO.Get(id).CompanyName;
                return View(model);
            }
            else
            {
                var model = medicineDAO.GetAll().ToPagedList(pageNumber, pageSize);
                return View(model);
            }
        }

        public ActionResult Detail(int? id, int? page)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            ViewBag.Child = medicineDAO.Get(id).MedicineName;
            ViewBag.Header = "Medicine";
            ViewBag.MedicineImages = medicineDAO.GetImages(id);
            ViewBag.Detail = medicineDAO.Get(id);
            var model = medicineDAO.GetFeedbacks(id).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        [HttpPost]
        public ActionResult Feedback(int medicineId, String message)
        {
            if (Session[Common.CommonConstants.CUSTOMER_SESSION] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            var feedback = new MedicineFeedback();
            feedback.Username = user.Username;
            feedback.MedicineID = medicineId;
            feedback.CreatedDate = DateTime.Now;
            feedback.Content = message;
            db.MedicineFeedbacks.Add(feedback);
            db.SaveChanges();
            return RedirectToAction("Detail", "Medicine", new { id = medicineId });
        }
    }
}