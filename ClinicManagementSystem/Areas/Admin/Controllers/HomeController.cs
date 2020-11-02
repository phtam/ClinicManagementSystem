using ClinicManagementSystem.Areas.Admin.Models;
using ClinicManagementSystem.DAO;
using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private ClinicSystemData db = new ClinicSystemData();
        private ScientificApparatusDAO apparatusDAO = new ScientificApparatusDAO();
        private MedicineDAO medicineDAO = new MedicineDAO();

        public ActionResult Index()
        {
            long apparatusInStock = 0;
            long medicineInStock = 0;
            long orders = 0;
            long total = 0;
            long apparatus = 0;
            long medicine = 0;

            foreach (var item in db.ScientificApparatus.ToList())
            {
                apparatusInStock += item.UnitInStock.GetValueOrDefault(0);
                orders += item.UnitOnOrder.GetValueOrDefault(0);
            }
            foreach (var item in db.Medicines.ToList())
            {
                medicineInStock += item.UnitInStock.GetValueOrDefault(0);
                orders += item.UnitOnOrder.GetValueOrDefault(0);
            }
            var _baseInfo = new long[] { apparatusInStock, medicineInStock, (db.EducationFeedbacks.Count() + db.MedicineFeedbacks.Count() + db.ScientificApparatusFeedbacks.Count()), db.Customers.Count() };

            ViewBag.BaseInfo = _baseInfo;

            foreach (var item in db.MedicineOrderDetails.ToList())
            {
                total += (item.Quantity.GetValueOrDefault(0) * item.Medicine.UnitPrice.GetValueOrDefault(0));
                medicine += (item.Quantity.GetValueOrDefault(0) * item.Medicine.UnitPrice.GetValueOrDefault(0));
            }
            foreach (var item in db.ScientificApparatusOrderDetails.ToList())
            {
                total += (item.Quantity.GetValueOrDefault(0) * item.ScientificApparatu.UnitPrice.GetValueOrDefault(0));
                apparatus += (item.Quantity.GetValueOrDefault(0) * item.ScientificApparatu.UnitPrice.GetValueOrDefault(0));
            }
            
            var _total = new long[] { orders, total, apparatus, medicine };
            ViewBag.Total = _total;

            List<int> dataCategoryChartInStock = new List<int>();
            List<int> dataCategoryChartOnOrder = new List<int>();

            foreach (var item in db.ScientificApparatusTypes.ToList())
            {
                dataCategoryChartInStock.Add(apparatusDAO.CountProductByCategoryInStock(item.TypeID));
                dataCategoryChartOnOrder.Add(apparatusDAO.CountProductByCategoryOnOrder(item.TypeID));
            }

            var labelChart = db.ScientificApparatusTypes.Select(x => x.TypeName);
            ViewBag.LabelChartCategory = labelChart;
            ViewBag.DataCategoryChartInStock = dataCategoryChartInStock;
            ViewBag.DataCategoryChartOnOrder = dataCategoryChartOnOrder;

            List<int> dataMedicineChartInStock = new List<int>();
            List<int> dataMedicineChartOnOrder = new List<int>();

            foreach (var item in db.MedicineTypes.ToList())
            {
                dataMedicineChartInStock.Add(medicineDAO.CountProductByCategoryInStock(item.TypeID));
                dataMedicineChartOnOrder.Add(medicineDAO.CountProductByCategoryOnOrder(item.TypeID));
            }

            var labelMedicineChart = db.MedicineTypes.Select(x => x.TypeName);
            ViewBag.MedicineLabel = labelMedicineChart;
            ViewBag.MedicineChartInStock = dataMedicineChartInStock;
            ViewBag.MedicineChartOnOrder = dataMedicineChartOnOrder;


            ViewBag.NewFeedbacks = db.EducationFeedbacks.OrderByDescending(i => i.CreatedDate).Take(7).ToList();
            var blogs = db.Educations.OrderByDescending(i => i.CreateDate).Take(4).ToList();

            
            ViewBag.RecentlyAddedBlogs = blogs;

            var dataSystem = new int[] { db.Suppliers.ToList().Count(), db.ScientificApparatus.ToList().Count(), db.Medicines.ToList().Count(), db.Educations.ToList().Count() };
            ViewBag.DataSystem = dataSystem;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Aside()
        {
            var user = (EmployeeAuthentication)Session[Common.CommonConstants.EMPLOYEE_SESSION];
            var model = db.Employees.Find(user.Username);
            return PartialView(model);
        }
    }
}