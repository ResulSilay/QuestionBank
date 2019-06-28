using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestionBank.Models;

namespace QuestionBank.Classes
{
    public class Methods : Controller
    {
        QBDBContext db;

        public Methods()
        { }

        public Methods(QBDBContext db)
        {
            this.db = db;
        }

        public int getAccount()
        {
            int ID = -1;
            try
            {
                ID = Convert.ToInt32(HttpContext.Session.GetString("AccountID"));
            }
            catch
            {
            }

            return ID;
        }

        public string getHash(string password)
        {
            //password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            //password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");

            return password;
        }

        public DateTime getDateFormat(string date, string format)
        {
            return DateTime.Parse(DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture).ToString(format), System.Globalization.CultureInfo.InvariantCulture);
        }

        public bool IsNull(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return true;
            else
                return false;
        }

        public string Safety(string value)
        {
            return value;
        }

        public int get_Account_ID()
        {
            int id = 0;
            try
            {
                //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                //id = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault());
                ////id = Convert.ToInt32(from x in db.Accounts where x.Username == name select x.Id);
                //id = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault());
                //Debug.WriteLine("ID---->"+id.ToString());
                id = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault());
            }
            catch { }
            return id > 0 ? id : -1;
        }

        public bool get_Account_VIP(int id)
        {
            int vip = 0;
            try
            {
                vip = Convert.ToInt32((from x in db.Accounts where x.Id == id select x.Vip).Single());
                //Debug.WriteLine("VIP:"+vip.ToString());
                //Debug.WriteLine("ID:"+ ID.ToString());
            }
            catch { }
            return vip == 1 ? true : false;
        }
    }
}