using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using MetLifeInsurance.Models;
using Microsoft.EntityFrameworkCore.Internal;
using MetLifeInsurance.Repository.RegisterRepository.AccountRepository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MetLifeInsurance.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRegisterRepository _registerRepository;      
        public readonly IConfiguration _configuration;



        public AccountController(HttpClient httpClient, IRegisterRepository registerRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _registerRepository = registerRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration= configuration;
        }

        private async Task<bool> ADlogin(string username, string currentpassword)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiendpoint = _configuration["ActiveDirectoryApi:apiEnd"].ToString();
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(username), "UserID");
                formData.Add(new StringContent(currentpassword), "CurrentPassword");
                HttpResponseMessage response = await client.PostAsync(apiendpoint, formData);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var data = responseBody;
                    bool apiResponse = bool.Parse(data);
                    return apiResponse;
                }
                else
                {
                    return false;

                }
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [Route("SubmitLogin")]
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            // bool isadlogin = await ADlogin(login.UserName, login.Password);         

            bool isadlogin = true;
                if (isadlogin == true)
                {
                    var getuser = await _registerRepository.getUser(login.UserName);
                    var user = getuser.FirstOrDefault();
                    HttpContext.Session.Remove("UserName");
                    HttpContext.Session.Remove("UserId");
                    HttpContext.Session.Remove("Role");
                    if (user != null)
                    {
                    if (user.is_active == true)
                    {
                        var session = _httpContextAccessor.HttpContext.Session;
                        session.SetString("UserName", user.UserName);
                        session.SetString("UserId", user.Id.ToString());
                        session.SetString("Role", user.Role);
                    }
                    else
                    {
                        ViewBag.UserActiveMessage = "User doesnot active.Plz contact Administration";
                        return View();
                    }                   
                    }
                    else
                    {
                        ViewBag.UserMessage = "User doesnot exists.Plz contact Administration";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "User doesnot exists in AD";
                    return View();
                }
         
        


            return Redirect("/Dashboard");
        }


        [HttpPost]
        public bool CheckAd([FromForm]TestAd testAd)
        {           
            return true;
        }


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Role");
            return Redirect("/Account/Login") ;
        }

    }
}
