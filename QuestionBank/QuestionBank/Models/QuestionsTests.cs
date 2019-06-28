using System;
using System.Collections.Generic;

namespace QuestionBank.Models
{
    public partial class QuestionsTests
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? LectureId { get; set; }
        public int? SubjectId { get; set; }
        public int? Levels { get; set; }
        public string Title { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public int? Result { get; set; }
        public DateTime? Datetime { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
    }
}
