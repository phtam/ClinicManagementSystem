using ClinicManagementSystem.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicManagementSystem.Controllers
{
    public class SearchController : Controller
    {
        private ClinicSystemData db = new ClinicSystemData();
        
        public ActionResult Index(String keyword, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var model = db.ScientificApparatus.Where(m => m.ScientificApparatusName.Contains(keyword) || m.ScientificApparatusType.TypeName.Contains(keyword) || m.Supplier.CompanyName.Contains(keyword)).ToList().ToPagedList(pageNumber, pageSize);

            return View(model);
        }
    }
}