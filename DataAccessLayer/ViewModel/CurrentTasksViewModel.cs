using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CurrentTasksViewModel
    {
        public int TaskId { get; set; }
        public string? TaskServiceCategoryId { get; set; }
        public string? TaskServiceId { get; set; }
        public string? TaskTitle { get; set; }
        public DateTime? TaskDeadlineDateTime { get; set; }
        public string? TaskRequesterDescription { get; set; }
        public string? TaskRequesterAttatchmentsUrl { get; set; }
        public string? TaskRequesterAttatchmentFilePath { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskRequesterAccountId { get; set; }
        public string? TaskResponderAccountId { get; set; }
        public string? TaskResponserDescription { get; set; }
        public string? TaskResponserAttatchmentsUrl { get; set; }
        public string? TaskResponserAttatchmentFilePath { get; set; }
        public string? TaskCost { get; set; }
        public DateTime? TaskResponseDateTime { get; set; }
    }
}
