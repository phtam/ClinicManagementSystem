using ClinicManagementSystem.EF;
using ClinicManagementSystem.Provider;
using Scrypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class RegisterController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "Username,Password,FullName,DateOfBirth,Gender,Phone,Email,Address,Avatar,Status,ImageFile")] Customer customer, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (db.Customers.Find(customer.Username) != null)
                {
                    TempData["ErrorMess"] = "Username has already existed.";
                    return View();
                }

                if (customer.Password != ConfirmPassword)
                {
                    TempData["ErrorMess"] = "Confirm password does not match.";
                    return View();
                }

                if (customer.Username.Length < 6 || customer.Username.Length > 50)
                {
                    TempData["ErrorMess"] = "Username must be between 6 to 50 characters.";
                    return View();
                }

                bool isExist = db.Customers.ToList().Exists(model => model.Email.Equals(customer.Email, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    TempData["ErrorMess"] = "Email has already existed.";
                    return View();
                }

                bool isAgeValid = true;
                if ((DateTime.Now.Year - customer.DateOfBirth.Value.Year) == 16)
                {
                    if ((DateTime.Now.Month - customer.DateOfBirth.Value.Month) == 0)
                    {
                        if ((DateTime.Now.Day - customer.DateOfBirth.Value.Day) > 0)
                        {
                            isAgeValid = false;
                        }
                    }
                    else if ((DateTime.Now.Month - customer.DateOfBirth.Value.Month) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Year - customer.DateOfBirth.Value.Year) < 16)
                {
                    isAgeValid = false;
                }

                if (!isAgeValid)
                {
                    TempData["ErrorMess"] = "Age must greater than 16 years old";
                    return View();
                }

                if (customer.Password.Length < 8 || customer.Password.Length > 50)
                {
                    TempData["ErrorMess"] = "Password must be between 8 to 50 characters";
                    return View();
                }

                if (imgProvider.Validate(customer.ImageFile) != null)
                {
                    TempData["ErrorMess"] = imgProvider.Validate(customer.ImageFile);
                    return View();
                }

                string fileName = Path.GetFileNameWithoutExtension(customer.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(customer.ImageFile.FileName);

                customer.Avatar = "~/public/uploadedFiles/customerPictures/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/customerPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                customer.ImageFile.SaveAs(fileName);

                ScryptEncoder encoder = new ScryptEncoder();
                customer.Password = encoder.Encode(customer.Password);
                customer.Status = true;

                db.Customers.Add(customer);

                if (db.SaveChanges() > 0)
                    TempData["Notice_Register_Success"] = "Congratulations! Register successfully.";
                return View();
            }

            return View(customer);
        }
    }
}