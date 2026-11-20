using E_Commerce_Website.DTO;
using E_Commerce_Website.Fitler;
using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace E_Commerce_Website.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _environment;
        private MyDbContext _context;
        public HomeController(MyDbContext db, IWebHostEnvironment environment)
        {
            _context = db;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Categories = await _context.tbl_category.ToListAsync(),
                Products = await _context.tbl_product.Take(8).ToListAsync()
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var customerIdStr = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            Guid customerId = Guid.Parse(customerIdStr);
            var customer = await _context.tbl_customer.FindAsync(customerId);
            if (customer == null) return NotFound();

            CustomerDto customerDto = new CustomerDto()
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail,
                CustomerHashPassword = customer.CustomerPassword, 
                CustomerGender = customer.CustomerGender,
                CustomerCity = customer.CustomerCity,
                CustomerAddress = customer.CustomerAddress,
                CustomerRole = customer.CustomerRole,
                CustTempImage = customer.CustomerImage ?? "default.png"
            };

            return View(customerDto);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(CustomerDto customerDto)
        {
            // Agar password required hai DTO mein lekin profile update karte waqt user password change nahi kar raha, 
            // toh aap ModelState se password ki validation hata sakte hain taaki error na aaye:
            ModelState.Remove("CustomerHashPassword");
            ModelState.Remove("CustomerIFormImage");

            if (!ModelState.IsValid)
            {
                return View(customerDto);
            }

            var existingCustomer = await _context.tbl_customer.FindAsync(customerDto.CustomerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            string filename = customerDto.CustTempImage; // Agar nayi image nahi di toh purani wali hi rahegi

            if (customerDto.CustomerIFormImage != null && customerDto.CustomerIFormImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Upload");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                filename = Guid.NewGuid().ToString() + Path.GetExtension(customerDto.CustomerIFormImage.FileName);
                var filePath = Path.Combine(uploadsFolder, filename);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await customerDto.CustomerIFormImage.CopyToAsync(stream);
                }
            }

            // Database entity update karna
            existingCustomer.CustomerName = customerDto.CustomerName;
            existingCustomer.CustomerEmail = customerDto.CustomerEmail;
            existingCustomer.CustomerGender = customerDto.CustomerGender;
            existingCustomer.CustomerCity = customerDto.CustomerCity;
            existingCustomer.CustomerAddress = customerDto.CustomerAddress;
            existingCustomer.CustomerImage = filename;

            _context.tbl_customer.Update(existingCustomer);
            await _context.SaveChangesAsync();

            // Session name aur image ko bhi foran update kar dein taaki navbar mein foran change nazar aaye
            HttpContext.Session.SetString("CustomerName", existingCustomer.CustomerName);
            HttpContext.Session.SetString("CustomerImage", filename);

            TempData["Login"] = "Profile updated successfully!";
            return RedirectToAction("Index", "Home");
        }

        public async Task <IActionResult> ProductDetails(Guid id)
        {
          var product =  _context.tbl_product.Include(p=> p.Category).FirstOrDefault(x=> x.ProductId == id);

            if(product== null)
            {
                return NotFound();
            }
           var realtedProduct = await _context.tbl_product.Where(p => p.Cat_Id == product.Cat_Id && p.ProductId != id).Take(4).ToListAsync();
            ViewBag.RelatedProducts = realtedProduct;
            return View(product);
        }

        // 1. Navbar aur Footer ke "Categories" link ke liye (saari categories dikhayega)
        public async Task<IActionResult> Category()
        {
            var categories = await _context.tbl_category.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> CategoryProducts(Guid id)
        {
            var category = await _context.tbl_category.FindAsync(id);
            if (category == null) return NotFound();

            var products = await _context.tbl_product.Where(p => p.Cat_Id == id).ToListAsync();

            ViewBag.Categories = await _context.tbl_category.ToListAsync();
            ViewBag.SelectedCategoryId = id;
            ViewBag.CategoryName = category.CategoryName;

            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Products(string? Search, Guid? categoryId)
        {
            var query = _context.tbl_product
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(p => p.ProductName.Contains(Search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.Cat_Id == categoryId.Value);
            }

            var products = await query.ToListAsync();
            return View(products);
        }
    }
}