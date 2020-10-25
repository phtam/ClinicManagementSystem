using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClinicManagementSystem.EF;
using ClinicManagementSystem.Areas.Admin.Models;
using Scrypt;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        ClinicSystemData db = new ClinicSystemData();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string pass)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            var user = db.Employees.SingleOrDefault(model => model.Username == username);
            if (user == null)
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }

            bool isValidPass = encoder.Compare(pass, user.Password);

            if (isValidPass)
            {
                if (user.Status == false)
                {
                    ViewBag.ErrorLogin = "Your account has been locked";
                    return View();
                }

                FormsAuthentication.SetAuthCookie(user.Username, false);

                var userSession = new EmployeeAuthentication();
                userSession.Username = user.Username;
                userSession.FullName = user.FullName;
                userSession.Gender = user.Gender;
                userSession.DateOfBirth = user.DateOfBirth;
                userSession.Phone = user.Phone;
                userSession.Email = user.Email;
                userSession.Address = user.Address;
                userSession.Position = user.Position;
                userSession.Status = user.Status;
                userSession.Avatar = user.Avatar;
                Session.Add(Common.CommonConstants.EMPLOYEE_SESSION, userSession);
                TempData["Notice_Login_Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove(Common.CommonConstants.EMPLOYEE_SESSION);
            return Redirect("Index");
        }
    }
}