using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CurrentTasksListViewModel
    {
        public List<CurrentTasksViewModel> currentTasksViewModels { get; set; }
        public Entities.Accounts accounts { get; set; }  
        
    }
}
