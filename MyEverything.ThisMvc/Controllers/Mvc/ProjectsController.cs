using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.CQRS.Command;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers;
using MyEverything.ThisMvc.Helpers.CookieGlobalVariablesValues;
using MyEverything.ThisMvc.Helpers.Token;
using System.Net.Http;
using System.Text;


namespace MyEverything.ThisMvc.Controllers.Mvc
{
    public class ProjectsController : Controller
    {
        private readonly EverythingDbContext everythingDbContext;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public ProjectsController(EverythingDbContext everythingDbContext, IConfiguration configuration,IHttpClientFactory httpClientFactory)
        {
            this.everythingDbContext = everythingDbContext;
            this.configuration = configuration;
            httpClient = httpClientFactory.CreateClient();
        }


        public async Task<IActionResult> Index()
        {
            bool jwtControl = false;
            var data = await everythingDbContext.ProjectsInfo.ToListAsync();//Bu istek Api den istenecek.
            if (data!=null)
            {
                ActiveTokenControl tokenControl = new ActiveTokenControl();
                var jwtSettings = configuration.GetSection("Jwt");
                var signatureKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var jwt = Request.Cookies[GlobalCookiesNames.JwtCookieName];

               (var claims ,var DateTime) = tokenControl.TokenActiveControl(jwt, jwtSettings, signatureKey);

                if (claims!=null)
                {
                    jwtControl = true;
                }
                
                return View((data,jwtControl));
            }
            return StatusCode(StatusCodes.Status200OK);
        }


        public async Task<IActionResult> ProjectDelete(Guid id)
        { 
            var response = await httpClient.DeleteAsync($"https://localhost:7071/api/projectsadmin/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(controllerName: "Projects", actionName: "Index");
            }

            return StatusCode(StatusCodes.Status204NoContent);
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

            return RedirectToAction("Details", new { id, slug });
        }
    }
}
