using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Website.Controllers
{
    public class CartController : Controller
    {
        [HttpGet]
        public IActionResult AddToCart(Guid id)
        {
            var customerId = HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerId))
            {
                TempData["Login"] = "Pehle login karein!";
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Checkout", "Product", new { id = id });
        }
    }
}