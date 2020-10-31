using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.DAO;
using PagedList;

namespace ClinicManagementSystem.Controllers
{
    public class EducationController : Controller
    {
        private EducationDAO educationDAO = new EducationDAO();
        private SubjectDAO subjectDAO = new SubjectDAO();
        private ActivityDAO activityDAO = new ActivityDAO();

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