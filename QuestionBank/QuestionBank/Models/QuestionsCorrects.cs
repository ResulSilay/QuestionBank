using System;
using System.Collections.Generic;

namespace QuestionBank.Models
{
    public partial class QuestionsCorrects
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? LectureId { get; set; }
        public int? SubjectId { get; set; }
        public int? Levels { get; set; }
        public string Title { get; set; }
        public string QTrue { get; set; }
        public string QFalse { get; set; }
        public int? Result { get; set; }
        public DateTime? Datetime { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
    }
}
