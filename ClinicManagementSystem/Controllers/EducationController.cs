using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.DAO;
using ClinicManagementSystem.EF;
using ClinicManagementSystem.Models;
using PagedList;

namespace ClinicManagementSystem.Controllers
{
    public class EducationController : Controller
    {
        private EducationDAO educationDAO = new EducationDAO();
        private SubjectDAO subjectDAO = new SubjectDAO();
        private ActivityDAO activityDAO = new ActivityDAO();
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Header = "Education";

            if (id != null)
            {
                ViewBag.Child = activityDAO.Get(id).ActivityName;
                ViewBag.ID = id;
                var model = educationDAO.SortByActivity(id).ToPagedList(pageNumber, pageSize);
                return View(model);
            }
            else
            {
                var model = educationDAO.GetAll().ToPagedList(pageNumber, pageSize);
                return View(model);
            }
        }

        public ActionResult SearchBySubject(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Header = "Education";

            if (id != null)
            {
                ViewBag.Child = subjectDAO.Get(id).SubjectName;
                ViewBag.ID = id;
                var model = educationDAO.SortBySubject(id).ToPagedList(pageNumber, pageSize);
                return View(model);
            }
            else
            {
                var model = educationDAO.GetAll().ToPagedList(pageNumber, pageSize);
                return View(model);
            }
        }

        public ActionResult Detail(int? id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var model = educationDAO.GetFeedbacks(id).ToPagedList(pageNumber, pageSize);
            ViewBag.Header = "Education";
            ViewBag.Child = educationDAO.Get(id).LessonName;
            ViewBag.Detail = educationDAO.Get(id);

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult _RightSlibar()
        {
            ViewBag.Subjects = subjectDAO.GetAll();
            ViewBag.Activities = activityDAO.GetAll();
            ViewBag.RecentPost = educationDAO.GetAll().Take(4).ToList();
            return PartialView();
        }

        [HttpPost]
        public ActionResult Feedback(int educationId, String comment)
        {
            if (Session[Common.CommonConstants.CUSTOMER_SESSION] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            var feedback = new EducationFeedback();
            feedback.Username = user.Username;
            feedback.EducationID = educationId;
            feedback.CreatedDate = DateTime.Now;
            feedback.Content = comment;
            db.EducationFeedbacks.Add(feedback);
            db.SaveChanges();
            return RedirectToAction("Detail", "Education", new { id = educationId });
        }

    }
}