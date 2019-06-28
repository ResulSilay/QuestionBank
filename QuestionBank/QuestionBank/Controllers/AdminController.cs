using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuestionBank.Classes;
using QuestionBank.Models;

namespace QuestionBank.Controllers
{
    public class AdminController : Controller
    {
        QBDBContext db = new QBDBContext();
        Database database = new Database();

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.count_questions_test_on = db.QuestionsTests.Count(x => x.Type == 1);
            ViewBag.count_questions_test_off = db.QuestionsTests.Count(x => x.Type == 0);
            ViewBag.count_questions_test_all = db.QuestionsTests.Count();

            ViewBag.count_questions_correct_on = db.QuestionsCorrects.Count(x => x.Type == 1);
            ViewBag.count_questions_correct_off = db.QuestionsCorrects.Count(x => x.Type == 0);
            ViewBag.count_questions_correct_all = db.QuestionsCorrects.Count();

            ViewBag.count_buggy_all = db.Buggy.Count(x => x.Status == 1);
            ViewBag.count_account_all = db.Accounts.Count(); 
            ViewBag.count_account_on = db.Accounts.Count(x => x.Status == 1); 
            ViewBag.count_account_pending = db.Accounts.Count(x => x.Status == 0); 
            ViewBag.count_account_vip = db.Accounts.Count(x => x.Vip == 1);

            return View();
        }

        [Route("Admin/Accounts/")]
        public IActionResult Accounts()
        {
            return View(db.Accounts.ToList());
        }

        [Route("Admin/Accounts/Edit")]
        public ActionResult Accounts_Edit(int id)
        {
            var table = db.Accounts.Where(x => x.Id == id).FirstOrDefault();
            return View(table);
        }

        [HttpPost]
        [Route("Admin/Accounts/Edit")]
        public ActionResult Accounts_Edit(int id,IFormCollection collection)
        {

            string data = (collection["Status"].ToString() + ":" + collection["Type"].ToString()+":"+ collection["Vip"].ToString());
            Debug.WriteLine("DATA---->"+data);

            //int id = Convert.ToInt32(collection["Id"]);

            int status = collection["Status"] == true ? 1 : 2;
            int type = bool.Parse(collection["Type"]) == true ? 1 : 2;
            int vip = bool.Parse(collection["Vip"]) == true ? 1 : 0;


            var account = db.Accounts.Find(id);
            account.Vip = vip;
            account.Status = status;
            account.Type = vip;
            db.SaveChanges();

            return Redirect("../Accounts");


            //string data = (test+ ":" + collection["Status"].ToString() + ":" + collection["Type"].ToString());
            //return Redirect(data);
        }

        [Route("Admin/Accounts/Delete")]
        public IActionResult Accounts(int id)
        {
            var account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return View();
        }

        [Route("Admin/Lectures/")]
        public IActionResult Lectures()
        {
            return View(db.Lectures.ToList());
        }

        [Route("Admin/Lectures/Create")]
        public IActionResult Lectures_Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/Lectures/Create")]
        public IActionResult Lectures_Create(IFormCollection collection)
        {
            Lectures lectures = new Lectures();
            lectures.Name = collection["Name"];
            lectures.Description = collection["Description"];
            db.Lectures.Add(lectures);
            db.SaveChanges();

            return Redirect("Admin/Lectures");
        }

        [Route("Admin/Lectures/Delete")]
        public ActionResult Lectures_Delete(int id)
        {
            var table = db.Lectures.Where(x => x.Id == id).FirstOrDefault();
            return View(table);
        }

        [HttpPost]
        [Route("Admin/Lectures/Delete")]
        public ActionResult Lectures_Delete(int id, IFormCollection collection)
        {
            var lectures = db.Lectures.Find(id);
            db.Lectures.Remove(lectures);
            db.SaveChanges();
            return View();
        }

