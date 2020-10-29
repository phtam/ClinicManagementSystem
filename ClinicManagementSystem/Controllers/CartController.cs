using ClinicManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicManagementSystem.Models;
using ClinicManagementSystem.DAO;
using System.Web.Script.Serialization;
using System.Configuration;
using ClinicManagementSystem.EF;

namespace ClinicManagementSystem.Controllers
{
    public class CartController : Controller
    {
        private MedicineDAO medicineDAO = new MedicineDAO();
        
        public ActionResult Index()
        {
            var medicineCart = Session[CommonConstants.CartMedicineSession];
            var appararusCart = Session[CommonConstants.CartApparatusSession];
            var medicineList = new List<MedicineItem>();
            var apparatusList = new List<ScientificApparatusItem>();
            long price = 0;
            long old_price = 0;

            if (medicineCart != null)
            {
                medicineList = (List<MedicineItem>)medicineCart;
                foreach (var item in medicineList)
                {
                    price += (long)(item.Medicine.UnitPrice * item.Quantity);
                    old_price += (long)(item.Medicine.OldUnitPrice * item.Quantity);
                }
            }
                
            if (appararusCart != null)
            {
                apparatusList = (List<ScientificApparatusItem>)appararusCart;
                foreach (var item in apparatusList)
                {
                    price += (long)(item.ScientificApparatus.UnitPrice * item.Quantity);
                    old_price += (long)(item.ScientificApparatus.OldUnitPrice * item.Quantity);
                }
            }
                
            ViewBag.Price = price;
            ViewBag.OldPrice = old_price;
            ViewBag.Header = "Cart";
            ViewBag.Apparatus = apparatusList;
            ViewBag.Medicine = medicineList;

            return View();
        }

