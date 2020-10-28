using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.DAO;

namespace ClinicManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private MedicineTypeDAO medicineTypeDAO = new MedicineTypeDAO();
        private ScientificApparatusTypeDAO scientificApparatusTypeDAO = new ScientificApparatusTypeDAO();
        private ActivityDAO activityDAO = new ActivityDAO();

        public ActionResult Index()
        {
            ViewBag.Slider = true;
            ViewBag.MedicineType = medicineTypeDAO.GetAll();
            var scientificApparatusTypeList = scientificApparatusTypeDAO.GetAll();
            ViewBag.ScientificApparatusType = scientificApparatusTypeList;

            return View();
        }

        [ChildActionOnly]
        public ActionResult _Header()
        {
            ViewBag.MedicineType = medicineTypeDAO.GetAll();
            ViewBag.ScientificApparatusType = scientificApparatusTypeDAO.GetAll();
            ViewBag.Activity = activityDAO.GetAll();

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _Filter()
        {
            ViewBag.TypeList = medicineTypeDAO.GetAll().ToList();
            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Header = "Contact";

            return View();
        }
    }
}