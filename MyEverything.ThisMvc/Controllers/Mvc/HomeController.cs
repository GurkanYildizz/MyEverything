
using Microsoft.AspNetCore.Mvc;



namespace MyEverything.ThisMvc.Controllers.Mvc;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }

    
}