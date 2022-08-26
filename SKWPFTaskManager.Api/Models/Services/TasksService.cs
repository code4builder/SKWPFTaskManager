using SKWPFTaskManager.Api.Models.Abstractions;
using SKWPFTaskManager.Api.Models.Data;
using SKWPFTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKWPFTaskManager.Api.Models.Services
{
    public class TasksService : AbstractionService, ICommonService<TaskModel>
    {
        private readonly ApplicationContext _db;

        public TasksService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(TaskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Task newTask = new Task(model);
                _db.Tasks.Add(newTask);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Task taskToDelete = _db.Tasks.FirstOrDefault(t => t.Id == id);
                _db.Tasks.Remove(taskToDelete);
                _db.SaveChanges();
            });
            return result;
        }

        public TaskModel Get(int id)
        {
            Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            return task?.ToDto();
        }


        public IQueryable<CommonModel> GetTasksForUser(int userId)
        {
            return _db.Tasks.Where(t => t.CreatorId == userId || t.ExecutorId == userId).Select(t => t.ToDto() as CommonModel);
        }

        public bool Update(int id, TaskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Task taskToUpdate = _db.Tasks.FirstOrDefault(t => t.Id == id);

                taskToUpdate.Name = model.Name;
                taskToUpdate.Description = model.Description;
                taskToUpdate.Photo = model.Photo;
                taskToUpdate.StartDate = model.CreationDate;
                taskToUpdate.FinishDate = model.FinishDate;
                taskToUpdate.File = model.File;
                taskToUpdate.Column = model.Column;
                taskToUpdate.ExecutorId = model.ExecutorId;

                _db.Tasks.Update(taskToUpdate);
                _db.SaveChanges();
            });
            return result;
        }

        public IQueryable<CommonModel> GetAll(int deskId)
        {
            return _db.Tasks.Where(t => t.DeskId == deskId).Select(t => t.ToShortDto());
        }
    }
}
