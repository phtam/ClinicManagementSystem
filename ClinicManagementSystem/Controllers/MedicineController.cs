using ClinicManagementSystem.DAO;
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

        public ActionResult Index(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var medicineList = medicineDAO.GetAll();
            ViewBag.MedicineType = medicineList;
            ViewBag.Header = "Medicine";

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

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Child = medicineDAO.Get(id).MedicineName;
            ViewBag.Header = "Medicine";
            ViewBag.MedicineImages = medicineDAO.GetImages(id);
            ViewBag.MedicineFeedbacks = medicineDAO.GetFeedbacks(id);
            return View(medicineDAO.Get(id));
        }
    }
}