        public ActionResult AddMedicineItem(int productID, int quantity)
        {
            var product = new MedicineDAO().Get(productID);
            var cart = Session[CommonConstants.CartMedicineSession];
            if (cart != null)
            {
                var list = (List<MedicineItem>)cart;
                if (list.Exists(x => x.Medicine.MedicineID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.Medicine.MedicineID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new MedicineItem();
                    item.Medicine = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CommonConstants.CartMedicineSession] = list;
            }
            else
            {
                var item = new MedicineItem();
                item.Medicine = product;
                item.Quantity = quantity;
                var list = new List<MedicineItem>();
                list.Add(item);
                Session[CommonConstants.CartMedicineSession] = list;
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddApparatusItem(int productID, int quantity)
        {
            var product = new ScientificApparatusDAO().Get(productID);
            var cart = Session[CommonConstants.CartApparatusSession];
            if (cart != null)
            {
                var list = (List<ScientificApparatusItem>)cart;
                if (list.Exists(x => x.ScientificApparatus.ScientificApparatusID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.ScientificApparatus.ScientificApparatusID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new ScientificApparatusItem();
                    item.ScientificApparatus = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CommonConstants.CartApparatusSession] = list;
            }
            else
            {
                var item = new ScientificApparatusItem();
                item.ScientificApparatus = product;
                item.Quantity = quantity;
                var list = new List<ScientificApparatusItem>();
                list.Add(item);
                Session[CommonConstants.CartApparatusSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteMedicine(long id)
        {
            var sessionCart = (List<MedicineItem>)Session[CommonConstants.CartMedicineSession];
            sessionCart.RemoveAll(x => x.Medicine.MedicineID == id);
            Session[CommonConstants.CartMedicineSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteApparatus(long id)
        {
            var sessionCart = (List<ScientificApparatusItem>)Session[CommonConstants.CartApparatusSession];
            sessionCart.RemoveAll(x => x.ScientificApparatus.ScientificApparatusID == id);
            Session[CommonConstants.CartApparatusSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.CartApparatusSession] = null;
            Session[CommonConstants.CartMedicineSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string medicineModel, string apparatusModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<MedicineItem>>(medicineModel);
            var sessionCart = (List<MedicineItem>)Session[CommonConstants.CartMedicineSession];
            if (sessionCart != null)
            {
                foreach (var item in sessionCart)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.Medicine.MedicineID == item.Medicine.MedicineID);
                    if (jsonItem != null)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                    if (item.Quantity == 0)
                    {
                        sessionCart.Remove(item);
                    }
                }
                Session[CommonConstants.CartMedicineSession] = sessionCart;
            }
            

            var jsonCart2 = new JavaScriptSerializer().Deserialize<List<ScientificApparatusItem>>(apparatusModel);
            var sessionCart2 = (List<ScientificApparatusItem>)Session[CommonConstants.CartApparatusSession];

            if (sessionCart2 != null)
            {
                foreach (var item in sessionCart2)
                {
                    var jsonItem2 = jsonCart2.SingleOrDefault(x => x.ScientificApparatus.ScientificApparatusID == item.ScientificApparatus.ScientificApparatusID);
                    if (jsonItem2 != null)
                    {
                        item.Quantity = jsonItem2.Quantity;
                    }
                    if (item.Quantity == 0)
                    {
                        sessionCart2.Remove(item);
                    }
                }
                Session[CommonConstants.CartApparatusSession] = sessionCart2;
            }
            
            return Json(new
            {
                status = true
            });
        }

        public ActionResult Checkout()
        {
            if (Session[Common.CommonConstants.CUSTOMER_SESSION] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var medicineCart = Session[CommonConstants.CartMedicineSession];
            var appararusCart = Session[CommonConstants.CartApparatusSession];
            var medicineList = new List<MedicineItem>();
            var apparatusList = new List<ScientificApparatusItem>();
            long price = 0;
            if (medicineCart != null)
            {
                medicineList = (List<MedicineItem>)medicineCart;
                foreach (var item in medicineList)
                {
                    price += (long)(item.Medicine.UnitPrice * item.Quantity);
                }
            }

            if (appararusCart != null)
            {
                apparatusList = (List<ScientificApparatusItem>)appararusCart;
                foreach (var item in apparatusList)
                {
                    price += (long)(item.ScientificApparatus.UnitPrice * item.Quantity);
                }
            }

            ViewBag.Price = price;
            ViewBag.Apparatus = apparatusList;
            ViewBag.Medicine = medicineList;
            ViewBag.Payments = new PaymentDAO().GetAll();

            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string address, int payment, string note)
        {
            if (address.Trim().Length == 0)
            {
                TempData["ErrorMess"] = "Please enter your address.";
                return RedirectToAction("Checkout");
            }
            if (payment.ToString().Length == 0)
            {
                TempData["ErrorMess"] = "Please choose your form of payment!";
                return RedirectToAction("Checkout");
            }

            //is in stock less than the quantity you ordered. Please reduce the quantity!

            Order order = new Order();
            var user = (ClinicManagementSystem.Models.CustomerAuthentication)Session[Common.CommonConstants.CUSTOMER_SESSION];

            order.Username = user.Username;
            order.DeliveredAddress = address;
            order.Note = note;
            order.PaymentID = payment;
            order.CreateDate = DateTime.Now;
            order.Status = 0;

            var orderDAO = new OrderDAO();
            var id = orderDAO.Create(order);
            var medicineCart = (List<MedicineItem>)Session[CommonConstants.CartMedicineSession];
            var apparatusCart = (List<ScientificApparatusItem>)Session[CommonConstants.CartApparatusSession];

            if (medicineCart != null)
            {
                foreach (var item in medicineCart)
                {
                    MedicineOrderDetail medicineOrderDetail = new MedicineOrderDetail();
                    medicineOrderDetail.OrderID = id;
                    medicineOrderDetail.MedicineID = item.Medicine.MedicineID;
                    medicineOrderDetail.Quantity = item.Quantity;
                    orderDAO.AddMedicine(medicineOrderDetail);
                    orderDAO.UpdateMedicineUnitOnOrder(item.Medicine.MedicineID, item.Quantity);
                }
            }

            if (apparatusCart != null)
            {
                foreach (var item in apparatusCart)
                {
                    ScientificApparatusOrderDetail scientificApparatusOrderDetail = new ScientificApparatusOrderDetail();
                    scientificApparatusOrderDetail.OrderID = id;
                    scientificApparatusOrderDetail.ScientificApparatusID = item.ScientificApparatus.ScientificApparatusID;
                    scientificApparatusOrderDetail.Quantity = item.Quantity;
                    orderDAO.AddApparatus(scientificApparatusOrderDetail);
                    orderDAO.UpdateApparatusUnitOnOrder(item.ScientificApparatus.ScientificApparatusID, item.Quantity);
                }
            }

            Session[CommonConstants.CartMedicineSession] = null;
            Session[CommonConstants.CartApparatusSession] = null;

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}