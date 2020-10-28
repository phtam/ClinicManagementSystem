using ClinicManagementSystem.DAO;
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

        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.Medicine = medicineDAO.SortByType(id);
                ViewBag.Child = medicineTypeDAO.Get(id).TypeName;
            }
            else
            {
                ViewBag.Medicine = medicineDAO.GetAll();
            }
            
            var medicineList = medicineDAO.GetAll();
            ViewBag.MedicineType = medicineList;
            ViewBag.Header = "Medicine";

            return View();
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