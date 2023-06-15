using MetLifeInsurance.Helpers;
using MetLifeInsurance.Models;
using MetLifeInsurance.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PagedList;
using Syncfusion.EJ2.Schedule;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;

namespace MetLifeInsurance.Controllers
{

    [AccessValidation(Feature = "dashboard", Role = "admin,ReportViewer")]
    public class DashboardController : Controller
    {
        public readonly IDashboardRepository _dashboardRepository;
        private readonly ValidateUser _userValidator;

        public DashboardController(IDashboardRepository dashboard, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRepository = dashboard;
            _userValidator = new ValidateUser(httpContextAccessor);

        }

        

        
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [Route("dashboard/home/testing/service")]
        public async Task<Object> GetDashboardPieChartData(DashboardFilter dashboardFilter)
        {

            var dashboard = new DashBoardVm();
            dashboard.teststatus = await _dashboardRepository.gettest(dashboardFilter);
            dashboard.bookingstatus = await _dashboardRepository.getbooking(dashboardFilter);
            dashboard.medicalstatus = await _dashboardRepository.getinstitute(dashboardFilter);
            dashboard.paymentstatus = await _dashboardRepository.getpayment(dashboardFilter);

            return PartialView("_dashboard", dashboard);
        }

        [HttpGet]
        [Route("doctor/dashboard")]
        public async Task<IActionResult> DoctorDashboard()
        {

            return View();
        }




        [HttpPost]
        [Route("doctor/wise/dashboard")]
        public async Task<Object> GetDashboardDoctorPieChartData(DashboardFilter dashboardFilter)
        {

            var dashboard = new DashBoardVm();
            dashboard.bookingstatus = await _dashboardRepository.getdoctorbooking(dashboardFilter);
            dashboard.medicalstatus = await _dashboardRepository.getdoctorinstitute(dashboardFilter);
            dashboard.paymentstatus = await _dashboardRepository.getdoctorpayment(dashboardFilter);
            dashboard.doctorstatus = await _dashboardRepository.getdoctorstatus(dashboardFilter);
            return PartialView("_doctorDashboard", dashboard);
        }

        [HttpGet]
        [Route("tele/doctor/dashboard")]
        public async Task<IActionResult> TeleDashboard()
        {

            return View();
        }

        [HttpPost]
        [Route("teledoctor/wise/dashboard")]
        public async Task<Object> GetDashboardTeleDoctorPieChartData(DashboardFilter dashboardFilter)
        {

            var dashboard = new DashBoardVm();
            dashboard.bookingstatus = await _dashboardRepository.getteledoctorbooking(dashboardFilter);
            dashboard.medicalstatus = await _dashboardRepository.getteledoctorinstitute(dashboardFilter);
            dashboard.paymentstatus = await _dashboardRepository.getteledoctorpayment(dashboardFilter);
            dashboard.doctorstatus = await _dashboardRepository.getteledoctorstatus(dashboardFilter);

            return PartialView("GetDashboardTeleDoctorPieChartData", dashboard);

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public string Role { get; }


        public CustomAuthorizationAttribute(string role)
        {
            Role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            /*string role = HttpContext.Session.GetString("Role");

            if (role != Role)
            {
                context.Result = new ViewResult
                {
                    ViewName = "/Account/Login" // Name of the view to display for unauthorized access
                };
            }*/
        }


    }
    public class ValidateUser
    {
        public static IHttpContextAccessor _httpContextAccessor;
        public ValidateUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">User name in Session</param>
        /// <param name="role">Role for loggged in user in Session</param>
        /// <param name="feature">Functionality where we want to assign user access</param>
        /// <returns></returns>
        public static  bool Validate(string username, string role, string feature)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            // Access session data
            string sessionUsername = httpContext.Session.GetString("UserName");
            string sessionRole = httpContext.Session.GetString("Role");

            if (role !=null)
            {
                switch (feature)
                {
                    case "dashboard":
                        if (sessionRole == "admin" || sessionRole == "ReportViewer")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }


                    case "testing":
                        if (sessionRole == "admin" || sessionRole == "ReportViewer")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                      
                    case "userregistration":
                        if (sessionRole == "admin")
                        {
                           return true;
                        }
                        else
                        {
                            return false;
                        }                       
                        break;
                }
            }            
            return false;
        }
    }
}
