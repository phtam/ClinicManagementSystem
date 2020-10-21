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
    public class EmployeesController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        private ImageProvider imgProvider = new ImageProvider();

        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FullName,DateOfBirth,Gender,Phone,Email,Address,Avatar,Position,Status,ImageFile")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (db.Employees.Find(employee.Username) != null)
                {
                    ViewBag.Error = "Username already exists";
                    return View("Create");
                }

                if (employee.Username.Length < 6 || employee.Username.Length > 50)
                {
                    ViewBag.Error = "Username must be between 6 to 50 characters.";
                    return View("Create");
                }

                bool isExist = db.Employees.ToList().Exists(model => model.Email.Equals(employee.Email, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    ViewBag.Error = "Email already exists";
                    return View("Create");
                }

                bool isAgeValid = true;
                if ((DateTime.Now.Year - employee.DateOfBirth.Value.Year) == 16)
                {
                    if ((DateTime.Now.Month - employee.DateOfBirth.Value.Month) == 0)
                    {
                        if ((DateTime.Now.Day - employee.DateOfBirth.Value.Day) > 0)
                        {
                            isAgeValid = false;
                        }
                    }
                    else if ((DateTime.Now.Month - employee.DateOfBirth.Value.Month) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Year - employee.DateOfBirth.Value.Year) < 16)
                {
                    isAgeValid = false;
                }

                if (!isAgeValid)
                {
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Create");
                }

                if (employee.Password.Length < 8 || employee.Password.Length > 50)
                {
                    ViewBag.Error = "Password must be between 8 to 50 characters";
                    return View("Create");
                }

                if (imgProvider.Validate(employee.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(employee.ImageFile);
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(employee.ImageFile.FileName);

                employee.Avatar = "~/public/uploadedFiles/employeePictures/" + fileName;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/employeePictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                employee.ImageFile.SaveAs(fileName);

                ScryptEncoder encoder = new ScryptEncoder();
                employee.Password = encoder.Encode(employee.Password);

                db.Employees.Add(employee);

                if (db.SaveChanges() > 0)
                    TempData["Notice_Create_Success"] = true;

                return RedirectToAction("Index");
            }

            return View(employee);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            Session["OLD_EMPLOYEE_IMAGE"] = employee.Avatar;
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,FullName,DateOfBirth,Gender,Phone,Email,Address,Avatar,Position,Status,ImageFile")] Employee employee, string employeeOldImage)
        {
            if (ModelState.IsValid)
            {
                bool isAgeValid = true;
                if ((DateTime.Now.Year - employee.DateOfBirth.Value.Year) == 16)
                {
                    if ((DateTime.Now.Month - employee.DateOfBirth.Value.Month) == 0)
                    {
                        if ((DateTime.Now.Day - employee.DateOfBirth.Value.Day) > 0)
                        {
                            isAgeValid = false;
                        }
                    }
                    else if ((DateTime.Now.Month - employee.DateOfBirth.Value.Month) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Year - employee.DateOfBirth.Value.Year) < 16)
                {
                    isAgeValid = false;
                }

                if (!isAgeValid)
                {
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Edit");
                }

                if (employee.ImageFile == null)
                {
                    employee.Avatar = employeeOldImage;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(employee.ImageFile.FileName);

                    employee.Avatar = "~/public/uploadedFiles/employeePictures/" + fileName;

                    string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/employeePictures/");

                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);
                    if (employeeOldImage != null)
                    {
                        System.IO.File.Delete(Server.MapPath(employeeOldImage));
                    }

                    employee.ImageFile.SaveAs(fileName);

                }
                db.Entry(employee).State = EntityState.Modified;
                db.Entry(employee).Property(x => x.Password).IsModified = false;

                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OLD_CUSTOMER_IMAGE");
                    TempData["Notice_Save_Success"] = true;
                }
                return RedirectToAction("Index");
            }
            return View(employee);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Employee employee = db.Employees.Find(id);
                db.Employees.Remove(employee);
                db.SaveChanges();
                //if (employee.Avatar != null)
                //{
                //    System.IO.File.Delete(Server.MapPath(employee.Avatar));
                //}
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
