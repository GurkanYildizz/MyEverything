using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyEverything.ThisMvc.Controllers
{
    [Authorize]
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        
        [HttpGet]
        public IActionResult AddProject()
        {


            return Ok("Şu an yetkin var adminnn");
        }

    }
}
