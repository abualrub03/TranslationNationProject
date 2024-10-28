using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Entities;
using ViewModel.UserViewModels;
using Microsoft.EntityFrameworkCore;
using RacoonProvider;
using ViewModel.AdminViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentFormat.OpenXml.Wordprocessing;
using TranslationNation.Web.Models;

namespace TranslationNation.Controllers
{
    public class CustomerController : BaseController
    {
        
        // <<< sendContactRequest  11
        [HttpPost]
        public IActionResult sendContactRequest(Entities.Contact con)
        {
            new RacoonProvider.Contact().newContactRequest(con);
            return contact();
        }
      
        

        // sendContactRequest >>>
        // <<< newsNewSubscribtion
        [HttpPost]
        public IActionResult newsNewSubscribtion(Entities.News news)
        {
            new RacoonProvider.News().newSubscribtion(news);
            return Index();
        }
        // newsNewSubscribtion >>>
        // <<< loginValidation First step
        [HttpPost]
        public async Task<IActionResult> loginValidation(ViewModel.UserViewModels.loginViewModel acc)
            {
            if (ModelState.IsValid)
            {
                var data = new RacoonProvider.Team().getTeamMemberByInfo(acc.Email, acc.Password);
                if (data == null)
                {
                    ModelState.AddModelError("FormValidation", "Wrong Username or Password");
                    return View("login", acc);
                }
                var claims = new List<Claim>
                     {
                        
                         new Claim("CustomerID", data.Id.ToString() ,ClaimValueTypes.Integer32),
                         new Claim("CustomerEmail", data.Email,ClaimValueTypes.Email),
                         new Claim(ClaimTypes.Role, data.Role,ClaimValueTypes.String ),
                         new Claim(ClaimTypes.Role, data.Name,ClaimValueTypes.String )
                     };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            else
                return View("login", acc);


            var v = new ViewMoreViewModel() { };
            

            return RedirectToAction("dashboard", "Admin", v);
        }
        // loginValidation >>>
        // <<< login View 
        public IActionResult validation()
        {
            return View("validation");
        }
        public IActionResult AboutUs()
        {
            return View("AboutUs");
        }
         public ActionResult PrivacyPolicy()
            {
                return View("PrivacyPolicy");
            }
         public ActionResult ContactUs()
        {
            return View("ContactUs");
        }
         public ActionResult TermsOfService()
        {
            return View("TermsOfService");
        }
        // login view >>>
        
        [Route("")]
        [Route("/Customer/Index")]
        public ActionResult Index()
        {
            return View("Index");
        }
        // index >>>

        public IActionResult SignUp()
        {
            return View("SignUp");
        }public IActionResult SignIn()
        {
            return View("SignIn");
        }
        public IActionResult Services()
        {
            return View("Services");
        }
        // <<< service
        public ActionResult service()
        {
            var obj = new RacoonProvider.Services();
            List<Entities.Service> entityList = obj.getAllServices();
            return View(entityList);
        }
        // service >>>
        // <<< team 
        public IActionResult team()
        {

            var obj = new RacoonProvider.Team();
            List<Entities.Team> entityList = obj.getAllTeamMembers();
            return View(entityList);
        }
        // team >>>
        // <<< about
        public IActionResult about()
        {
            return View();
        }
        // about >>
        // << contact
        [HttpPost]
        [HttpGet]
        public IActionResult contact()
        {
            var obj = new RacoonProvider.Services();
            List<Entities.Service> entityList = obj.getAllServices();
            return View("_contact", entityList);
        }
        // contact >>
        [HttpGet]
        public IActionResult GetImage(int id)
        {
            using (var con = new RacoonProvider.Team())
            {
                Entities.Team tem = new Entities.Team();
                tem = con.getAMember(id).FirstOrDefault();
                HttpContext.Response.Headers.Add("Content-Type", tem.ImageContentType);
                return File(tem.ImageData, tem.ImageContentType);
            }
        }
            
        ///////////////TRANSLATION
        

        [HttpPost]
        public async Task<IActionResult> SignUpRequestAsCustomerAsync(string FullName, string Email, string BirthDate, string Password, string ConfirmPassword, bool AcceptTerms)
         {
            if (ModelState.IsValid)
            {

                var data = new RacoonProvider.TN_DB_Accounts().newSignUpClientRequest(FullName, Email, BirthDate, Password, AcceptTerms);
                if (data == null)
                {
                    ModelState.AddModelError("FormValidation", "Wrong Username or Password");
                    return View("SignIn");
                }
                var claims = new List<Claim>
                     {
                         new Claim("AccountId", data.AccountId.ToString() ,ClaimValueTypes.Integer32),
                         new Claim("EmailAddress", data.EmailAddress,ClaimValueTypes.Email),
                         new Claim(ClaimTypes.Role, data.TheRole,ClaimValueTypes.String ),
                         new Claim("FirstName", (data.FirstName),ClaimValueTypes.String ),
                         new Claim("LastName", (data.LastName),ClaimValueTypes.String ),
                         new Claim("University", (data.University.ToString()),ClaimValueTypes.String )
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            else
                return View("SignUp");

            return RedirectToAction("SignIn", "Customer");
        }

        [HttpPost]
        public async Task<IActionResult> SignInRequest(string Email, string Password)
        {
            Accounts data = new Accounts();
            if (ModelState.IsValid)
            {
                data = new RacoonProvider.TN_DB_Accounts().newSignInRequest(Email, Password);
                if (data == null)
                {
                    ModelState.AddModelError("FormValidation", "Wrong Username or Password");
                    return View("SignIn");
                }
                if(data.IsActive == "No")
                {
                    return View("validation");
                }
                var claims = new List<Claim>
                     {
                         new Claim("AccountId", data.AccountId.ToString() ,ClaimValueTypes.Integer32),
                         new Claim("EmailAddress", data.EmailAddress,ClaimValueTypes.Email),
                         new Claim(ClaimTypes.Role, data.TheRole,ClaimValueTypes.String ),
                         new Claim("FirstName", (data.FirstName),ClaimValueTypes.String ),
                         new Claim("LastName", (data.LastName),ClaimValueTypes.String ),
                                                  new Claim("University", (data.University.ToString()),ClaimValueTypes.String )

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            else
                return View("SignIn");

            if (data.TheRole == "Client")
            {
                return RedirectToAction("ClientIndex", "Client");

            }
            else if (data.TheRole == "Translator")
            {
                return RedirectToAction("TranslatorIndex", "Translator");

            }
            else if (data.TheRole == "Supervisor")
            {
                return RedirectToAction("SupervisorIndex", "Supervisor");
            }
            else {
                return View("SignIn");

            }


        }




        [HttpPost]
        public async Task<IActionResult> SignUpRequestAsTranslatorAsync(string FullName, string Email, int Location, int University, string Major, string Faculty, string MajorYear, string GPA,string PhoneNumber,string KnowAboutTN, string Password, string ConfirmPassword, bool AcceptTerms , [FromServices] EmailService emailService)
        {
            if (ModelState.IsValid) 
            {
                var data = new RacoonProvider.TN_DB_Accounts().newSignUpTranslatorRequest(FullName, Email, Password, Location, University , Major , Faculty , MajorYear , GPA , KnowAboutTN , PhoneNumber);
                if (data == null)
                {
                    ModelState.AddModelError("FormValidation", "Wrong Username or Password");
                    return View("SignIn");
                }

                // Email the user after a successful signup
                emailService.SendEmail(Email, "Welcome to Our Platform", $"Hi {FullName}, thanks for signing up!" , 1);

                var claims = new List<Claim>
                {
                    new Claim("AccountId", data.AccountId.ToString(), ClaimValueTypes.Integer32),
                    new Claim("EmailAddress", data.EmailAddress, ClaimValueTypes.Email),
                    new Claim(ClaimTypes.Role, data.TheRole, ClaimValueTypes.String),
                    new Claim("FirstName", data.FirstName, ClaimValueTypes.String),
                    new Claim("LastName", data.LastName, ClaimValueTypes.String),
                                             new Claim("University", (data.University.ToString()),ClaimValueTypes.String )

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            else
            {
                return View("SignUp");
            }


            return await SignInRequest(Email, Password);
        }



        [HttpPost]
        public async Task<IActionResult> SignUpRequestAsSupervisorAsync(string FullName, string Email, int Location,
        string PhoneNumber, string Username, int University, string Password, string ConfirmPassword, bool AcceptTerms)
        {
                var data = new RacoonProvider.TN_DB_Accounts().newSignUpSupervisorRequest(FullName, Email, Location, PhoneNumber, Username, University, Password, AcceptTerms);
                if (data == null)
                {
                    ModelState.AddModelError("FormValidation", "Sign-up failed. Please check your input.");
                    return View("SignUp");
                }
                var claims = new List<Claim>
                     {
                         new Claim("AccountId", data.AccountId.ToString() ,ClaimValueTypes.Integer32),
                         new Claim("EmailAddress", data.EmailAddress,ClaimValueTypes.Email),
                         new Claim(ClaimTypes.Role, data.TheRole,ClaimValueTypes.String ),
                         new Claim("FirstName", (data.FirstName),ClaimValueTypes.String ),
                         new Claim("LastName", (data.LastName),ClaimValueTypes.String ),
                                                  new Claim("University", (data.University.ToString()),ClaimValueTypes.String )

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("SignIn", "Customer");
        }




        public List<Country> GetCountries()
        {
           
            return new RacoonProvider.TN_DB_Countries().GetAllCountries();
        }

        [HttpGet]
        public List<University> GetUniveritiesBasedOnId( int id)
        {
           

            return new RacoonProvider.TN_DB_University().UniversityBasedOnCountryId(id);
        }

    }
}
