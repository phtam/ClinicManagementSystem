using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.DAO;

namespace ClinicManagementSystem.Controllers
{
    public class EducationController : Controller
    {
        private EducationDAO educationDAO = new EducationDAO();
        private SubjectDAO subjectDAO = new SubjectDAO();
        private ActivityDAO activityDAO = new ActivityDAO();

        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.Child = activityDAO.Get(id).ActivityName;
                ViewBag.Education = educationDAO.SortByActivity(id);
            }
            else
            {
                ViewBag.Education = educationDAO.GetAll();
            }
            ViewBag.Header = "Education";
            return View();
        }

        public ActionResult SearchBySubject(int? id)
        {
            if (id != null)
            {
                ViewBag.Child = subjectDAO.Get(id).SubjectName;
                ViewBag.Education = educationDAO.SortBySubject(id);
            }
            else
            {
                ViewBag.Education = educationDAO.GetAll();
            }
            ViewBag.Header = "Education";
            return View();
        }

        public ActionResult Detail(int? id)
        {
            ViewBag.EducationFeedbacks = educationDAO.GetFeedbacks(id);
            ViewBag.Header = "Education";
            ViewBag.Child = educationDAO.Get(id).LessonName;
            return View(educationDAO.Get(id));
        }

        [ChildActionOnly]
        public ActionResult _RightSlibar()
        {
            ViewBag.Subjects = subjectDAO.GetAll();
            ViewBag.Activities = activityDAO.GetAll();
            ViewBag.RecentPost = educationDAO.GetAll().Take(4).ToList();
            return PartialView();
        }
        
    }
}