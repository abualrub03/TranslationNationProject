using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TranslationNation.Controllers;
using Microsoft.Data.SqlClient;
using System.Data;
using Entities;
using System.Threading.Tasks;

namespace TranslationNation.Web.Controllers
{
    
    public class ClientController : AuthorizedController
    {
        public IActionResult ClientIndex()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();

            return View("ClientIndex", currentTasksListViewModel);
        }
        public IActionResult TaskViewDetails()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();

            return View("TaskViewDetails", currentTasksListViewModel);
        }
        [HttpPost]
        public IActionResult TaskViewDetails(int Id)
            {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().get_TaskDetailsOnTaskId(Id);

            return View("TaskViewDetails", currentTasksListViewModel);
        }   
        public IActionResult CurrentTasks()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().get_AllTasksOnClientId(GetCurrentUser().AccountId);
            return View("CurrentTasks", currentTasksListViewModel);
        }
        public IActionResult NewServiceRequest_Document()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();

            return View("NewServiceRequest_Document", currentTasksListViewModel);
        }
        public IActionResult NewServiceRequest_Video()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            return View("NewServiceRequest_Video", currentTasksListViewModel);
        }

        public  async Task<IActionResult> signOutFromAuthorized()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Customer");
        }
        [HttpPost]
        public async Task<IActionResult> NewServiceRequest_Document(ViewModel.DocumentTranslationRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate a unique filename for the uploaded document
                string uniqueFileName = null;
                if (model.UploadedDocument != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Documents");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedDocument.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedDocument.CopyToAsync(stream);
                    }
                }
                Entities.Tasks task = new Entities.Tasks();
                task.TaskTitle= model.Title;
                task.TaskRequesterDescription= model.TaskDescription;
                task.TaskDeadlineDateTime= model.Deadline;
                task.TaskRequesterAttachmentFilePath= uniqueFileName;
                task.TaskRequesterAccountId= GetCurrentUser().AccountId;
                task.TaskRequesterAttachmentsUrl= model.DocumentUrl;
                var a = new RacoonProvider.TN_DB_Tasks().new_WrittenTranslation_DocumentTask(task);


                return RedirectToAction("Success");
            }
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            return View("NewServiceRequest_Document", currentTasksListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> NewServiceRequest_Video(ViewModel.DocumentTranslationRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate a unique filename for the uploaded document
                string uniqueFileName = null;
                if (model.UploadedDocument != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Videos");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedDocument.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedDocument.CopyToAsync(stream);
                    }
                }
                Entities.Tasks task = new Entities.Tasks();
                task.TaskTitle= model.Title;
                task.TaskRequesterDescription= model.TaskDescription;
                task.TaskDeadlineDateTime= model.Deadline;
                task.TaskRequesterAttachmentFilePath= uniqueFileName;
                task.TaskRequesterAccountId= GetCurrentUser().AccountId;
                task.TaskRequesterAttachmentsUrl= model.DocumentUrl;
                var a = new RacoonProvider.TN_DB_Tasks().new_WrittenTranslation_VideoTask(task);


                return RedirectToAction("Success");
            }
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            return View("NewServiceRequest_Document", currentTasksListViewModel);
        }


    }
}
