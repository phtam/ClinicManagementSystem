using ClinicManagementSystem.DAO;
using PagedList;
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

        public ActionResult Index(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var scientificApparatusTypeList = scientificApparatusTypeDAO.GetAll();
            ViewBag.ScientificApparatusType = scientificApparatusTypeList;
            ViewBag.Header = "Scientific Apparatus";

            if (id != null)
            {
                var model = scientificApparatusDAO.SortByType(id).ToPagedList(pageNumber, pageSize);
                ViewBag.Child = scientificApparatusTypeDAO.Get(id).TypeName;
                return View(model);
            }
            else
            {
                var model = scientificApparatusDAO.GetAll().ToPagedList(pageNumber, pageSize);
                return View(model);
            }

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