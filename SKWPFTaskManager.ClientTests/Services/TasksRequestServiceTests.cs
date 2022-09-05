using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Common.Models;
using System;
using System.Net;

namespace SKWPFTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class TasksRequestServiceTests
    {
        private AuthToken _token;
        private TasksRequestService _service;

        public TasksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new TasksRequestService();
        }

        [TestMethod()]
        public void GetAllTasksTest()
        {
            var tasks = _service.GetAllTasks(_token);

            Console.WriteLine(tasks.Count);

            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod()]
        public void GetTaskByIdTest()
        {
            var task = _service.GetTaskById(_token, 1);

            Assert.AreNotEqual(null, task);
        }

        [TestMethod()]
        public void GetTasksByDeskTest()
        {
            var tasks = _service.GetTasksByDesk(_token, 3);

            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod()]
        public void CreateTaskTest()
        {
            TaskModel task = new TaskModel("New test task 1", "Description test task 1", DateTime.Now, DateTime.Now, "New4");
            task.DeskId = 3;
            task.ExecutorId = 1;

            var result = _service.CreateTask(_token, task);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            TaskModel task = new TaskModel("New test updated task 1", "Description test updated task 1", DateTime.Now, DateTime.Now, "InProgress4");
            task.Id = 6;
            task.ExecutorId = 2;

            var result = _service.UpdateTask(_token, task);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteTaskByIdTest()
        {
            var result = _service.DeleteTask(_token, 6);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}