        [Route("Admin/Lectures/Edit")]
        public ActionResult Lectures_Edit(int id)
        {
            var table = db.Lectures.Where(x => x.Id == id).FirstOrDefault();
            return View(table);
        }

        [HttpPost]
        [Route("Admin/Lectures/Edit")]
        public ActionResult Lectures_Edit(int id, IFormCollection collection)
        {
            var lectures = db.Lectures.Find(id);
            lectures.Name = collection["Title"];
            lectures.Description = collection["Title"];
            db.SaveChanges();
            return View();
        }


        [Route("Admin/Subjects/")]
        public IActionResult Subjects(int id)
        {
            return View(db.Subjects.ToList().Where(sub => sub.LectureId==id));
        }

        [Route("Admin/Subjects/Create")]
        public IActionResult Subjects_Create()
        {
            ViewBag.ListOfLectures = get_List_Lectures();
            return View();
        }

        [HttpPost]
        [Route("Admin/Subjects/Create")]
        public IActionResult Subjects_Create(IFormCollection collection)
        {
            Subjects subjects = new Subjects();
            subjects.LectureId = Convert.ToInt32(collection["LectureId"]);
            subjects.Name = collection["Name"];
            subjects.Description = collection["Description"];
            db.Subjects.Add(subjects);
            db.SaveChanges();

            ViewBag.ListOfLectures = get_List_Lectures();
            return View();
        }

        [Route("Admin/Questions/Tests")]
        public IActionResult Questions_Tests(int subject_id,int lecture_id)
        {
            return View(db.QuestionsTests.ToList().Where(que => que.SubjectId == subject_id && que.LectureId==lecture_id));
        }

        [Route("Admin/Questions/Corrects")]
        public IActionResult Questions_Corrects(int subject_id, int lecture_id)
        {
            return View(db.QuestionsCorrects.ToList().Where(que => que.SubjectId == subject_id && que.LectureId == lecture_id));
        }

        [Route("Admin/Buggy/")]
        public IActionResult Buggy(int id)
        {
            return View(db.Buggy.ToList());
        }

        //[Route("Admin/Load")]
        //public IActionResult Load()
        //{
        //    return View();
        //}

        [Route("Admin/Load")]
        public IActionResult Load(IFormCollection collection)
        {
            OnPostImport();
            return View();
        }

        public ActionResult OnPostImport()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");// _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        sb.Append("<table class='mdl - data - table mdl - js - data - table mdl - data - table--selectable mdl - shadow--2dp'><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                            }

                            QuestionsTests test = new QuestionsTests();
                            test.LectureId = 1;
                            test.SubjectId = 1;
                            test.Title = row.GetCell(2).ToString();
                            test.A = row.GetCell(3).ToString();
                            test.B = row.GetCell(4).ToString();
                            test.C = row.GetCell(5).ToString();
                            test.D = row.GetCell(6).ToString();
                            test.E = row.GetCell(7).ToString();
                            test.Result = 1;
                            test.Levels = 1;
                            test.Status = 1;
                            test.Type = 1;

                            db.QuestionsTests.Add(test);
                            db.SaveChanges();

                            sb.AppendLine("</tr>");
                        }
                        sb.Append("</table>");
                    }
                }
                //Debug.WriteLine(sb.ToString());
                ViewBag.table  = sb.ToString();
                return this.Content(sb.ToString());
            }
            catch { }

            return null;
        }

        List<Lectures> get_List_Lectures()
        {
            List<Lectures> lecturesList = new List<Lectures>();
            lecturesList = (from Lectures in db.Lectures select Lectures).ToList();
            lecturesList.Insert(0, new Lectures { Id = 0, Name = "Select" });

            return lecturesList;
        }

        List<Subjects> get_List_Subjects()
        {
            List<Subjects> subjectsList = new List<Subjects>();
            subjectsList = (from Subjects in db.Subjects select Subjects).ToList();
            subjectsList.Insert(0, new Subjects { Id = 0, Name = "Select" });
            return subjectsList;
        }
    }
}
