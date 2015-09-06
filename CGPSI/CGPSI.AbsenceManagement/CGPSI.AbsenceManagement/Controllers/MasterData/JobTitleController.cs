using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class JobTitleController : Controller
    {
        // GET: JobTitle
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Get(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            var temp = new CGPSI_AbsenceDBEntities().SP_JOB_TITLES_PagingAndView("", "");
            string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);
            var data = new
            {
                Result = new CGPSI_AbsenceDBEntities().SP_JOB_TITLES_PagingAndView("", ""), // .MstActivityCategory_getList(filters),
                CResult = new CGPSI_AbsenceDBEntities().JobTitles.Count()
            };
            return Json(data);
            //return Json(null);
        }

        public ActionResult Put(JobTitle data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_JOB_TITLES(data.ID_JobTitle, data.JobTitleName, null, DateTime.Now, 1, DateTime.Now, 1, "Insert"));
        }

        public ActionResult Post(JobTitle data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_JOB_TITLES(data.ID_JobTitle, data.JobTitleName, null, DateTime.Now, 1, DateTime.Now, 1, "Update"));
        }

        public ActionResult Delete(JobTitle data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_JOB_TITLES(data.ID_JobTitle, data.JobTitleName, null, DateTime.Now, 1, DateTime.Now, 1, "Delete"));
        }
    }
}