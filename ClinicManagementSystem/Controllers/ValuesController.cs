using ClinicManagementSystem.EF;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class ValuesController : Controller
    {
        ClinicSystemData db = new ClinicSystemData();
        
        public ActionResult Index()
        {
            return View();
        }

        public String CusUsernameValidation(String username)
        {
            if (username.Trim().Length < 6 || username.Trim().Length > 50)
            {
                return "Username must be from 6 to 50 characters";
            }
            else
            {
                if (db.Customers.Find(username) != null)
                    return "Username already existed";
                else
                    return null;
            }
        }

        public String EmpUsernameValidation(String username)
        {
            if (username.Trim().Length < 6 || username.Trim().Length > 50)
            {
                return "Username must be from 6 to 50 characters.";
            }
            else
            {
                if (db.Employees.Find(username) != null)
                    return "Username already existed.";
                else
                    return null;
            }
        }

        public String CheckCusEmail(String emailAddress)
        {
            bool isEmail = Regex.IsMatch(emailAddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                bool isExist = db.Customers.ToList().Exists(model => model.Email.Equals(emailAddress, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    return "Email already existed.";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return "Email is invalid.";
            }
        }

        public String CheckEmpEmail(String emailAddress)
        {
            bool isEmail = Regex.IsMatch(emailAddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                bool isExist = db.Employees.ToList().Exists(model => model.Email.Equals(emailAddress, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    return "Email already existed.";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return "Email is invalid.";
            }
        }




    }
}