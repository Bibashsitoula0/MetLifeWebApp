using MetLifeInsurance.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MetLifeInsurance.Models
{
    
        public class AccessValidationAttribute : ActionFilterAttribute
        {
            public string Feature { get; set; }
            public string Role { get; set; }

            public override void OnActionExecuting(ActionExecutingContext context)
            {

            string username = context.HttpContext.User.Identity.Name; // Replace with your actual username retrieval logic
                string role = Role;
                string feature = Feature; 
                bool hasAccess = ValidateUser.Validate(username, role, feature);
                if (!hasAccess)
                {
                context.Result = new RedirectToRouteResult(
               new RouteValueDictionary
              {
                    { "controller", "Account" },
                    { "action", "Login" }
            });
            }
            }
        }

}
