using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CGPSI.AbsenceManagement.Models;
using System.Security.Cryptography;

namespace CGPSI.AbsenceManagement.Controllers.MasterData
{
    public class AttachmentsController : Controller
    {
        //
        // GET: /Attachments/
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            string physicalPath = string.Empty;
            // The Name of the Upload component is "files"
            if (files != null)
            {
                using(CGPSI_AbsenceDBEntities cgpsi=new CGPSI_AbsenceDBEntities())
                {
                    foreach (var file in files)
                    {
                        string SHA256=BitConverter.ToString(new SHA256Managed().ComputeHash(file.InputStream)).Replace("-", String.Empty);
                        if(cgpsi.Attachments.Where(t=>t.MD5FileIdentity==SHA256).ToList().Count==0)
                        {
                            cgpsi.Attachments.Add(new Attachment()
                            {
                                FileName=Path.GetFileName(file.FileName),
                                FileSize=file.ContentLength,
                                MimeType=file.ContentType,
                                FileValue= new BinaryReader(file.InputStream).ReadBytes(file.ContentLength),
                                MD5FileIdentity = SHA256
                            });
                            cgpsi.SaveChanges();
                        }
                    }
                }
            }

            // Return an empty string to signify success
            return Json(physicalPath);
        }


        /// <summary>
        /// Download file attachment
        /// </summary>
        [HttpPost,HttpGet]
        public ActionResult Download(string id)
        {
            CGPSI_AbsenceDBEntities cgpsi=new CGPSI_AbsenceDBEntities();
            var file=cgpsi.Attachments.Where(t=>t.ID_Attachment==int.Parse(id)).FirstOrDefault();
            return File(file.FileValue, file.MimeType, file.FileName);
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/DataImportAbsence"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }

	}
}