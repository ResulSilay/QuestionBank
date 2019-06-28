using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QuestionBank.Classes
{
    public class Database : Controller
    {
        readonly String connectionString = "Server=(localdb)\\MSSQLLocalDB; Database=QBDB; Trusted_Connection=True; MultipleActiveResultSets=true";
        SqlConnection con;

        public Database()
        {
            con = new SqlConnection(connectionString);
            con.Open();
        }

        public string get(string command)
        {
            SqlCommand com = new SqlCommand(command, con);
            var reader = com.ExecuteReader();

            if (reader.Read())
                return reader[0].ToString();

            return null;
        }

        public int getInt(string command)
        {
            SqlCommand com = new SqlCommand(command, con);
            var reader = com.ExecuteReader();

            if (reader.Read())
                return Convert.ToInt32(reader[0].ToString());

            return -1;
        }

        public bool getBool(string command)
        {
            SqlCommand com = new SqlCommand(command, con);
            var reader = com.ExecuteReader();

            if (reader.Read())
                return true;

            return false;
        }
    }
}