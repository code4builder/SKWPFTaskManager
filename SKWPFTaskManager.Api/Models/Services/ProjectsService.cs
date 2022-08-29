using Microsoft.EntityFrameworkCore;
using SKWPFTaskManager.Api.Models.Abstractions;
using SKWPFTaskManager.Api.Models.Data;
using SKWPFTaskManager.Common.Models;

namespace SKWPFTaskManager.Api.Models.Services
{
    public class ProjectsService : AbstractionService, ICommonService<ProjectModel>
    {
        private readonly ApplicationContext _db;

        public ProjectsService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(ProjectModel model)
        {
            bool result = DoAction(delegate ()
            {
                Project newProject = new Project(model);
                _db.Projects.Add(newProject);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Project projectToRemove = _db.Projects.FirstOrDefault(p => p.Id == id);
                _db.Projects.Remove(projectToRemove);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Update(int id, ProjectModel model)
        {
            bool result = DoAction(delegate ()
            {
                Project projectToUpdate = _db.Projects.FirstOrDefault(p => p.Id == id);

                projectToUpdate.Name = model.Name;
                projectToUpdate.Description = model.Description;
                projectToUpdate.Photo = model.Photo;
                projectToUpdate.Status = model.Status;
                //projectToUpdate.AdminId = model.AdminId;

                _db.Projects.Update(projectToUpdate);
                _db.SaveChanges();
            });
            return result;
        }

        public ProjectModel Get(int id)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).Include(p => p.AllDesks).FirstOrDefault(p => p.Id == id);
            //get all users from projects
            var projectModel = project?.ToDto();
            if (projectModel != null)
            {
                projectModel.AllUsersIds = project.AllUsers.Select(u => u.Id).ToList();
                projectModel.AllDesksIds = project.AllDesks.Select(u => u.Id).ToList();
            }
            return projectModel;
        }

        public async Task<IEnumerable<ProjectModel>> GetByUserId(int userId)
        {
            List<ProjectModel> result = new List<ProjectModel>();
            var admin = _db.ProjectAdmins.FirstOrDefault(p => p.UserId == userId);
            if (admin == null)
            {
                var projectsForAdmin = await _db.Projects.Where(p => p.AdminId == admin.Id).Select(p => p.ToDto()).ToListAsync();
                result.AddRange(projectsForAdmin);
            }
            var projectsByUser = await _db.Projects.Include(p => p.AllUsers).Where(p => p.AllUsers.Any(u => u.Id == userId)).Select(p => p.ToDto()).ToListAsync();
            result.AddRange(projectsByUser);
            return result;
        }

        public IQueryable<CommonModel> GetAll()
        {
            return _db.Projects.Select(p => p.ToDto() as CommonModel);
        }

        public void AddUsersToProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.FirstOrDefault(p => p.Id == id);

            foreach (int userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.AllUsers.Contains(user) == false)
                    project.AllUsers.Add(user);
            }
            _db.SaveChanges();
        }

        public void RemoveUsersFromProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);

            foreach (int userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.AllUsers.Contains(user))
                    project.AllUsers.Remove(user);
            }
            _db.SaveChanges();
        }
    }
}
