using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using MyEverything.ThisMvc.Helpers.Token;
using MyMediatr;

namespace MyEverything.ThisMvc.CQRS.Queries
{

    public class AuthorQuery : AdminLogin_Dto, IRequest<LoginResponse_Dto>
    {

    }
    public class AuthorQueryHandler : IRequestHandler<AuthorQuery, LoginResponse_Dto>
    {
        private readonly UserManager<AdminLoginInfo> userManager;
        private readonly CreateTokensControl createTokensControl;
        public AuthorQueryHandler(UserManager<AdminLoginInfo> userManager, CreateTokensControl createTokensControl)
        {

            this.userManager = userManager;
            this.createTokensControl = createTokensControl;

        }



        public async Task<LoginResponse_Dto> Handle(AuthorQuery request, CancellationToken cancellationToken)
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



            var user = await userManager.FindByEmailAsync(request.Email);


            if (user == null || !(await userManager.CheckPasswordAsync(user, request.Password)))
            {

                return null;
                //Burada mesaj gönderilecek şifre yanlış vb...
            }

            var addLoginResponse = await createTokensControl.TokenControls(user, cancellationToken);
            return addLoginResponse;
        }
    }
}
