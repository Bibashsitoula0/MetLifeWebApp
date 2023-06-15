using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MetLifeInsurance.Repository.RegisterRepository.AccountRepository;
using MetLifeInsurance.Models;
using System.Security.Claims;
using OfficeOpenXml.FormulaParsing.Excel.Functions;

namespace MetLifeInsurance.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRegisterRepository _registerRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _httpClient;
        private string apiEndpoint = "http://10.68.15.14:9999/api/values";

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger, IRegisterRepository registerRepository,
            UserManager<IdentityUser> userManager, HttpClient httpClient)
        {
            _httpClient=httpClient;
            _registerRepository = registerRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _httpClient = httpClient;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        
      /*  private async Task<bool> ADlogin(string username,string currentpassword)
        {
            using (HttpClient client = new HttpClient())
            {
                  var formData = new MultipartFormDataContent();
                  formData.Add(new StringContent(username), "UserID");
                  formData.Add(new StringContent(currentpassword), "CurrentPassword");
                  HttpResponseMessage response = await client.PostAsync(apiEndpoint, formData);              

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
        }*/

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
               /* bool isadlogin = await ADlogin(Input.Email, Input.Password);
                if (isadlogin == true)
                {*/
                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var checkIsActive = await _registerRepository.getUserByEmail(Input.Email);
                        var status = checkIsActive[0].is_active;
                        if (status)
                        {
                            returnUrl = "/Dashboard";
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User is currently inactive.Plz contact administrative");
                            return Page();
                        }
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid email or password");

               /* }
                else
                {
                    ModelState.AddModelError(string.Empty, "User doesnot exist in AD");
                }*/
            }
            return Page();
        }
    }
}
