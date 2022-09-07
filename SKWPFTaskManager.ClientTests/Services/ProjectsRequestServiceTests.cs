using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace SKWPFTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class ProjectsRequestServiceTests
    {
        private AuthToken _token;
        private ProjectsRequestService _service;

        public ProjectsRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new ProjectsRequestService();
        }


        [TestMethod()]
        public void GetAllProjectsTest()
        {
            var projects = _service.GetAllProjects(_token);

            Console.WriteLine(projects.Count);

            Assert.AreNotEqual(Array.Empty<ProjectModel>, projects);
        }

        [TestMethod()]
        public void GetProjectByIdTest()
        {
            var project = _service.GetProjectById(_token, 3);

            Assert.AreNotEqual(null, project);
        }

        [TestMethod()]
        public void CreateProjectTest()
        {
            ProjectModel project = new ProjectModel("New project 3", "Description project 3", ProjectStatus.To_Do);
            project.AdminId = 1;

            var result = _service.CreateProject(_token, project);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateProjectTest()
        {
            ProjectModel project = new ProjectModel("New project 5 updated", "Description project 5 updated", ProjectStatus.To_Do);
            project.Id = 6;

            var result = _service.UpdateProject(_token, project);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteProjectTest()
        {
            var result = _service.DeleteProject(_token, 6);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void AddUsersToProjectsTest()
        {
            var result = _service.AddUsersToProjects(_token, 3, new List<int>() { 5, 6, 7 });

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void RemoveUsersFromProjectTest()
        {
            var result = _service.RemoveUsersFromProject(_token, 3, new List<int>() { 5 });

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}