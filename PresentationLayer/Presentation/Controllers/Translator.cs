using Microsoft.AspNetCore.Mvc;
using TranslationNation.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TranslationNation.Web.Controllers
{
    public class Translator : AuthorizedController
    {
        public IActionResult TranslatorIndex()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            var t = new RacoonProvider.TN_DB_Tasks().GetAllNotAssignTasks();
            currentTasksListViewModel.currentTasksViewModels = t;
            return View("TranslatorIndex", currentTasksListViewModel);
        }
        [HttpPost]
        public IActionResult TaskViewDetails(int Id)
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().get_TaskDetailsOnTaskId(Id);

            return View("TaskViewDetails", currentTasksListViewModel);
        } 
        
        public IActionResult PickedTaskViewDetails(int Id)
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().get_TaskDetailsOnTaskId(Id);

            return View("PickedTaskViewDetails", currentTasksListViewModel);
        }
        [HttpPost]
        public IActionResult PickTask(int Id)
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            new RacoonProvider.TN_DB_Tasks().AssignTaskToTranslator(Id , GetCurrentUser().AccountId);
            return View("CurrentTasks", currentTasksListViewModel);
        }
        [HttpPost]
        public IActionResult sendOffer(int taskId  , string description, string deadline , Double price)
        {

            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().newOffer( taskId, GetCurrentUser().FirstName+ " "+ GetCurrentUser().SecondName, deadline , description,    price , GetCurrentUser().AccountId);

            return View("CurrentTasks", currentTasksListViewModel);
        } 
        public IActionResult CurrentTasks()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            var i = GetCurrentUser().AccountId;
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().TasksAssignedToTranslator(GetCurrentUser().AccountId);



            return View("CurrentTasks", currentTasksListViewModel);
        }
        [HttpPost]
         public async Task<IActionResult> submitTask(  int TaskId , string taskDescription, string documentUrl , IFormFile uploadedDocument )
            {
            string uniqueFileName = null;
          
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Documents");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedDocument.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedDocument.CopyToAsync(stream);
            }
            
            Entities.Tasks task = new Entities.Tasks();
            task.TaskId = TaskId;
            task.TaskResponderDescription = taskDescription;
            task.TaskResponderAttachmentFileName = uniqueFileName;
            task.TaskResponderAccountId = GetCurrentUser().AccountId;
            task.TaskResponderAttachmentsUrl = documentUrl;
            var a = new RacoonProvider.TN_DB_Tasks().submitTask(task);



            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().TasksAssignedToTranslator(GetCurrentUser().AccountId);



            return View("CurrentTasks", currentTasksListViewModel);
            }


    }
}
