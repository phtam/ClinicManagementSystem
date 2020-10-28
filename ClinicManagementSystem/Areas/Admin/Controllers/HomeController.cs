using ClinicManagementSystem.Areas.Admin.Models;
using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Aside()
        {
            var user = (EmployeeAuthentication)Session[Common.CommonConstants.EMPLOYEE_SESSION];
            var model = db.Employees.Find(user.Username);
            return PartialView(model);
        }
    }
}