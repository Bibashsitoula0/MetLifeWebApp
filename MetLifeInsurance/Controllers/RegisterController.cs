using MetLifeInsurance.Models;
using MetLifeInsurance.Repository;
using MetLifeInsurance.Repository.RegisterRepository.AccountRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using Syncfusion.EJ2.FileManager;
using Syncfusion.EJ2.Gantt;


namespace MetLifeInsurance.Controllers
{
    [AccessValidation(Feature = "userregistration", Role = "ReportViewer")]
    public class RegisterController : Controller
    {
        public readonly IRegisterRepository _registerRepository;
        public readonly IConfiguration _configuration;
     

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public RegisterController(IRegisterRepository registerRepository, IConfiguration configuration)
        {
            _registerRepository = registerRepository;   
            _configuration = configuration; 
           

        }
       
        [Route("UsersList")]
        public async Task<Object> UsersList()
        {
            return View();
        }
              
        [Route("userlistall")]
        public async Task<Object> UserListPartial()
        {
            string[] generalUserRoles = _configuration["Roles:role"].ToString().Split(',');
            ViewBag.GeneralUserRoles = generalUserRoles;
            var users = await _registerRepository.getallusers();
            return PartialView("UserListPartial", users);
        }

                 
        [Route("CreateUser")]
        public async Task<Object> CreateUser()
        {
            string[] generalUserRoles = _configuration["Roles:role"].ToString().Split(',');
            return View(generalUserRoles);
        }

        [Route("SaveUser")]
        [HttpPost]
		public async Task<IActionResult> SaveUser(RegisterUser registerUser)
		{
			var getuser = await _registerRepository.getUser(registerUser.UserName);
			if (getuser.Count == 0)
			{
				ViewBag.CreateMessage = "User Created Sucessfully";
				var data = await _registerRepository.Adduser(registerUser);
				return RedirectToAction("UsersList", "Register");
			}
			else
			{
				ViewBag.Message = "Username already exists";
				string[] generalUserRoles = _configuration["Roles:role"].ToString().Split(',');
				return View("CreateUser", generalUserRoles);
			}
		}

		/*[Route("ChangePassword")]
        [HttpPost]
        public async Task<Object> ChangePassword(string Emails, string Password, string ConfirmPassword)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(Emails);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token,Password);
                }
                 TempData["SuccessMessage"] = "Password changed successfully.";
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return RedirectToAction("UsersList", "Register");
        }*/

        [Route("users/status/change")]
        [HttpPost]
        public async Task<Object> Changestatus(ChangeStatus status)
        {
            

            var users = await _registerRepository.Changestatus(Convert.ToInt32(status.Id),status.Is_Active);
            return RedirectToAction("UsersList", "Register");
        }

        [Route("EditUser")]
        [HttpPost]
        public async Task<Object> EditUser(string UserId,string Username, string Email,string Role)
        {
            var data = await _registerRepository.EditUser(Convert.ToInt32(UserId), Username, Email, Role);
            return RedirectToAction("UsersList", "Register");
        }

    }
}
