using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuestionBank.Classes;
using QuestionBank.Models;

namespace QuestionBank.Controllers
{
    public class ConnectController : Controller
    {
        QBDBContext db = new QBDBContext();
        Database database = new Database();
        Methods method;

        public ConnectController()
        {
            method = new Methods(db);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Profile(IFormCollection collection)
        {
            var account = db.Accounts.Find(method.get_Account_ID());
            account.Password = collection["Password"].ToString();
            db.SaveChanges();
            return View();
        }

        public ActionResult Login()
        {
            //var value = database.get("select username from Accounts where id=1");
            //ViewData["DataUser"] = value;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(IFormCollection collection)
        {
            try
            {
                var username = collection["username"];
                var password = collection["password"];

                password = method.getHash(password);

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    int accountID = database.getInt("select id from Accounts where username='" + username + "' and password='" + password + "'");
                    if (accountID > 0)
                    {
                        int account_type = database.getInt("select type from Accounts where username='" + username + "' and password='" + password + "'");

                        if (account_type == 1)
                        {
                            //HttpContext.Session.SetString("AccountID", accountID.ToString());
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, username),
                                new Claim(ClaimTypes.Sid, accountID.ToString()),
                                new Claim(ClaimTypes.Role, "Admin")
                            };

                            var userIdentity = new ClaimsIdentity(claims, "index");

                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            await HttpContext.SignInAsync(principal);
                            Thread.CurrentPrincipal = principal;

                            return Redirect("../Admin/Index");
                        }
                        else if (account_type == 2)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, username),
                                new Claim(ClaimTypes.Sid, accountID.ToString()),
                                new Claim(ClaimTypes.Role, "User")
                            };

                            var userIdentity = new ClaimsIdentity(claims,"index");

                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            await HttpContext.SignInAsync(principal);
                            Thread.CurrentPrincipal = principal;

                            return Redirect("../User/Index");
                        }

                    }
                    else
                    {
                        ViewBag.warningMessage = "*uyarı: kullanıcı adı veya şifre hatalı";
                    }
                }
                else
                {
                    ViewBag.warningMessage = "*uyarı: kullanıcı adı veya şifre hatalı";
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message.ToString());
                ViewBag.warningMessage = e.Message.ToString();
            }

            ViewBag.warningMessage = "*uyarı: kullanıcı adı veya şifre hatalı";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            try
            {
                var username = method.Safety(collection["username"]);
                var password = method.Safety(collection["password"]);
                var email = method.Safety(collection["email"]);
                var tel = method.Safety(collection["tel"]);
                var address = method.Safety(collection["address"]);

                byte[] bytes = Encoding.ASCII.GetBytes(password);
                HashAlgorithm sha = new SHA1CryptoServiceProvider();
                byte[] pass = sha.ComputeHash(bytes);

                //password = Synercoding.FormsAuthentication.EncryptionMethod.AES().HashPasswordForStoringInConfigFile(password, "SHA1");
                //password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
                {
                    Accounts account = new Accounts();
                    account.Username = username;
                    account.Password = pass.ToString();
                    account.Email = email;
                    account.Tel = tel;
                    account.Address = address;
                    account.Type = 0;
                    account.Status = 0;

                    db.Accounts.Add(account);
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.warningMessage = "*uyarı: gerekli bilgileri doldurunuz.";
                }
            }
            catch { }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("../Connect");
        }
    }
}
