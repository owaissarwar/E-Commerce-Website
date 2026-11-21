using E_Commerce_Website.DTO;
using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ToListAsync, FirstOrDefaultAsync ke liye lazmi include karein

namespace E_Commerce_Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext Context;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public AccountController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            Context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            var role = HttpContext.Session.GetString("Role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin")
                {
                    return RedirectToAction("AdminPage", "Admin");
                }
                else if (role == "Customer")
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(CustomerDto Customerdto)
        {
            if (string.IsNullOrEmpty(Customerdto.CustomerRole))
            {
                Customerdto.CustomerRole = "Customer";
            }

            if (!ModelState.IsValid)
            {
                return View(Customerdto);
            }

            string imageName = "default.png";
            if (Customerdto.CustomerIFormImage != null)
            {
                var firstpath = Path.Combine(webHostEnvironment.WebRootPath, "Upload");
                if (!Directory.Exists(firstpath))
                {
                    Directory.CreateDirectory(firstpath);
                }

                imageName = Guid.NewGuid().ToString() + Path.GetExtension(Customerdto.CustomerIFormImage.FileName);
                var combinepath = Path.Combine(firstpath, imageName);

                using (FileStream stream = new FileStream(combinepath, FileMode.Create))
                {
                    await Customerdto.CustomerIFormImage.CopyToAsync(stream);
                }
            }

            Customer customer = new Customer()
            {
                CustomerId = Guid.NewGuid(),
                CustomerName = Customerdto.CustomerName,
                CustomerEmail = Customerdto.CustomerEmail,
                CustomerPassword = BCrypt.Net.BCrypt.HashPassword(Customerdto.CustomerHashPassword),
                CustomerAddress = Customerdto.CustomerAddress,
                CustomerCity = Customerdto.CustomerCity,
                CustomerGender = Customerdto.CustomerGender,
                CustomerRole = Customerdto.CustomerRole,
                CustomerImage = imageName
            };

            await Context.tbl_customer.AddAsync(customer);
            await Context.SaveChangesAsync();

            TempData["Signup"] = "Signup Successfully! Please login now.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            var role = HttpContext.Session.GetString("Role");
            
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin")
                {
                    return RedirectToAction("AdminPage", "Admin");
                }
                else if (role == "Customer")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string CustomerEmail, string CustomerHashPassword)
        {
            var customer = await Context.tbl_customer
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CustomerEmail == CustomerEmail);

            if (customer != null && BCrypt.Net.BCrypt.Verify(CustomerHashPassword, customer.CustomerPassword))
            {
                HttpContext.Session.SetString("CustomerName", customer.CustomerName);
                HttpContext.Session.SetString("CustomerId", customer.CustomerId.ToString());
                HttpContext.Session.SetString("Role", customer.CustomerRole);
                HttpContext.Session.SetString("CustomerImage", customer.CustomerImage ?? "default.png");

                TempData["Login"] = "Login Successfully!";

                if (string.Equals(customer.CustomerRole?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("AdminPage", "Admin");
                }
                else if (string.Equals(customer.CustomerRole?.Trim(), "Customer", StringComparison.OrdinalIgnoreCase))
                {
                    var pendingProductId = HttpContext.Session.GetString("PendingProductId");
                    if (!string.IsNullOrEmpty(pendingProductId))
                    {
                        HttpContext.Session.Remove("PendingProductId");
                        return RedirectToAction("Checkout", "Product", new { id = Guid.Parse(pendingProductId) });
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Email or Password!");
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}