using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TranslationNation.Controllers;
using Microsoft.Data.SqlClient;
using System.Data;
using Entities;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Office2010.Excel;

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
            if (currentTasksListViewModel.currentTasksViewModels.FirstOrDefault().TaskResponderAccountId == null)
            {
                currentTasksListViewModel.offers = new RacoonProvider.TN_DB_Tasks().newOffer(currentTasksListViewModel.currentTasksViewModels.FirstOrDefault().TaskId);

            }
            return View("TaskViewDetails", currentTasksListViewModel);
        }   
        [HttpPost]
        public IActionResult ChhooseTranslator(int offerId )
        {
            new RacoonProvider.TN_DB_Tasks().AssignTranslator(offerId);

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
        public async Task<string> NewServiceRequest_Document(ViewModel.DocumentTranslationRequestViewModel model)
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



              
                return "Succsess";

        }

        [HttpPost]
        public async Task<string> NewServiceRequest_Video(ViewModel.DocumentTranslationRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                var a = new RacoonProvider.TN_DB_Tasks().new_WrittenTranslation_VideoTask(task);


                return "Sucess";
            }
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            return "Sucess";
        }

























        [HttpPost]
        public async Task<JsonResult> GetWordCount(IFormFile uploadedDocument)
        {
            if (uploadedDocument != null)
            {
                string uniqueFileName = null;
                int wordCount = 0; // To store the word count

                // Generate a unique filename and store the file
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Documents");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedDocument.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the uploaded file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedDocument.CopyToAsync(stream);
                }

                // Calculate the word count using the helper function
                wordCount = CalculateWordCount(filePath, uploadedDocument.FileName);

                // Return the word count as a JSON result
                return Json(new { wordCount });
            }

            // Return an error response if no file was provided
            return Json(new { error = "No file provided" });
        }

        // Helper function to calculate word count based on file type
        private int CalculateWordCount(string filePath, string fileName)
        {
            int wordCount = 0;

            // Check if the file is a .docx file
            if (Path.GetExtension(fileName).ToLower() == ".docx")
            {
                wordCount = GetWordCountFromDocx(filePath);
            }
            
            return wordCount;
        }

        // Method to count words in a .docx file
        private int GetWordCountFromDocx(string filePath)
        {
            using (var document = WordprocessingDocument.Open(filePath, false))
            {
                var props = document.ExtendedFilePropertiesPart.Properties;
                int.TryParse(props.Words?.Text ?? "0", out int wordCount);
                return wordCount;
            }
        }












        public IActionResult CurrentTasks()
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            currentTasksListViewModel.currentTasksViewModels = new RacoonProvider.TN_DB_Tasks().get_AllTasksOnClientId(GetCurrentUser().AccountId);
           
            return View("CurrentTasks", currentTasksListViewModel);
        }

    }
}
