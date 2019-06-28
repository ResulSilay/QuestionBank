using System;
using System.Collections.Generic;

namespace QuestionBank.Models
{
    public partial class Buggy
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? QId { get; set; }
        public int? QType { get; set; }
        public string Description { get; set; }
        public DateTime? Datetime { get; set; }
        public int? Status { get; set; }
    }
}
