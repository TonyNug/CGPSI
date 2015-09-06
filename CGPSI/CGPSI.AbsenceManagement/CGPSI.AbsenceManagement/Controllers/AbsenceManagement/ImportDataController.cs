using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CGPSI.AbsenceManagement.Models;
using System.Globalization;

namespace CGPSI.AbsenceManagement.Controllers.AbsenceManagement
{
    public class ImportDataController : Controller
    {
        // GET: ImportData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            string physicalPath = string.Empty;
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var fileFullPath = Path.Combine(Server.MapPath("~/App_Data/DataImportAbsence"), fileName);
                    
                    if (!System.IO.File.Exists(fileFullPath))
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/App_Data/DataImportAbsence"), fileName);
                    }
                    else
                    {
                        fileName = fileName.Split('.')[0].ToString() + "_" + DateTime.Now.ToString("ddMMyy_hh_mm_ss.") + fileName.Split('.')[1].ToString();
                        physicalPath = Path.Combine(Server.MapPath("~/App_Data/DataImportAbsence"), fileName);
                    }

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Json(physicalPath);
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

        public ActionResult ImportData()
        {
            object output = null;
            try
            {
                using (CGPSI_AbsenceDBEntities cgpsi = new CGPSI_AbsenceDBEntities())
                {
                    DateTime importStart = DateTime.Now;
                    string fileSource = string.Empty,
                        ImportSession = Guid.NewGuid().ToString() + DateTime.Now.ToString("ddMMyy_hhmmss");
                    int dataCount = 0, dataError = 0;
                    var listFile = System.IO.Directory.GetFiles(Server.MapPath("~/App_Data/DataImportAbsence"));
                    foreach (var file in listFile)
                    {
                        if (!System.IO.File.Exists(file))
                            throw new System.IO.FileNotFoundException(file + " Is not Found...");
                        if (file.Split('.')[file.Split('.').Count() - 1].ToString().ToLower() == "bak")
                        {
                            var items = System.IO.File.ReadAllLines(file);
                            foreach (string item in items)
                            {

                                try
                                {
                                    cgpsi.DataAbsences.Add(new DataAbsence()
                                    {
                                        NIK = item.Substring(5, 4),
                                        AbsenceDate = DateTime.ParseExact(item.Substring(9, 6), "ddMMyy", CultureInfo.InvariantCulture),
                                        AbsenceTime = TimeSpan.ParseExact(item.Substring(15, 6), "hhmmss", CultureInfo.InvariantCulture),
                                        Flag = int.Parse(item.Substring(24, 2)),
                                        ImportSession = ImportSession
                                    });
                                    cgpsi.SaveChanges();
                                }
                                catch { dataError++; }
                                dataCount++;
                            }
                        }
                        else
                        {
                            System.IO.File.Delete(file);
                        }
                        //Move File to Archive
                        System.IO.File.Move(file, file.Replace("DataImportAbsence", "ImportedData"));
                    }

                    var history = new ImportHistory()
                    {
                        ImportSession = ImportSession,
                        DataError = dataError,
                        DataExsist = 0,
                        DataImported = dataCount,
                        DataUpdated = 0,
                        ImportDate = DateTime.Now,
                        ImportStart = importStart,
                        ImportEnd = DateTime.Now,
                        FileSource = string.Join(", ", listFile.ToList().Select(s => s.Split('\\').Last()))
                    };
                    //add History
                    cgpsi.ImportHistories.Add(history);
                    cgpsi.SaveChanges();

                    output = new
                    {
                        ImportSummary = history,
                        status = true
                    };

                    cgpsi.SP_SaveAbsenceHistory();
                }
            }
            catch (Exception ex)
            {
                output = new
                {
                    ImportSummary = "",
                    DataImport = "",
                    status=false
                };
            }
            return Json(output);
        }


    }
}