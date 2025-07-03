using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using MyEverything.ThisMvc.Helpers.CookieGlobalVariablesValues;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

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
            if (!String.IsNullOrEmpty(Request.Cookies[GlobalCookiesNames.JwtCookieName]))
            {
                return RedirectToAction(actionName: "Index", controllerName: "AdminPanel");//--------------------
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                Response.Cookies.Append(GlobalCookiesNames.JwtCookieName, loginResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = loginResponse.AccessTokenExpiration
                });
                Response.Cookies.Append(GlobalCookiesNames.RefreshTokenName, loginResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = loginResponse.RefreshTokenExpiration
                });



                return RedirectToAction(actionName: "Index", controllerName: "AdminPanel");//--------------------
            }



            return View();

        }
    }
}

