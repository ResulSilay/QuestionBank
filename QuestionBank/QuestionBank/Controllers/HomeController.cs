using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestionBank.Classes;
using QuestionBank.Models;

namespace QuestionBank.Controllers
{
    public class HomeController : Controller
    {
        QBDBContext db = new QBDBContext();
        Methods methods;

        public HomeController()
        {
            methods = new Methods(db);
        }

        public IActionResult Index()
        {
            return View(db.Lectures.ToList());
        }

        [Route("Subjects")]
        public IActionResult Subjects(int id)
        {
            return View(db.Subjects.ToList().Where(sub => sub.LectureId == id));
        }

        [Route("/Questions/Test")]
        public IActionResult Questions_Tests(int id)
        {
            return View(db.QuestionsTests.ToList().Where(sub => sub.SubjectId == id));
        }

        [Route("Buggy")]
        public IActionResult Buggy()
        {
            return View();
        }

        [HttpGet]
        [Route("Buggy")]
        public IActionResult Buggy(int id, int qtype)
        {
            int account_id = methods.get_Account_ID();
            //Response.WriteAsync("ID:"+account_id.ToString());
            bool vip = methods.get_Account_VIP(account_id);

            Debug.WriteLine("---> ID:" + account_id.ToString());
            Debug.WriteLine("---> VIP:" + vip.ToString());

            if (vip)
            {
                Buggy buggy = new Buggy();
                buggy.AccountId = account_id;
                buggy.QId = id;
                buggy.QType = qtype; ;
                db.Buggy.Add(buggy);
                db.SaveChanges();

                Response.WriteAsync("<script>alert('Bildirim gönderildi.');</script>");
            }
            else
            {
                Response.WriteAsync("<script>alert('VIP üye değilsiniz.');</script>");
            }

            return View();
        }

        [Route("/Questions/Insert")]
        public IActionResult Questions_Insert()
        {
            ViewBag.ListOfLectures = get_List_Lectures();
            return View();
        }
        
        [HttpGet]
        //[Route("/Questions/Insert/{0}",Name ="id")]
        public IActionResult Questions_Insert(int id)
        {
            //List<QuestionsTests> qList =db.QuestionsTests.ToList().Where(sub => sub.SubjectId == id).ToList();
            //ViewBag.qList = qList;
            //value = "@((ViewBag.qList as List<QuestionsTests>).First().Title)"
            ViewBag.ListOfLectures = get_List_Lectures();
            return View();
        }

        //[Route("/Questions/Insert")]
        public IActionResult Questions_Delete(int id)
        {
            var test = db.QuestionsTests.Find(id);
            db.QuestionsTests.Remove(test);
            db.SaveChanges();
            return View();
        }

        [Route("Search")]
        public IActionResult Search()
        {
            return View(db.Subjects.ToList());
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string search_key)
        {
            //Response.WriteAsync(search_key);
            //string key = collection["search_key"].ToString();
            Debug.WriteLine("Search: "+ search_key.ToString());
            Debug.WriteLine("Search: merhaba");
            return View(db.Subjects.Where(item => item.Name.Contains(search_key)).ToList());
            //return View(db.Subjects.ToList().Where(sub => sub.Name == collection["search_key"].ToString()));
        }

        public JsonResult GetSubjects(int LectureId)
        {
            List<Subjects> subjectsList = new List<Subjects>();
            subjectsList = (from Subjects in db.Subjects where Subjects.LectureId == LectureId select Subjects).ToList();
            subjectsList.Insert(0, new Subjects { Id = 0, Name = "Select" });

            return Json(new SelectList(subjectsList, "Id", "Name"));
        }

        [HttpPost]
        [Route("/Questions/Insert")]
        public IActionResult Questions_Insert(IFormCollection collection)
        {
            var l_id = collection["LectureId"];
            var s_id = collection["SubjectId"];

            if (collection["tf_true"].ToString() != null)
            {
                QuestionsCorrects questionsCorrects = new QuestionsCorrects();
                questionsCorrects.AccountId = methods.get_Account_ID();
                questionsCorrects.LectureId = Convert.ToInt32(l_id);
                questionsCorrects.SubjectId = Convert.ToInt32(s_id);
                questionsCorrects.Title = collection["title"];
                questionsCorrects.QTrue = collection["tf_true"];
                questionsCorrects.QFalse = collection["tf_false"];
                questionsCorrects.Result = Convert.ToInt32(collection["tf_Result"]);
                questionsCorrects.Levels = Convert.ToInt16(collection["q_type"]);
                db.QuestionsCorrects.Add(questionsCorrects);
            }
            else
            {
                QuestionsTests test = new QuestionsTests();
                test.AccountId = methods.get_Account_ID();
                test.LectureId = Convert.ToInt32(l_id);
                test.SubjectId = Convert.ToInt32(s_id);
                test.Title = collection["title"];
                test.A = collection["A"];
                test.B = collection["B"];
                test.C = collection["C"];
                test.D = collection["D"];
                test.E = collection["E"];
                test.Result = Convert.ToInt32(collection["Result"]);
                //test.q_type = collection["q_type"];
                db.QuestionsTests.Add(test);
            }

            db.SaveChanges();

            ViewBag.ListOfLectures = get_List_Lectures();
            return View();
        }

        List<Lectures> get_List_Lectures()
        {
            List<Lectures> lecturesList = new List<Lectures>();
            lecturesList = (from Lectures in db.Lectures select Lectures).ToList();
            lecturesList.Insert(0, new Lectures { Id = 0, Name = "Select" });

            return lecturesList;
        }
    }
}