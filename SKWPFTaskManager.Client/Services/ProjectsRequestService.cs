﻿using Newtonsoft.Json;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Common.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SKWPFTaskManager.Client.Services
{
    public class ProjectsRequestService : CommonRequestService
    {
        private string _projectsControllerUrl = HOST + "projects";

        public List<ProjectModel> GetAllProjects(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _projectsControllerUrl, token);
            List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(response);
            return projects;
        }

        public ProjectModel GetProjectById(AuthToken token, int projectId)
        {
            var response = GetDataByUrl(HttpMethod.Get, _projectsControllerUrl + $"/{projectId}", token);
            ProjectModel project = JsonConvert.DeserializeObject<ProjectModel>(response);

            return project;
        }

        public HttpStatusCode CreateProject(AuthToken token, ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = SendDataByURL(HttpMethod.Post, _projectsControllerUrl, token, projectJson);
            return result;
        }

        public HttpStatusCode UpdateProject(AuthToken token, ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = SendDataByURL(HttpMethod.Patch, _projectsControllerUrl + $"/{project.Id}", token, projectJson);
            return result;
        }

        public HttpStatusCode DeleteProject(AuthToken token, int projectId)
        {
            var result = DeleteDataByURL(_projectsControllerUrl + $"/{projectId}", token);
            return result;
        }

        public HttpStatusCode AddUsersToProjects(AuthToken token, int projectId, List<int> userIds)
        {
            string userIdsJson = JsonConvert.SerializeObject(userIds);
            var result = SendDataByURL(HttpMethod.Patch, _projectsControllerUrl + $"/{projectId}/users", token, userIdsJson);
            return result;
        }

        public HttpStatusCode RemoveUsersFromProject(AuthToken token, int projectId, List<int> userIds)
        {
            string userIdsJson = JsonConvert.SerializeObject(userIds);
            var result = SendDataByURL(HttpMethod.Patch, _projectsControllerUrl + $"/{projectId}/users/remove", token, userIdsJson);
            return result;
        }
    }
}
