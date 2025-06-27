using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers;


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
            var data = await everythingDbContext.ProjectsInfo.ToListAsync();//Burada tüm verilerin hepsi değil de parça parça gözükecek... Hatta sadece öne çıkanlar gözükecek...

            return View(data);
        }


     
       

        public async Task<IActionResult> Details(Guid id, string slug)
        {
            
            if (slug == null)
            {
                //return Error 404 ...
            }

            var selectedData = await everythingDbContext.ProjectsInfo.FirstOrDefaultAsync(f => f.Id == id);
            


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
                .Replace(".", "-")
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
