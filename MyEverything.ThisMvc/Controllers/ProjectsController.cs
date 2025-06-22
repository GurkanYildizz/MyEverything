using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEverything.ThisMvc.Entities;


namespace MyEverything.ThisMvc.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly EverythingDbContext everythingDbContext;

        public ProjectsController(EverythingDbContext everythingDbContext)
        {
            this.everythingDbContext = everythingDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await everythingDbContext.ProjectsInfo.ToListAsync();

            return View(data);
        }
        
        public async Task<IActionResult> Details(Guid id, string slug)
        {
            
            if (slug == null)
            {
                //return Error 404 ...
            }

            var selectedData = everythingDbContext.ProjectsInfo.FirstOrDefault(f => f.Id == id);
            


            return View(selectedData);

        }
        public string GenerateSlug(string title)
        {
            return title
                .ToLower()
                .Replace("ç", "c")
                .Replace("ş", "s")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ü", "u")
                .Replace("ğ", "g")
                .Replace(" ", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("?", "")
                .Replace("!", "");
        }

        public RedirectToActionResult DetailOfSelected(Guid id,string title)
        {
            
            var slug = GenerateSlug(title);

            return RedirectToAction("Details", new { id = id, slug = slug });
        }
    }
}
