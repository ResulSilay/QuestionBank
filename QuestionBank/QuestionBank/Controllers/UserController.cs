using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionBank.Classes;
using QuestionBank.Models;

namespace QuestionBank.Controllers
{
    public class UserController : Controller
    {
        QBDBContext db = new QBDBContext();
        Methods methods;

        public UserController()
        {
            methods = new Methods(db);
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.count_questions_test_on = db.QuestionsTests.Count(x => x.Type == 1);
            ViewBag.count_questions_test_off = db.QuestionsTests.Count(x => x.Type == 0);
            ViewBag.count_questions_test_all = db.QuestionsTests.Count();
            return View();
        }

        //[Authorize(Roles ="User")]
        [Route("User/Questions")]
        public IActionResult Questions_Test()
        {
            return View(db.QuestionsTests.ToList().Where(x=>x.AccountId==methods.get_Account_ID()));
        }

        [Route("User/Questions/Detail")]
        public IActionResult Questions_Test_Details(int id)
        {
            var table = db.QuestionsTests.Where(x => x.Id == id).FirstOrDefault();
            return View(table);
        }

        [Route("User/Questions/Delete")]
        public IActionResult Questions_Test_Delete(int id)
        {
            var question = db.QuestionsTests.Find(id);
            db.QuestionsTests.Remove(question);
            db.SaveChanges();
            return View();
        }
    }
}
