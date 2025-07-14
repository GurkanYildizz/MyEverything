using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyEverything.ThisMvc.CQRS.Command;
using MyEverything.ThisMvc.CQRS.Queries;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using MyEverything.ThisMvc.Helpers.Token;
using MyMediatr;
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

        private readonly ISender sender;

        public AuthController(ISender sender)
        {
            this.sender = sender;
        }



        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] AuthorQuery authorQuery, CancellationToken cancellationToken)
        {
            var response = await sender.Send(authorQuery, cancellationToken);

            return Ok(response);
        }







    }
}
