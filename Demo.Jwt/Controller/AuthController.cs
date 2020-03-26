using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Jwt.Controller
{
    [ApiController]
    //[Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("api/nopermission")]
        public IActionResult NoPermission()
        {
            return Forbid("No Permission");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/auth")]
        public IActionResult Get(string userName, string pwd)
        {
            // 如果用户名和密码不为空，则验证通过
            if (CheckAccount(userName, pwd, out string role)) 
            {
                // 每次登陆动态刷新
                Const.ValidAudience = userName + pwd + DateTime.Now.ToString();
                // push the user's name into a claim, so we can identify the user later on
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim("Role", role)
                };

                // sign the token using a security key, This secret will be shared between api and anything that needs to check that the token is legit.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
                // use HmacSha256 encrypt the key
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // .net core's JwtSecurityToken class takes on the heavy lifting and actually creates the token
                var token = new JwtSecurityToken(
                    //颁发者
                    issuer: Const.Domain,
                    // 接收者
                    audience: Const.ValidAudience,
                    // user-defined claims
                    claims: claims,
                    // expires time
                    expires: DateTime.Now.AddMinutes(10),
                    // 签名证书
                    signingCredentials: creds
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            } else
            {
                return BadRequest(new { message = "user or password is incorrect" });
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool CheckAccount(string userName, string pwd, out string role) 
        {
            role = "user";
            if (string.IsNullOrEmpty(userName)) return false;

            if (userName.Equals("admin")) role = "admin";

            return true;
        }
    
    }



}