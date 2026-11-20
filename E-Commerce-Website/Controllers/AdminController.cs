using E_Commerce_Website.DTO;
using E_Commerce_Website.Fitler;
using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce_Website.Controllers
{

    [AuthorizationFilter("Admin")]
    [NoCache]
    public class AdminController : Controller
    {
        private readonly MyDbContext myDbContext;
        private readonly IWebHostEnvironment environment;
        public AdminController(MyDbContext db, IWebHostEnvironment environment)
        {
            myDbContext = db;
            this.environment = environment;
        }
        public IActionResult AdminPage()
        {
            return View();
        }
        public IActionResult ManageAdmins()
        {
           var AdminList = myDbContext.tbl_customer.Where(x => x.CustomerRole == "Admin").ToList();
            
            return View(AdminList);
        }
        public IActionResult ManageCustomers()
        {
            var CustomerList = myDbContext.tbl_customer.Where(x => x.CustomerRole == "Customer").ToList();
            return View(CustomerList);
        }
        [HttpGet]
        public IActionResult DeleteCustomer(Guid id)
        {
          var cusDel=  myDbContext.tbl_customer.FirstOrDefault(x => x.CustomerId == id);
            if (cusDel !=null)
            {
                if(!string.IsNullOrEmpty(cusDel.CustomerImage))
                {
                   var urldele= Path.Combine(environment.WebRootPath, "Upload", cusDel.CustomerImage);
                    if(System.IO.File.Exists(urldele))
                    {
                        System.IO.File.Delete(urldele);
                    }
                }
                myDbContext.tbl_customer.Remove(cusDel);
                myDbContext.SaveChanges();
            }
                return RedirectToAction("ManageCustomers", "Admin");

        }
        [HttpGet]
        public IActionResult EditCustomer(Guid id)
        {
            var editData = myDbContext.tbl_customer.FirstOrDefault(x => x.CustomerId == id);

            if (editData == null)
            {
                return RedirectToAction("ManageCustomers", "Admin");
            }

            CustomerDto customerDto = new CustomerDto()
            {
                CustomerId = editData.CustomerId,
                CustomerName = editData.CustomerName,
                CustomerEmail = editData.CustomerEmail,
                CustomerAddress = editData.CustomerAddress,
                CustomerCity = editData.CustomerCity,
                CustomerGender = editData.CustomerGender,
                CustomerRole = editData.CustomerRole,
                CustTempImage = editData.CustomerImage
            };

            return View(customerDto);
        }
        [HttpPost]
        public IActionResult EditCustomer(CustomerDto Dto)
        {
            var edit = myDbContext.tbl_customer.FirstOrDefault(x => x.CustomerId == Dto.CustomerId);

            if (edit != null)
            {
                edit.CustomerName = Dto.CustomerName;
                edit.CustomerEmail = Dto.CustomerEmail;
                edit.CustomerGender = Dto.CustomerGender;
                edit.CustomerCity = Dto.CustomerCity;
                edit.CustomerAddress = Dto.CustomerAddress;
                edit.CustomerRole = Dto.CustomerRole;

                if (Dto.CustomerIFormImage != null)
                {
                    var oldPath = Path.Combine(environment.WebRootPath, "Upload", edit.CustomerImage);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    var folder = Path.Combine(environment.WebRootPath, "Upload");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var imgName = Guid.NewGuid().ToString() + Path.GetExtension(Dto.CustomerIFormImage.FileName);
                    var fullPath = Path.Combine(folder, imgName);

                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        Dto.CustomerIFormImage.CopyTo(stream);
                    }

                    edit.CustomerImage = imgName;
                }

                myDbContext.SaveChanges();
                return RedirectToAction(nameof(ManageCustomers));
            }

            return View(Dto);
        }

        public IActionResult DeleteAdmin(Guid id)
        {
          var delete =  myDbContext.tbl_customer.FirstOrDefault(x => x.CustomerId == id);
            if(delete != null)
            {
                if(delete.CustomerImage != null)
                {
                   var deleted = Path.Combine(environment.WebRootPath, "Upload");
                    if (System.IO.File.Exists(deleted))
                    {
                        System.IO.File.Delete(deleted);
                    }
                    myDbContext.tbl_customer.Remove(delete);
                    myDbContext.SaveChanges();
                }
                return RedirectToAction(nameof(ManageAdmins));
            }
            return View();
        }

        [HttpGet]

        public IActionResult EditAdmin(Guid id)
        {
         var editadmin =   myDbContext.tbl_customer.FirstOrDefault(x => x.CustomerId == id);
            if (editadmin == null)
            {
                return RedirectToAction("ManageAdmin", "Admin");
            }
            CustomerDto customer = new CustomerDto()
            {
                CustomerId = editadmin.CustomerId,
                CustomerName = editadmin.CustomerName,
                CustomerEmail = editadmin.CustomerEmail,
                CustomerAddress = editadmin.CustomerAddress,
                CustomerCity = editadmin.CustomerCity,
                CustomerGender = editadmin.CustomerGender,
                CustomerRole = editadmin.CustomerRole,
                CustTempImage = editadmin.CustomerImage
            };

            return View(customer);
        }
        [HttpPost]
        public IActionResult EditAdmin(CustomerDto customerDto)
        {
           var edit = myDbContext.tbl_customer.FirstOrDefault(x => x.CustomerId == customerDto.CustomerId);
            if(edit != null)
            {
                edit.CustomerName = customerDto.CustomerName;
                edit.CustomerEmail = customerDto.CustomerEmail;
                edit.CustomerGender = customerDto.CustomerGender;
                edit.CustomerCity = customerDto.CustomerCity;
                edit.CustomerAddress = customerDto.CustomerAddress;
                edit.CustomerRole = customerDto.CustomerRole;

            }
            if(customerDto.CustomerIFormImage  != null)
            {
               var deleted= Path.Combine(environment.WebRootPath, "Upload", edit.CustomerImage);
                if(System.IO.File.Exists(deleted))
                {
                    System.IO.File.Delete(deleted);
                }

             var firstedit =   Path.Combine(environment.WebRootPath, "Upload");
                if (!Directory.Exists(firstedit))
                {
                    Directory.CreateDirectory(firstedit);
                }
              var url =  Guid.NewGuid().ToString() + Path.GetExtension(customerDto.CustomerIFormImage.FileName);
           var combinePath =     Path.Combine(firstedit, url);
                using(FileStream stream = new FileStream(combinePath, FileMode.Create))
                {
                    customerDto.CustomerIFormImage.CopyTo(stream);
                    edit.CustomerImage = url;
                }
                myDbContext.SaveChanges();
                return RedirectToAction(nameof(ManageAdmins));
            }

            return View(customerDto);
        }

        public IActionResult FetchCategory()
        {
          var categories=  myDbContext.tbl_category.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryDto categoryDto)
        {
            if (categoryDto.CategoryImageFile == null)
            {
                ModelState.AddModelError("CategoryImageFile", "Category Image is Required.");
            }

            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            string filename = "";
            if (categoryDto.CategoryImageFile != null)
            {
                var firstpath = Path.Combine(environment.WebRootPath, "CategoryImages");
                if (!Directory.Exists(firstpath))
                {
                    Directory.CreateDirectory(firstpath);
                }
                filename = Guid.NewGuid().ToString() + Path.GetExtension(categoryDto.CategoryImageFile.FileName);
                var combine = Path.Combine(firstpath, filename);

                using (FileStream stream = new FileStream(combine, FileMode.Create))
                {
                    categoryDto.CategoryImageFile.CopyTo(stream);
                }

                Category category = new Category()
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = categoryDto.CategoryName,
                    CategoryImage = filename
                };

                myDbContext.tbl_category.Add(category);
                myDbContext.SaveChanges();
                return RedirectToAction(nameof(FetchCategory));
            }

            return View(categoryDto);
        }
        [HttpGet]
        public IActionResult UpdateCategory(Guid id)
        {
            var update = myDbContext.tbl_category.FirstOrDefault(x => x.CategoryId == id);
            if (update == null)
            {
                return NotFound();
            }
            CategoryDto categoryDto = new CategoryDto()
            {
                CategoryId = update.CategoryId,
                CategoryName = update.CategoryName,
                TempImage = update.CategoryImage
            };
            return View(categoryDto);
        }

        [HttpPost]
        public IActionResult UpdateCategory(CategoryDto categoryDto)
        {
            var existingCategory = myDbContext.tbl_category.FirstOrDefault(x => x.CategoryId == categoryDto.CategoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                categoryDto.TempImage = existingCategory.CategoryImage;
                return View(categoryDto);
            }

            string filename = existingCategory.CategoryImage;

            if (categoryDto.CategoryImageFile != null)
            {
                var firstpath = Path.Combine(environment.WebRootPath, "CategoryImages");
                if (!Directory.Exists(firstpath))
                {
                    Directory.CreateDirectory(firstpath);
                }

                filename = Guid.NewGuid().ToString() + Path.GetExtension(categoryDto.CategoryImageFile.FileName);
                var combine = Path.Combine(firstpath, filename);

                using (FileStream stream = new FileStream(combine, FileMode.Create))
                {
                    categoryDto.CategoryImageFile.CopyTo(stream);
                }
            }

            existingCategory.CategoryName = categoryDto.CategoryName;
            existingCategory.CategoryImage = filename;

            myDbContext.tbl_category.Update(existingCategory);
            myDbContext.SaveChanges();

            return RedirectToAction(nameof(FetchCategory));
        }
        public IActionResult DeleteCategory(Guid id)
        {
            var delete = myDbContext.tbl_category.FirstOrDefault(x => x.CategoryId == id);
            if (delete != null)
            {
                if (!string.IsNullOrEmpty(delete.CategoryImage))
                {
                    var imagePath = Path.Combine(environment.WebRootPath, "CategoryImages", delete.CategoryImage);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                myDbContext.Remove(delete);
                myDbContext.SaveChanges();
                return RedirectToAction(nameof(FetchCategory));
            }
            return RedirectToAction(nameof(FetchCategory));
        }
        public IActionResult FetchProduct()
        {
          var products =  myDbContext.tbl_product.ToList();
            return View(products); 
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.Categories = new SelectList(myDbContext.tbl_category, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(ProductDto productDto)
        {
            if(productDto.ProductIFormImage == null)
            {
                ModelState.AddModelError("ProductIFormImage", "Product Image is Requried.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(myDbContext.tbl_category, "CategoryId", "CategoryName");
                return View(productDto);

            }
                string filename = "";
            if(productDto.ProductIFormImage != null)
            {
               var firstpath =  Path.Combine(environment.WebRootPath, "Images", filename);
                if (!Directory.Exists(firstpath))
                {
                    Directory.CreateDirectory(firstpath);
                }
             filename =   Guid.NewGuid().ToString() + Path.GetExtension(productDto.ProductIFormImage.FileName);
             var combine =   Path.Combine(firstpath, filename);

                using(FileStream stream = new FileStream(combine,FileMode.Create))
                {
                    productDto.ProductIFormImage.CopyTo(stream);
                }

                Product product = new Product()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = productDto.ProductName,
                    ProductPrice = productDto.ProductPrice,
                    ProductDescription = productDto.ProductDescription,
                    ProductImage = filename,
                    Cat_Id = productDto.Cat_Id,

                };
                myDbContext.tbl_product.Add(product);
                myDbContext.SaveChanges();
                return RedirectToAction("FetchProduct");
            }
            return View(productDto);
        }

        [HttpGet]
        public IActionResult UpdateProduct(Guid id)
        {
         var updateProduct=   myDbContext.tbl_product.FirstOrDefault(x=> x.ProductId == id);
            ViewBag.Categories = new SelectList(myDbContext.tbl_category, "CategoryId", "CategoryName", updateProduct.Cat_Id);
            ProductDto productDto = new ProductDto()
            {
                ProductId = updateProduct.ProductId,
                ProductName = updateProduct.ProductName,
                ProductPrice =  updateProduct.ProductPrice,
                ProductDescription = updateProduct.ProductDescription,
                ProductTempImage = updateProduct.ProductImage,
                    Cat_Id = updateProduct.Cat_Id


            };
            return View(productDto);
        }
        [HttpPost]
        public IActionResult UpdateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(myDbContext.tbl_category, "CategoryId", "CategoryName", productDto.Cat_Id);
                return View(productDto);
            }

            var existingProduct = myDbContext.tbl_product.FirstOrDefault(x => x.ProductId == productDto.ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            string filename = existingProduct.ProductImage;

            if (productDto.ProductIFormImage != null)
            {
                var firstpath = Path.Combine(environment.WebRootPath, "Images");
                if (!Directory.Exists(firstpath))
                {
                    Directory.CreateDirectory(firstpath);
                }

                filename = Guid.NewGuid().ToString() + Path.GetExtension(productDto.ProductIFormImage.FileName);
                var combine = Path.Combine(firstpath, filename);

                using (FileStream stream = new FileStream(combine, FileMode.Create))
                {
                    productDto.ProductIFormImage.CopyTo(stream);
                }
            }

            existingProduct.ProductName = productDto.ProductName;
            existingProduct.ProductPrice = productDto.ProductPrice;
            existingProduct.ProductDescription = productDto.ProductDescription;
            existingProduct.ProductImage = filename;
            existingProduct.Cat_Id = productDto.Cat_Id;

            myDbContext.tbl_product.Update(existingProduct);
            myDbContext.SaveChanges();

            return RedirectToAction("FetchProduct");
        }
        public IActionResult DetailProduct(Guid id)
        {
            var details = myDbContext.tbl_product.Include(x => x.Category).FirstOrDefault(x => x.ProductId == id); 
            if (details == null)
            {
                return NotFound();
            }


            return View(details);
        }
    }
    }
    


