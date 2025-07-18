using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using MyEverything.ThisMvc.Helpers.CookieGlobalVariablesValues;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyEverything.ThisMvc.Controllers.Mvc
{
    [Authorize]
    public class AdminPanelController : Controller
    {
        private readonly HttpClient httpClient;

        public AdminPanelController(IHttpClientFactory httpClientFactory)
        {
            httpClient= httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult AddProject()
        {


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectInfoAndImage_Dto projectInfoImage)
        {
            
            

            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(projectInfoImage.ProjectName ?? ""), nameof(projectInfoImage.ProjectName));
            formData.Add(new StringContent(projectInfoImage.Explanation ?? ""), nameof(projectInfoImage.Explanation));
            formData.Add(new StringContent(projectInfoImage.Version.ToString()), nameof(projectInfoImage.Version));
            formData.Add(new StringContent(projectInfoImage.Title ?? ""), nameof(projectInfoImage.Title));
            formData.Add(new StringContent(projectInfoImage.YoutubeLink ?? ""), nameof(projectInfoImage.YoutubeLink));

            // Kapak fotoğrafı
            if (projectInfoImage.ImageFile != null )
            {
                var fileContent = new StreamContent(projectInfoImage.ImageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(projectInfoImage.ImageFile.ContentType);
                formData.Add(fileContent, nameof(projectInfoImage.ImageFile),projectInfoImage.ImageFile.FileName);//içerik,resmin yolu , resmin tam ismi(cat.jpg)
            }


            // Galeri resimleri
            if (projectInfoImage.ProjectImageFiles != null)
            {
                foreach (var image in projectInfoImage.ProjectImageFiles)
                {
                    var fileContent = new StreamContent(image.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                    formData.Add(fileContent, nameof(projectInfoImage.ProjectImageFiles), image.FileName);
                }
            }



            var token = Request.Cookies[GlobalCookiesNames.JwtCookieName];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7071/api/projectsadmin/addproject");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            request.Content = formData;

            var response=await httpClient.SendAsync(request);
            

            if (response.IsSuccessStatusCode)
            {
                return View();// Başarılı olunca projeler sayfasına gidecek...
            }
            return RedirectToAction("Index");

            


        }
    }
}
