using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SKWPFTaskManager.Api.Models;
using SKWPFTaskManager.Api.Models.Data;
using SKWPFTaskManager.Api.Models.Services;
using SKWPFTaskManager.Common.Models;
using System.IdentityModel.Tokens.Jwt;

namespace SKWPFTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        public AccountController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }

        [Authorize]
        [HttpGet("info")]
        public IActionResult GetCurrentUserInfo()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == userName);

            if (user != null)
            {
                return Ok(user.ToDto());
            }
            return NotFound();
        }

        [HttpPost("token")]
        public IActionResult GetToken()
        {
            var userData = _userService.GetUserLoginPassFromBasicAuth(Request);
            var login = userData.Item1;
            var pass = userData.Item2;
            var identity = _userService.GetIdentity(login, pass);

            if (identity == null)
            {
                return BadRequest("Wrong login or password");
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }
        
        [HttpPatch("update")]
        public IActionResult UpdateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                string userName = HttpContext.User.Identity.Name;
                User userForUpdate = _db.Users.FirstOrDefault(x => x.Email == userName);
                if (userForUpdate != null)
                {
                    userForUpdate.FirstName = userModel.FirstName;
                    userForUpdate.LastName = userModel.LastName;
                    userForUpdate.Password = userModel.Password;
                    userForUpdate.Phone = userModel.Phone;
                    userForUpdate.Photo = userModel.Photo;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }


    }
}
