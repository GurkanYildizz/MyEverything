
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.CQRS.Command;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers;
using MyEverything.ThisMvc.Helpers.CookieGlobalVariablesValues;
using MyEverything.ThisMvc.Helpers.Token;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace MyEverything.ThisMvc.Controllers.Mvc
{
    public class ProjectsController : Controller
    {

        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public ProjectsController(EverythingDbContext everythingDbContext, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {

            this.configuration = configuration;
            httpClient = httpClientFactory.CreateClient();
        }


        public async Task<IActionResult> Index()
        {
            bool jwtControl = false;
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7071/api/projectsuser/main-projects");
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ActiveTokenControl tokenControl = new ActiveTokenControl();
                var jwtSettings = configuration.GetSection("Jwt");
                var signatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var jwt = Request.Cookies[GlobalCookiesNames.JwtCookieName];

                (var claims, var DateTime) = tokenControl.TokenActiveControl(jwt, jwtSettings, signatureKey);

                if (claims != null)
                {
                    jwtControl = true;
                }
                var json = await response.Content.ReadAsStringAsync();
                var projectList = JsonConvert.DeserializeObject<List<ProjectInfo>>(json);


                return View((projectList, jwtControl));
            }
            return StatusCode(StatusCodes.Status204NoContent);



        }


        [Authorize]
        public async Task<IActionResult> ProjectDelete(Guid id)
        {
            var token = Request.Cookies[GlobalCookiesNames.JwtCookieName];
            if (String.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7071/api/projectsadmin/{id}");


            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(actionName: "Index", controllerName: "Projects");//Burası sildikten sonra yönlendirme yapacak
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }


        public async Task<IActionResult> Details(Guid id, string slug)
        {

            if (slug == null)
            {
                //return Error 404 ...
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7071/api/projectsuser/project-details/{id}");
            var response = await httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            var project = JsonConvert.DeserializeObject<ProjectInfo>(json);



            return View(project);

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

        public RedirectToActionResult DetailOfSelected(Guid id, string title)
        {
            var slug = GenerateSlug(title);

            return RedirectToAction("Details", new { id, slug });
        }
    }
}
