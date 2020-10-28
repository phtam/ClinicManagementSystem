using ClinicManagementSystem.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class ScientificApparatusController : Controller
    {
        private ScientificApparatusTypeDAO scientificApparatusTypeDAO = new ScientificApparatusTypeDAO();
        private ScientificApparatusDAO scientificApparatusDAO = new ScientificApparatusDAO();

        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.ScientificApparatus = scientificApparatusDAO.SortByType(id);
                ViewBag.Child = scientificApparatusTypeDAO.Get(id).TypeName;
            }
            else
            {
                ViewBag.ScientificApparatus = scientificApparatusDAO.GetAll();
            }
            var scientificApparatusTypeList = scientificApparatusTypeDAO.GetAll();
            ViewBag.ScientificApparatusType = scientificApparatusTypeList;
            ViewBag.Header = "Scientific Apparatus";
            
            return View();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Child = scientificApparatusDAO.Get(id).ScientificApparatusName;
            ViewBag.Header = "Scientific Apparatus";
            ViewBag.ApparatusImages = scientificApparatusDAO.GetImages(id);
            ViewBag.ApparatusFeedbacks = scientificApparatusDAO.GetFeedbacks(id);
            return View(scientificApparatusDAO.Get(id));
        }
    }
}