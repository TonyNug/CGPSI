using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.Dashboard
{
    public class DashboardAbsenceController : Controller
    {
        // GET: DashboardAbsence
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult DataAbsence()
        {
            return PartialView();
        }

        public ActionResult DepartmentReport()
        {
            return PartialView();
        }
    }
}