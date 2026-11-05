using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Commerce_Website.Fitler
{
    public class AuthorizationFilter:Attribute,IAuthorizationFilter
    {
        private string Role;
        public AuthorizationFilter(string role)
        {
            Role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

          var Roles =  context.HttpContext.Session.GetString("Role");


            if (string.IsNullOrEmpty(Roles))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            if (Roles == null || Roles != Role)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }
    }
}
