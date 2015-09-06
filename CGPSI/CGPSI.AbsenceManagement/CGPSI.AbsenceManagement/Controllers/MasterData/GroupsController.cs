using CGPSI.AbsenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class GroupsController : Controller
    {
        // GET: Groups
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            var temp = new CGPSI_AbsenceDBEntities().SP_GROUPS_PagingAndView("", "");
            string filters = KendoHelper.GenerateFilters(take, page, skip, pageSize, sort, filter);
            var data = new
            {
                Result = new CGPSI_AbsenceDBEntities().SP_GROUPS_PagingAndView("", ""), // .MstActivityCategory_getList(filters),
                CResult = new CGPSI_AbsenceDBEntities().Groups.Count()
            };
            return Json(data);
            //return Json(null);
        }
         
        public ActionResult Put(Group data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_GROUPS(data.ID_Group, data.GroupName, "Insert"));
        }

        public ActionResult Post(Group data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_GROUPS(data.ID_Group, data.GroupName, "Update"));
        }

        public ActionResult Delete(Group data)
        {
            return Json(new CGPSI_AbsenceDBEntities().SP_GROUPS(data.ID_Group, data.GroupName, "Delete"));
        }
    }
}