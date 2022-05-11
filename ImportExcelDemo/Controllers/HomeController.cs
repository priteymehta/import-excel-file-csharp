using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImportExcelDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase files)
        {
            var count = Request.Files.Count;
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    // get a stream
                    var stream = fileContent.InputStream;
                    // and optionally write the file to disk
                    var fileName = Path.GetFileName(file);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    //using (var fileStream = File.Create(path))
                    //{
                    //    stream.CopyTo(fileStream);
                    //}
                }
            }
            //if (file != null && file.ContentLength > 0)
            //{
            //    var fileName = Path.GetFileName(file.FileName);
            //    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            //    file.SaveAs(path);
            //}

            return RedirectToAction("UploadDocument");
        }


        public JsonResult Upload(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    CsvParser csvParser = new CsvParser(new StreamReader(file.InputStream));
                    CsvReader csvReader = new CsvReader(csvParser);
                    //string[] headers = { };
                    List<string[]> rows = new List<string[]>();
                    string[] row;
                    while (csvReader.Read())
                    {
                        List<string> headers = csvReader.Context.HeaderRecord.ToList();
                        //row = new string[headers.Count()];
                        //for (int j = 0; j < headers.Count(); j++)
                        //{
                        //    row[j] = csvReader.GetField(j);
                        //}
                        //rows.Add(row);
                    }
                }
            }
            return Json("Uploaded", JsonRequestBehavior.AllowGet);
        }

        #region [Private Method]


   

        public bool CompareString(string strx, string stry)
        {
            if (strx == null) //stry may contain only whitespace
                return string.IsNullOrWhiteSpace(stry);

            else if (stry == null) //strx may contain only whitespace
                return string.IsNullOrWhiteSpace(strx);

            int ix = 0, iy = 0;
            for (; ix < strx.Length && iy < stry.Length; ix++, iy++)
            {
                char chx = strx[ix];
                char chy = stry[iy];

                //ignore whitespace in strx
                while (char.IsWhiteSpace(chx) && ix < strx.Length)
                {
                    ix++;
                    chx = strx[ix];
                }

                //ignore whitespace in stry
                while (char.IsWhiteSpace(chy) && iy < stry.Length)
                {
                    iy++;
                    chy = stry[iy];
                }

                if (ix == strx.Length && iy != stry.Length)
                { //end of strx, so check if the rest of stry is whitespace
                    for (int iiy = iy + 1; iiy < stry.Length; iiy++)
                    {
                        if (!char.IsWhiteSpace(stry[iiy]))
                            return false;
                    }
                    return true;
                }

                if (ix != strx.Length && iy == stry.Length)
                { //end of stry, so check if the rest of strx is whitespace
                    for (int iix = ix + 1; iix < strx.Length; iix++)
                    {
                        if (!char.IsWhiteSpace(strx[iix]))
                            return false;
                    }
                    return true;
                }

                //The current chars are not whitespace, so check that they're equal (case-insensitive)
                //Remove the following two lines to make the comparison case-sensitive.
                chx = char.ToLowerInvariant(chx);
                chy = char.ToLowerInvariant(chy);

                if (chx != chy)
                    return false;
            }

            //If strx has more chars than stry
            for (; ix < strx.Length; ix++)
            {
                if (!char.IsWhiteSpace(strx[ix]))
                    return false;
            }

            //If stry has more chars than strx
            for (; iy < stry.Length; iy++)
            {
                if (!char.IsWhiteSpace(stry[iy]))
                    return false;
            }

            return true;
        }

        public int GetHashCode(string obj)
        {
            if (obj == null)
                return 0;

            int hash = 17;
            unchecked // Overflow is fine, just wrap
            {
                for (int i = 0; i < obj.Length; i++)
                {
                    char ch = obj[i];
                    if (!char.IsWhiteSpace(ch))
                        //use this line for case-insensitivity
                        hash = hash * 23 + char.ToLowerInvariant(ch).GetHashCode();

                    //use this line for case-sensitivity
                    //hash = hash * 23 + ch.GetHashCode();
                }
            }
            return hash;
        }
        #endregion

    }
}