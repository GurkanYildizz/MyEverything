using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using MyEverything.ThisMvc.Helpers.Token;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEverything.ThisMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AdminLoginInfo> userManager;
        private readonly CreateTokensControl createTokensControl;
        
        public AuthController(UserManager<AdminLoginInfo> userManager, CreateTokensControl createTokensControl)
        {
            this.userManager = userManager;
            this.createTokensControl = createTokensControl;
           

        }



        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] AdminLogin_Dto adminLogin_Dto,CancellationToken cancellationToken)
        {
            #region Burası ilk başta admin olmadığı için admin eklemek için geçici çözüm
            /* var newAdminUser = new AdminLoginInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "GurkanYildiz",
                    Email = "gurkan@mail.com",
                    EmailConfirmed = true,

                };
                  await userManager.CreateAsync(newAdminUser, "1q2w3e4R!");
               */
            #endregion

            

            var user = await userManager.FindByEmailAsync(adminLogin_Dto.Email);


            if (user == null || !(await userManager.CheckPasswordAsync(user, adminLogin_Dto.Password)))
            {

                return StatusCode(StatusCodes.Status401Unauthorized);
                //Burada mesaj gönderilecek şifre yanlış vb...
            }
            
             LoginResponse_Dto addLoginResponse = await createTokensControl.TokenControls(user, cancellationToken);
            return Ok(addLoginResponse);

        }

       /* [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshTokens([FromBody] string refreshToken , CancellationToken cancellationToken)
        {
           
            
            return Ok("Buraya girildi");
        }
       */
       

       

       
    }
}
