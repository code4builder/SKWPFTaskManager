﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKWPFTaskManager.Api.Models.Data;
using SKWPFTaskManager.Common.Models;
using Microsoft.EntityFrameworkCore;
using SKWPFTaskManager.Api.Models.Services;
using SKWPFTaskManager.Api.Models;

namespace SKWPFTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly ProjectsService _projectsService;
        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _projectsService = new ProjectsService(db);
        }

        /// <summary>
        /// Get all projects for admin role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<CommonModel>> Get()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user.Status == UserStatus.Admin)
                return await _projectsService.GetAll().ToListAsync();
            else
            {
                return await _projectsService.GetByUserId(user.Id);
            }
        }

        /// <summary>
        /// Get projects by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var project = _projectsService.Get(id);
            return project == null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == user.Id);
                        if (admin == null)
                        {
                            admin = new ProjectAdmin(user);
                            _db.ProjectAdmins.Add(admin);
                            _db.SaveChanges();
                        }
                        projectModel.AdminId = admin.Id;

                        bool result = _projectsService.Create(projectModel);
                        return result ? Ok() : NotFound();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool result = _projectsService.Update(id, projectModel);
                        return result ? Ok() : NotFound();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _projectsService.Delete(id);
            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectsService.AddUsersToProject(id, usersIds);
                        return Ok();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [HttpPatch("{id}/users/remove")]
        public IActionResult RemoveUsersFromProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectsService.RemoveUsersFromProject(id, usersIds);
                        return Ok();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
    }
}
