using System;
using System.Collections.Generic;

namespace QuestionBank.Models
{
    public partial class Accounts
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public DateTime? Date { get; set; }
        public int? Vip { get; set; }
        public DateTime? VipDatetime { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
    }
}
