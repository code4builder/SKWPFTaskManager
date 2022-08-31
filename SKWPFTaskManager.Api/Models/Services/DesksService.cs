using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SKWPFTaskManager.Api.Models.Abstractions;
using SKWPFTaskManager.Api.Models.Data;
using SKWPFTaskManager.Common.Models;

namespace SKWPFTaskManager.Api.Models.Services
{
    public class DesksService : AbstractionService, ICommonService<DeskModel>
    {
        private readonly ApplicationContext _db;

        public DesksService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(DeskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Desk newDesk = new Desk(model);
                _db.Desks.Add(newDesk);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Desk deskToDelete = _db.Desks.FirstOrDefault(d => d.Id == id);
                _db.Desks.Remove(deskToDelete);
                _db.SaveChanges();
            });
            return result;
        }

        public DeskModel Get(int id)
        {
            Desk desk = _db.Desks.Include(d => d.Tasks).FirstOrDefault(d => d.Id == id);
            var deskModel = desk?.ToDto();
            if (deskModel != null)
                deskModel.TasksIds = desk?.Tasks.Select(d => d.Id).ToList();
            return desk?.ToDto();
        }

        public bool Update(int id, DeskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Desk deskToUpdate = _db.Desks.FirstOrDefault(d => d.Id == id);

                deskToUpdate.Name = model.Name;
                deskToUpdate.Description = model.Description;
                deskToUpdate.Photo = model.Photo;
                deskToUpdate.AdminId = model.AdminId;
                deskToUpdate.IsPrivate = model.IsPrivate;
                deskToUpdate.ProjectId = model.ProjectId;
                deskToUpdate.Columns = JsonConvert.SerializeObject(model.Columns);

                _db.Desks.Update(deskToUpdate);
                _db.SaveChanges();
            });
            return result;
        }

        public IQueryable<CommonModel> GetAll(int userId)
        {
            return _db.Desks.Where(d => d.AdminId == userId).Select(d => d.ToShortDto());
        }

        public IQueryable<CommonModel> GetProjectDesks(int projectId, int userId)
        {
            return _db.Desks.Where(d => (d.ProjectId == projectId &&
            (d.AdminId == userId || d.IsPrivate == false))).Select(d => d.ToDto() as CommonModel);
        }

    }
}
