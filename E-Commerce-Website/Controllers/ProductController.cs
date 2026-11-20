using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Website.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext _context;

        public ProductController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(Guid productId)
        {
            var customerId = HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerId))
            {
                TempData["Login"] = "Pehle login karein!";
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Checkout", new { id = productId });
        }
        [HttpGet]
        public async Task<IActionResult> Checkout(Guid id)
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["Login"] = "Please login first to continue.";
                return RedirectToAction("Login", "Account");
            }

            var product = await _context.tbl_product.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}