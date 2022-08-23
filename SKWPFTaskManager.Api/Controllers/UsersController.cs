using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKWPFTaskManager.Api.Models;
using SKWPFTaskManager.Api.Models.Data;
using SKWPFTaskManager.Common.Models;

namespace SKWPFTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
        public UsersController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet("Test")]
        public IActionResult TestApi()
        {
            return Ok("All is ok!");
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                User newUser = new User(userModel.FirstName, userModel.LastName,
                    userModel.Email, userModel.Password, userModel.Status,
                    userModel.Phone, userModel.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPatch("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                User userForUpdate = _db.Users.FirstOrDefault(x => x.Id == id);
                if (userForUpdate != null)
                {
                    userForUpdate.FirstName = userModel.FirstName;
                    userForUpdate.LastName = userModel.LastName;
                    userForUpdate.Email = userModel.Email;
                    userForUpdate.Password = userModel.Password;
                    userForUpdate.Phone = userModel.Phone;
                    userForUpdate.Photo = userModel.Photo;
                    userForUpdate.Status = userModel.Status;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpPatch("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            User userToRemove = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userToRemove != null)
            {
                _db.Users.Remove(userToRemove);
                _db.SaveChanges();
                return Ok();
            }
            return NotFound();
        }


        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();
        }

        [HttpPost("create/all")]
        public async Task<IActionResult> CreateMultipleUsers([FromBody] List<UserModel> userModels)
        {
            if (userModels != null && userModels.Count > 0)
            {
                var newUsers = userModels.Select(u => new User(u));
                _db.AddRange(newUsers);
                await _db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
