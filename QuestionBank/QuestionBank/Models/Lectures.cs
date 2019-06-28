using System;
using System.Collections.Generic;

namespace QuestionBank.Models
{
    public partial class Lectures
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Datetime { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
    }
}
