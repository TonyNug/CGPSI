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
            return PartialView();
        }

        /// <summary>
        /// Get Data for View
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
                CGPSI_AbsenceDBEntities temp =new CGPSI_AbsenceDBEntities();
                temp.Entry(lr).CurrentValues.SetValues(lr);
                temp.SaveChanges();                
            }
            return Json(true);
        }

        /// <summary>
        /// Delete Data
        /// </summary>
        public ActionResult Delete(ViewLeaveRequest data)
        {
            LeaveRequest lr = new LeaveRequest()
            {
                ID_LeaveRequest = data.ID_LeaveRequest,
                ID_Requestor = data.ID_Requestor,
                ID_Approver = data.ID_Approver,
                LeaveCategory = data.ID_LeaveCategory,
                LeaveReason = data.LeaveReason,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                CreatedBy = 0,
                ModifiedBy = 0,
                ModifiedDate = DateTime.Now,
                CreatedDate = DateTime.Now
            };
            CGPSI_AbsenceDBEntities temp = new CGPSI_AbsenceDBEntities();
            temp.LeaveRequests.Remove(temp.LeaveRequests.Where(t=>t.ID_LeaveRequest==data.ID_LeaveRequest).FirstOrDefault());            
            return Json(temp.SaveChanges());          

        }
	}
}