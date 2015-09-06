using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class LeaveCategoryController : Controller
    {
        //
        // GET: /LeaveCategory/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Data Employees for Reference View
        /// </summary>
        public ActionResult GetRefLeaveCategory()
        {
            return Json(new CGPSI_AbsenceDBEntities().LeaveCategories.ToList());
        }
	}
}