using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.Models;

namespace MyEverything.ThisMvc.Controllers
{
    public class ProjectsController : Controller
    {

        public IActionResult Index()
        {
            Datas.ClearDatas();
            AllDatas allDatass = new AllDatas();
            var datas = Datas.GetAllDatas();

            return View(datas);
        }
        public IActionResult Details(Guid id,string slug)
        {

            var data = Datas.GetAllDatas();
            var filterData = data.First(x => x.Id == id);
            var correctSlug = GenerateSlug(filterData.Title);

            if (slug != correctSlug)
            {
                return RedirectToAction("Details", new { id = id, slug = correctSlug });
            }

            return View(model: filterData);

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
    }
}
