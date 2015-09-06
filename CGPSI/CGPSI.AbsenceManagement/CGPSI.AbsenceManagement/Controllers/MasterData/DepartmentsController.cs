using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class DepartmentsController : Controller
    {
        // GET: Departments
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Get(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            var temp = new CGPSI_AbsenceDBEntities().SP_DEPARTMENTS_PagingAndView("", "");
            string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);
            var data = new
            {
                Result = new CGPSI_AbsenceDBEntities().SP_DEPARTMENTS_PagingAndView("", ""), // .MstActivityCategory_getList(filters),
                CResult = new CGPSI_AbsenceDBEntities().Departments.Count()
            };
            return Json(data);
            //return Json(null);
        }

        public ActionResult Put(Department data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_DEPARTMENTS(data.ID_Department, data.DepartmentName, DateTime.Now, 1, DateTime.Now, 1, "Insert"));
        }

        public ActionResult Post(Department data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_DEPARTMENTS(data.ID_Department, data.DepartmentName, DateTime.Now, 1, DateTime.Now, 1, "Update"));
        }

        public ActionResult Delete(Department data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_DEPARTMENTS(data.ID_Department, data.DepartmentName, DateTime.Now, 1, DateTime.Now, 1, "Delete"));
        }
    }
}