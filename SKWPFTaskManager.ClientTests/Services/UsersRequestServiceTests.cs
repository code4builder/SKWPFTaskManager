using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace SKWPFTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        private AuthToken _token;
        private UsersRequestService _service;
        public UsersRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin", "qwerty123");
            _service = new UsersRequestService();
        }

        [TestMethod()]
        public void GetTokenTest()
        {
            Console.WriteLine(_token.access_token);
            Assert.IsNotNull(_token);
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            UserModel userTest = new UserModel("Alexander", "Selikhov", "selikhov@mail.com", "qwerty123", UserStatus.User, "54894456");

            var result = _service.CreateUser(_token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetAllUsersTest()
        {
            var result = _service.GetAllUsers(_token);

            Console.WriteLine(result.Count);

            Assert.AreNotEqual(Array.Empty<UserModel>, result.ToArray());
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            var result = _service.DeleteUser(_token, 10);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void CreateMultipleUserTest()
        {
            UserModel userTest1 = new UserModel("Alexander", "Selikhov", "selikhov@mail.com", "qwerty123", UserStatus.User, "54894456");
            UserModel userTest2 = new UserModel("Sasha", "Gray", "gray@mail.com", "qwerty123", UserStatus.Editor, "45633465");
            UserModel userTest3 = new UserModel("Artem", "Novruzov", "novruzov@mail.com", "qwerty123", UserStatus.User, "45654645");

            List<UserModel> users = new List<UserModel>() { userTest1, userTest2, userTest3 };

            var result = _service.CreateMultipleUsers(_token, users);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateUserTest()
        {
            UserModel userToUpdate = new UserModel("Petr", "Selikhov", "selikhov@mail.com", "qwerty123", UserStatus.User, "54894456");
            userToUpdate.Id = 13;

            var result = _service.UpdateUser(_token, userToUpdate);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}