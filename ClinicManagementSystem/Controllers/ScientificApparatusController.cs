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
    public class ScientificApparatusController : Controller
    {
        private ScientificApparatusTypeDAO scientificApparatusTypeDAO = new ScientificApparatusTypeDAO();
        private ScientificApparatusDAO scientificApparatusDAO = new ScientificApparatusDAO();
        private ClinicSystemData db = new ClinicSystemData();

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

        public ActionResult Detail(int? id, int? page)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            ViewBag.Child = scientificApparatusDAO.Get(id).ScientificApparatusName;
            ViewBag.Header = "Scientific Apparatus";
            ViewBag.ApparatusImages = scientificApparatusDAO.GetImages(id);
            ViewBag.Detail = scientificApparatusDAO.Get(id);
            var model = scientificApparatusDAO.GetFeedbacks(id).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        [HttpPost]
        public ActionResult Feedback(int apparatusId, String message)
        {
            if (Session[Common.CommonConstants.CUSTOMER_SESSION] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            var feedback = new ScientificApparatusFeedback();
            feedback.Username = user.Username;
            feedback.ScientificApparatusID = apparatusId;
            feedback.CreatedDate = DateTime.Now;
            feedback.Content = message;
            db.ScientificApparatusFeedbacks.Add(feedback);
            db.SaveChanges();
            return RedirectToAction("Detail", "ScientificApparatus", new { id = apparatusId });
        }
    }
}