using ClinicManagementSystem.EF;
using ClinicManagementSystem.Models;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            var user = db.Customers.SingleOrDefault(model => model.Username == username);
            if (user == null)
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }

            bool isValidPass = encoder.Compare(password, user.Password);

            if (isValidPass)
            {
                if (user.Status == false)
                {
                    ViewBag.ErrorLogin = "Your account has been locked";
                    return View();
                }

                var userSession = new CustomerAuthentication();
                userSession.Username = user.Username;
                userSession.FullName = user.FullName;
                userSession.Gender = user.Gender;
                userSession.DateOfBirth = user.DateOfBirth;
                userSession.Phone = user.Phone;
                userSession.Email = user.Email;
                userSession.Address = user.Address;
                userSession.Status = user.Status;
                userSession.Avatar = user.Avatar;
                Session.Add(Common.CommonConstants.CUSTOMER_SESSION, userSession);
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
            Session[Common.CommonConstants.CUSTOMER_SESSION] = null;
            return RedirectToAction("Index", "Home");
        }
    }

    
}