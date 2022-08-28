using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKWPFTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace SKWPFTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("admin","qwerty123");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest = new UserModel("Alexander", "Selikhov", "selikhov@mail.com", "qwerty123", UserStatus.User, "54894456");

            var result = service.CreateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetAllUsersTest()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "qwerty123");

            var result = service.GetAllUsers(token);

            Console.WriteLine(result.Count);

            Assert.AreNotEqual(Array.Empty<UserModel>, result.ToArray());
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "qwerty123");

            var result = service.DeleteUser(token, 10);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void CreateMultipleUserTest()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest1 = new UserModel("Alexander", "Selikhov", "selikhov@mail.com", "qwerty123", UserStatus.User, "54894456");
            UserModel userTest2 = new UserModel("Sasha", "Gray", "gray@mail.com", "qwerty123", UserStatus.Editor, "45633465");
            UserModel userTest3 = new UserModel("Artem", "Novruzov", "novruzov@mail.com", "qwerty123", UserStatus.User, "45654645");

            List<UserModel> users = new List<UserModel>() { userTest1, userTest2, userTest3 };

            var result = service.CreateMultipleUsers(token, users);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateUserTest()
        {
            var service = new UsersRequestService();

            var token = service.GetToken("admin", "qwerty123");

            UserModel userToUpdate = new UserModel("Petr", "Selikhov", "selikhov@mail.com", "qwerty123", UserStatus.User, "54894456");
            userToUpdate.Id = 13;

            var result = service.UpdateUser(token, userToUpdate);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}