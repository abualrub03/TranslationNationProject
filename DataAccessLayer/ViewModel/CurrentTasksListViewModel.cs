using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CurrentTasksListViewModel
    {
       
        public List<CurrentTasksViewModel>? currentTasksViewModels { get; set; }
        public Entities.Accounts? accounts { get; set; }
        public List<Country>? countries { get; set; }
        public List<University>? universities { get; set; }
        public string? universityName { get; set; }
        public int universityNumberOfStudents { get; set; }
        public List<Accounts>? NotVerifiedUniversityStudent { get; set; }
        public List<Offers>? offers { get; set; }

    }
}
