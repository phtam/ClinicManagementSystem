using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.Areas.Admin.Models;
using ClinicManagementSystem.EF;
using Scrypt;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index()
        {
            var user = (EmployeeAuthentication)Session[Common.CommonConstants.EMPLOYEE_SESSION];
            var model = db.Employees.Find(user.Username);
            Session["OLD_PROFILE_IMAGE"] = model.Avatar;

            return View(model);
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
                db.Entry(employee).Property(x => x.Position).IsModified = false;
                db.Entry(employee).Property(x => x.Status).IsModified = false;

                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OLD_CUSTOMER_IMAGE");
                    TempData["Notice_Save_Success"] = true;
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangePassword(string username, string currentPass, string newPass, string confirmPass)
        {
            if (username == null || currentPass == null || newPass == null || confirmPass == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScryptEncoder encoder = new ScryptEncoder();
            var user = db.Employees.Find(username);
            bool isValidPass = encoder.Compare(currentPass, user.Password);
            if (!isValidPass)
            {
                TempData["Current_Pass_Fail"] = true;
                return RedirectToAction("Index");
            }
            if (newPass != confirmPass)
            {
                TempData["Password_Not_Match"] = true;
                return RedirectToAction("Index");
            }

            if (newPass.Length < 8 || newPass.Length > 50 )
            {
                TempData["Password_Incorrect_Format"] = true;
                return RedirectToAction("Index");
            }

            user.Password = encoder.Encode(newPass);
            if (db.SaveChanges() > 0)
            {
                TempData["Notice_Save_Success"] = true;
            }
            return RedirectToAction("Index");

        }
    }
}