using E_Commerce_Website.Fitler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Website.Controllers
{

    [AuthorizationFilter("Customer")]
    [NoCache]
    public class UserController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
