using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Common.Models;
using System;
using System.Net;

namespace SKWPFTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class DesksRequestServiceTests
    {
        private AuthToken _token;
        private DesksRequestService _service;

        public DesksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new DesksRequestService();
        }

        [TestMethod()]
        public void GetAllDesksTest()
        {
            var desks = _service.GetAllDesks(_token);

            Console.WriteLine(desks.Count);

            Assert.AreNotEqual(Array.Empty<DeskModel>, desks);
        }

        [TestMethod()]
        public void GetDeskByIdTest()
        {
            var desk = _service.GetDeskById(_token, 3);

            Assert.AreNotEqual(null, desk);
        }

        [TestMethod()]
        public void GetDesksByProjectTest()
        {
            var desks = _service.GetDesksByProject(_token, 5);

            Assert.AreNotEqual(0, desks.Count);
        }

        [TestMethod()]
        public void CreateDeskTest()
        {
            DeskModel desk = new DeskModel("New test desk 6", "Description test desk 6", true, new string[] { "New", "Done" });
            desk.ProjectId = 3;
            desk.AdminId = 1;

            var result = _service.CreateDesk(_token, desk);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateDeskTest()
        {
            DeskModel desk = new DeskModel("Test desk 6 updated", "Description test desk 6 updated", false, new string[] { "NewUpd", "DoneUpd" });
            desk.ProjectId = 3;
            desk.AdminId = 1;
            desk.Id = 6;

            var result = _service.UpdateDesk(_token, desk);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteDeskByIdTest()
        {
            var result = _service.DeleteDesk(_token, 6);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}