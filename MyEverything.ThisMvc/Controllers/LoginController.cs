using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace MyEverything.ThisMvc.Controllers
{
    public class LoginController : Controller
    {

        readonly IHttpClientFactory httpClientFactory;
        public LoginController(IHttpClientFactory httpClientFactory)
        {

            this.httpClientFactory = httpClientFactory;
        }
        

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginAdmin()
        {
            if (!String.IsNullOrEmpty(Request.Cookies["jwt"]))
            {
                return RedirectToAction(actionName: "AddProject", controllerName: "Projects");//--------------------
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(AdminLogin_Dto adminLogin_Dto)
        {
            /* ViewBag.ErrorText = "";
             var admin = context.AdminLogins.FirstOrDefault(f => f.Email == adminLogin.Email);
             if (admin != null && Argon2.Verify(admin.Password, adminLogin.Password))
             {
                 return RedirectToAction(actionName: "AddProject",controllerName:"Projects");//Giriş başarılı
             }
             ViewBag.ErrorText = "E-posta veya şifre hatalı.";
            */

            var client = httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(adminLogin_Dto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7071/api/auth/login-admin", data);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse_Dto>();

                Response.Cookies.Append("jwt", loginResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = loginResponse.AccessTokenExpiration
                });
                Response.Cookies.Append("jwt-refresh", loginResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = loginResponse.RefreshTokenExpiration
                });



                return RedirectToAction(actionName: "AddProject", controllerName: "Projects");//--------------------
            }



            return View();

        }
    }
}

