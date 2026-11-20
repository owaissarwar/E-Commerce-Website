using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Website.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public CategoryMenuViewComponent(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.tbl_category.ToListAsync();
            return View(categories);
        }
    }
}