using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            return PartialView();
        }

        /// <summary>
        /// Get Data Employees for Reference View
        /// </summary>
        public ActionResult GetRefEmployee(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);
            var data = new
            {
                Result = new CGPSI_AbsenceDBEntities().SP_ViewEMPLOYEES(filters.Split('|')[0], filters.Split('|')[0]),
                CResult = new CGPSI_AbsenceDBEntities().Employees.Count()
            };
            return Json(data);
        }


        /// <summary>
        /// Get Data Employees for Master Data View
        /// </summary>
        public ActionResult Get(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {            
            string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);            
            var data = new
            {
                Result = new CGPSI_AbsenceDBEntities().SP_EMPLOYEES_PagingAndView(filters.Split('|')[0], filters.Split('|')[0]),
                CResult = new CGPSI_AbsenceDBEntities().Employees.Count()
            };
            return Json(data);
        }

        /// <summary>
        /// Insert New Data Employees
        /// </summary>
        public ActionResult Put(Employee data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_EMPLOYEES(data.ID_Employee,data.NIK,data.FirstName,data.LastName,data.DisplayName,data.Address,data.Telephone,
                data.Email,data.BirthDate,data.BirthPlace,data.JoinDate,data.CurrentPosition,data.CurrentDepartment,data.IsActive,data.LeaveDate,data.ID_Supervisor,DateTime.Now,1,DateTime.Now,1,"Insert"));
        }

        /// <summary>
        /// Update Data Employees
        /// </summary>
        public ActionResult Post(Employee data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_EMPLOYEES(data.ID_Employee, data.NIK, data.FirstName, data.LastName, data.DisplayName, data.Address, data.Telephone,
                data.Email, data.BirthDate, data.BirthPlace, data.JoinDate, data.CurrentPosition, data.CurrentDepartment, data.IsActive, data.LeaveDate, data.ID_Supervisor, DateTime.Now, 1, DateTime.Now, 1, "Update"));
        }

        /// <summary>
        /// Delete Data Employees
        /// </summary>
        public ActionResult Delete(Employee data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_EMPLOYEES(data.ID_Employee,data.NIK,data.FirstName,data.LastName,data.DisplayName,data.Address,data.Telephone,
                data.Email,data.BirthDate,data.BirthPlace,data.JoinDate,data.CurrentPosition,data.CurrentDepartment,data.IsActive,data.LeaveDate,data.ID_Supervisor,DateTime.Now,1,DateTime.Now,1,"Delete"));
        }


    }
}