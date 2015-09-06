using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.AbsenceManagement
{
    public class AbsenceDataController : Controller
    {
        // GET: AbsenceData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get()//(DateTime start,DateTime end)//(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            //string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);
            var result=new CGPSI_AbsenceDBEntities().SP_GetDataAbsenceOfCurrentMonth().ToList();
            var data = new
            {
                Result = result
                    .Select(a => new
                    {
                        StartTime = a.StartTime.Value.ToString(@"hh\:mm\:ss"),
                        EndTime = a.EndTime.Value.ToString(@"hh\:mm\:ss"),
                        FirstName = a.FirstName,
                        Department = a.Department,
                        NIK = a.NIK,
                        Dates = a.Dates,
                        TimeIN = (a.TimeIN != null ? a.TimeIN.Value.ToString(@"hh\:mm\:ss") : ""),
                        TimeOUT = (a.TimeOUT != null ? a.TimeOUT.Value.ToString(@"hh\:mm\:ss") : ""),
                        actualDayname = a.DayName,
                        ShiftName = a.ShiftName,
                        AbsenceStatus = a.AbsenceStatus
                    }),
                //CResult = new CGPSI_AbsenceDBEntities().ViewAbsenceSummaries.Count()
            };
            return Json(data);
            //return Json(null);
        }
    }
}