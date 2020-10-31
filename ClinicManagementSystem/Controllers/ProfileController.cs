using ClinicManagementSystem.EF;
using ClinicManagementSystem.Models;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class ProfileController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        public ActionResult Index()
        {
            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Customer customer = db.Customers.Find(user.Username);
            Session["OLD_IMAGE"] = customer.Avatar;
            return View(customer);
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "Username,Password,FullName,DateOfBirth,Gender,Phone,Email,Address,Avatar,Status,ImageFile")] Customer customer, String OldImage)
        {
            if (ModelState.IsValid)
            {
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
                    return RedirectToAction("Index");
                }

                if (customer.ImageFile == null)
                {
                    customer.Avatar = OldImage;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(customer.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(customer.ImageFile.FileName);

                    customer.Avatar = "~/public/uploadedFiles/customerPictures/" + fileName;

                    string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/customerPictures/");

                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    if (OldImage != null)
                    {
                        System.IO.File.Delete(Server.MapPath(OldImage));
                    }

                    customer.ImageFile.SaveAs(fileName);

                }
                db.Entry(customer).State = EntityState.Modified;
                db.Entry(customer).Property(x => x.Password).IsModified = false;
                db.Entry(customer).Property(x => x.Status).IsModified = false;

                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OLD_IMAGE");
                    TempData["Notice_Profile_Success"] = "Save successfully!";
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string current, string password, string confirm)
        {
            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            ScryptEncoder encoder = new ScryptEncoder();
            var account = db.Customers.Find(user.Username);
            bool isValidPass = encoder.Compare(current, account.Password);

            if (isValidPass)
            {
                if (password.Length < 8 || password.Length > 50)
                {
                    ViewBag.ErrorPassword = "Password must be between 8 to 50 characters.";
                    return View();
                }

                if (password != confirm)
                {
                    ViewBag.ErrorConfirm = "Confirm password does not match.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorCurrent = "Current password is incorrect.";
                return View();
            }

            account.Password = encoder.Encode(password);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewOrder()
        {
            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var order = db.Orders.Where(o => o.Username == user.Username);
            return View(order.ToList());
        }

        public ActionResult OrderDetail(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Index", "Home");
            }
            var user = (CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (user.Username == null)
            {
                RedirectToAction("Index", "Login");
            }
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id && o.Username == user.Username);
            if (order == null)
            {
                RedirectToAction("Index", "Home");
            }

            ViewBag.MedicineOrder = db.MedicineOrderDetails.Where(m => m.OrderID == id).ToList();
            ViewBag.ApparatusOrder = db.ScientificApparatusOrderDetails.Where(a => a.OrderID == id).ToList();

            return View(order);

        }
    }
}