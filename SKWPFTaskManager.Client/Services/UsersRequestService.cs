using Newtonsoft.Json;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Common.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SKWPFTaskManager.Client.Services
{
    public class UsersRequestService : CommonRequestService
    {
        private string _usersControllerUrl = HOST + "users";

        public AuthToken GetToken(string userName, string password)
        {
            string url = HOST + "account/token";
            var resultStr = GetDataByUrl(HttpMethod.Post, url, null, userName, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(resultStr);
            return token;
        }

        public UserModel GetCurrentUser(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, HOST + "account/info", token);
            UserModel user = JsonConvert.DeserializeObject<UserModel>(response);
            return user;
        }

        public UserModel GetUserById(AuthToken token, int userId)
        {
            string response = GetDataByUrl(HttpMethod.Get, _usersControllerUrl + $"/{userId}", token);
            UserModel user = JsonConvert.DeserializeObject<UserModel>(response);
            return user;
        }

        public HttpStatusCode CreateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByURL(HttpMethod.Post, _usersControllerUrl, token, userJson);
            return result;
        }

        public List<UserModel> GetAllUsers(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _usersControllerUrl, token);
            List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(response);
            return users;
        }

        public HttpStatusCode DeleteUser(AuthToken token, int userId)
        {
            var result = DeleteDataByURL(_usersControllerUrl + $"/{userId}", token);
            return result;
        }

        public HttpStatusCode CreateMultipleUsers(AuthToken token, List<UserModel> users)
        {
            string userJson = JsonConvert.SerializeObject(users);
            var result = SendDataByURL(HttpMethod.Post, _usersControllerUrl + "/all", token, userJson);
            return result;
        }

        public HttpStatusCode UpdateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByURL(HttpMethod.Patch, _usersControllerUrl + $"/{user.Id}", token, userJson);
            return result;
        }

        public int? GetProjectUserAdmin(AuthToken token, int userId)
        {
            var result = GetDataByUrl(HttpMethod.Get, _usersControllerUrl + $"{userId}/admin", token);
            int adminId;
            bool parseResult = int.TryParse(result, out adminId);
            return adminId;
        }
    }
}
