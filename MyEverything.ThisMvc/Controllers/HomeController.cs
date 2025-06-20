
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.Models;


namespace MyEverything.ThisMvc.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }

    
}