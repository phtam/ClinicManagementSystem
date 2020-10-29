using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.EF;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private ClinicSystemData db = new ClinicSystemData();

        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.Payment);
            return View(orders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", order.Username);
            ViewBag.PaymentID = new SelectList(db.Payments, "PaymentID", "PaymentName", order.PaymentID);
            ViewBag.MedicineOrder = db.MedicineOrderDetails.Where(m => m.OrderID == id).ToList();
            ViewBag.ApparatusOrder = db.ScientificApparatusOrderDetails.Where(a => a.OrderID == id).ToList();

            return View(order);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Username = new SelectList(db.Customers, "Username", "Password", order.Username);
            ViewBag.PaymentID = new SelectList(db.Payments, "PaymentID", "PaymentName", order.PaymentID);

            ViewBag.MedicineOrder = db.MedicineOrderDetails.Where(m => m.OrderID == id).ToList();
            ViewBag.ApparatusOrder = db.ScientificApparatusOrderDetails.Where(a => a.OrderID == id).ToList();

            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(int OrderID, int Status)
        {
            var order = db.Orders.Find(OrderID);
            order.Status = Status;
            if (Status == 2)
            {
                order.DeliveredDate = DateTime.Now;
            }
            if (db.SaveChanges() > 0)
                TempData["Notice_Save_Success"] = true;
            return RedirectToAction("Edit", "Orders", new { id = OrderID });
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
