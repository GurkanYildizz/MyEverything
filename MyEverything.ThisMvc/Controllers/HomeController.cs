
using Microsoft.AspNetCore.Mvc;



namespace MyEverything.ThisMvc.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }

    
}