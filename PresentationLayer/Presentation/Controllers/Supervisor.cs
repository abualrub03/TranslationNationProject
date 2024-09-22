using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using TranslationNation.Controllers;
using TranslationNation.Web.Models;
using ViewModel;

namespace TranslationNation.Web.Controllers
{
    public class Supervisor : AuthorizedController
    {
        public IActionResult SupervisorIndex()
        {

            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            var t = new RacoonProvider.TN_DB_Tasks().GetAllNotAssignTasks();
            currentTasksListViewModel.currentTasksViewModels = t;
            var test = new RacoonProvider.TN_DB_University().GetUniversityNameAndNumberOfStudents(GetCurrentUser().University);
            currentTasksListViewModel.universityName = test.FirstOrDefault().UniversityName;
            currentTasksListViewModel.universityNumberOfStudents = test.FirstOrDefault().NumberOfStudents;


            currentTasksListViewModel.NotVerifiedUniversityStudent = new RacoonProvider.TN_DB_University().GetVerificationRequests(GetCurrentUser().University); ;
            return View("SupervisorIndex", currentTasksListViewModel);
        }

        [HttpPost]
        public IActionResult InviteEmail(string Email , string Name)
        {
            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            var t = new RacoonProvider.TN_DB_Tasks().GetAllNotAssignTasks();
            currentTasksListViewModel.currentTasksViewModels = t;
            var mail = new EmailService();
            mail.SendEmail(Email, "", $"{Name}", 2);

         
            return View("SupervisorIndex", currentTasksListViewModel);

        }
        [HttpPost]
        public IActionResult Accept(int AccountId )
        {


            var temp = new RacoonProvider.TN_DB_Accounts().VerifyTranslator(AccountId);






            ViewModel.CurrentTasksListViewModel currentTasksListViewModel = new ViewModel.CurrentTasksListViewModel();
            currentTasksListViewModel.accounts = GetCurrentUser();
            var t = new RacoonProvider.TN_DB_Tasks().GetAllNotAssignTasks();
            currentTasksListViewModel.currentTasksViewModels = t;
            var test = new RacoonProvider.TN_DB_University().GetUniversityNameAndNumberOfStudents(GetCurrentUser().University);
            currentTasksListViewModel.universityName = test.FirstOrDefault().UniversityName;
            currentTasksListViewModel.universityNumberOfStudents = test.FirstOrDefault().NumberOfStudents;


            currentTasksListViewModel.NotVerifiedUniversityStudent = new RacoonProvider.TN_DB_University().GetVerificationRequests(GetCurrentUser().University); ;
            var mail = new EmailService();

            mail.SendEmail(temp.EmailAddress, "", $"{temp.FirstName} {temp.SecondName}", 3);



            return View("SupervisorIndex", currentTasksListViewModel);

        }
    }
}   
