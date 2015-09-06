using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class LeaveRequestController : Controller
    {
        //
        // GET: /LeaveRequest/
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Get Data Employees for Master Data View
        /// </summary>
        public ActionResult Get(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);
            var data = new
            {
                Result = new CGPSI_AbsenceDBEntities().SP_ViewLeaveRequest(filters.Split('|')[0], filters.Split('|')[0]),
                CResult = new CGPSI_AbsenceDBEntities().LeaveRequests.Count()
            };
            return Json(data);
        }

        /// <summary>
        /// Insert/Update New Data
        /// </summary>
        public ActionResult Post(ViewLeaveRequest data)
        {
            LeaveRequest lr = new LeaveRequest()
            {
                ID_LeaveRequest=data.ID_LeaveRequest,
                ID_Requestor=data.ID_Requestor,
                ID_Approver=data.ID_Approver,
                LeaveCategory=data.ID_LeaveCategory,
                LeaveReason=data.LeaveReason,
                StartDate=data.StartDate,
                EndDate=data.EndDate,
                CreatedBy=0,
                ModifiedBy=0,
                ModifiedDate=DateTime.Now,
                CreatedDate=DateTime.Now
            };
            
            if (data.ID_LeaveRequest==0)
            {
                //Add New Item
                new CGPSI_AbsenceDBEntities().LeaveRequests.Add(lr);
            }
            else
            {
                var temp =new CGPSI_AbsenceDBEntities();
                temp
                if(temp.)
            }
            return Json(true);
        }

        /// <summary>
        /// Delete Data
        /// </summary>
        public ActionResult Delete(Employee data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_EMPLOYEES(data.ID_Employee, data.NIK, data.FirstName, data.LastName, data.DisplayName, data.Address, data.Telephone,
                data.Email, data.BirthDate, data.BirthPlace, data.JoinDate, data.CurrentPosition, data.CurrentDepartment, data.IsActive, data.LeaveDate, data.ID_Supervisor, DateTime.Now, 1, DateTime.Now, 1, "Delete"));
        }
	}
}