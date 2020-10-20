using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.EF;
using ClinicManagementSystem.Provider;
using Scrypt;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    public class CustomersController : Controller
    {
        private ClinicData db = new ClinicData();
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FullName,DateOfBirth,Gender,Phone,Email,Address,Avatar,Status,ImageFile")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (db.Customers.Find(customer.Username) != null)
                {
                    ViewBag.Error = "Username already exists";
                    return View("Create");
                }

                if (customer.Username.Length < 6 || customer.Username.Length > 50)
                {
                    ViewBag.Error = "Username must be between 6 to 50 characters.";
                    return View("Create");
                }

                bool isExist = db.Customers.ToList().Exists(model => model.Email.Equals(customer.Email, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    ViewBag.Error = "Email already exists";
                    return View("Create");
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
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Create");
                }

                if (customer.Password.Length < 8 || customer.Password.Length > 50)
                {
                    ViewBag.Error = "Password must be between 8 to 50 characters";
                    return View("Create");
                }

                if (imgProvider.Validate(customer.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(customer.ImageFile);
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(customer.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(customer.ImageFile.FileName);
                
                customer.Avatar = "~/public/uploadedFiles/customerPictures/" + fileName;
                // ~/public/uploadedFiles/customerPictures/anh.jpg

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/customerPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                customer.ImageFile.SaveAs(fileName);

                ScryptEncoder encoder = new ScryptEncoder();
                customer.Password = encoder.Encode(customer.Password);

                db.Customers.Add(customer);

                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            Session["OLD_CUSTOMER_IMAGE"] = customer.Avatar;
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,FullName,DateOfBirth,Gender,Phone,Email,Address,Avatar,Status,ImageFile")] Customer customer, String userOldImage)
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
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Edit");
                }

                if (customer.ImageFile == null)
                {
                    customer.Avatar = userOldImage;
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

                    if (userOldImage != null)
                    {
                        System.IO.File.Delete(Server.MapPath(userOldImage));
                    }

                    customer.ImageFile.SaveAs(fileName);

                }
                db.Entry(customer).State = EntityState.Modified;
                db.Entry(customer).Property(x => x.Password).IsModified = false;

                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OLD_CUSTOMER_IMAGE");
                    TempData["Notice_Save_Success"] = true;
                }
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Customer customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
                db.SaveChanges();
                if (customer.Avatar != null)
                {
                    System.IO.File.Delete(Server.MapPath(customer.Avatar));
                }
                TempData["Notice_Delete_Success"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
