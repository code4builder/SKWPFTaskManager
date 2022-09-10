using Prism.Commands;
using Prism.Mvvm;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Client.Services;
using SKWPFTaskManager.Client.Views.AddWindows;
using SKWPFTaskManager.Client.Views.Pages;
using SKWPFTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKWPFTaskManager.Client.ViewModels
{
    public class ProjectsPageViewModel : BindableBase
    {
        private AuthToken _token;
        private UsersRequestService _usersRequestService;
        private ProjectsRequestService _projectsRequestService;
        private CommonViewService _viewService;
        private MainWindowViewModel _mainWindowVM;

        #region COMMANDS

        public DelegateCommand OpenNewProjectCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateProjectCommand { get; private set; }
        public DelegateCommand<object> ShowProjectInfoCommand { get; private set; }
        public DelegateCommand CreateOrUpdateProjectCommand { get; private set; }
        public DelegateCommand DeleteProjectCommand { get; private set; }
        public DelegateCommand SelectPhotoForProjectCommand { get; private set; }
        public DelegateCommand AddUsersToProjectCommand { get; private set; }
        public DelegateCommand OpenNewUsersToProjectCommand { get; private set; }
        public DelegateCommand OpenProjectDesksPageCommand { get; private set; }

        #endregion
        public ProjectsPageViewModel(AuthToken token, MainWindowViewModel mainWindowVM)
        {
            _usersRequestService = new UsersRequestService();
            _projectsRequestService = new ProjectsRequestService();
            _viewService = new CommonViewService();
            _mainWindowVM = mainWindowVM;

            _token = token;
            UpdatePage();

            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            OpenUpdateProjectCommand = new DelegateCommand<object>(OpenUpdateProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
            CreateOrUpdateProjectCommand = new DelegateCommand(CreateOrUpdateProject);
            DeleteProjectCommand = new DelegateCommand(DeleteProject);
            SelectPhotoForProjectCommand = new DelegateCommand(SelectPhotoForProject);
            AddUsersToProjectCommand = new DelegateCommand(AddUsersToProject);
            OpenNewUsersToProjectCommand = new DelegateCommand(OpenNewUsersToProject);
            OpenProjectDesksPageCommand = new DelegateCommand(OpenProjectDesksPage);
        }

        #region PROPERTIES
        public UserModel CurrentUser
        {
            get => _usersRequestService.GetCurrentUser(_token);
        }

        private ClientAction _typeActionWithProject;
        public ClientAction TypeActionWithProject
        {
            get => _typeActionWithProject;
            set
            {
                _typeActionWithProject = value;
                RaisePropertyChanged(nameof(TypeActionWithProject));
            }
        }

        private List<ModelClient<ProjectModel>> _userProjects;

        public List<ModelClient<ProjectModel>> UserProjects
        {
            get => _userProjects;
            set
            {
                _userProjects = value;
                RaisePropertyChanged(nameof(UserProjects));
            }
        }

        private ModelClient<ProjectModel> _selectedProject;

        public ModelClient<ProjectModel> SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                RaisePropertyChanged(nameof(SelectedProject));
                if (SelectedProject?.Model.AllUsersIds != null && SelectedProject?.Model.AllUsersIds.Count > 0)
                    ProjectUsers = SelectedProject.Model.AllUsersIds?.Select(userId => _usersRequestService.GetUserById(_token, userId)).ToList();
                else
                    ProjectUsers = new List<UserModel>();
            }
        }

        private List<UserModel> _projectUsers = new List<UserModel>();

        public List<UserModel> ProjectUsers
        {
            get => _projectUsers;
            set
            {
                _projectUsers = value;
                RaisePropertyChanged(nameof(ProjectUsers));
            }
        }

        public List<UserModel> NewUsersForSelectedProject
        {
            get
            {
                var allUsers = _usersRequestService.GetAllUsers(_token);

                var result = allUsers.Where(user => ProjectUsers.Any(u => u.Id == user.Id) == false).ToList();
                return result;
            }
        }

        private List<UserModel> _selectedUsersForProject = new List<UserModel>();

        public List<UserModel> SelectedUsersForProject
        {
            get => _selectedUsersForProject;
            set
            {
                _selectedUsersForProject = value;
                RaisePropertyChanged(nameof(SelectedUsersForProject));
            }
        }

        private string _createOrUpdateProjectWindowTitle;

        public string CreateOrUpdateProjectWindowTitle
        {
            get => _createOrUpdateProjectWindowTitle;
            set
            {
                _createOrUpdateProjectWindowTitle = value;
                RaisePropertyChanged(nameof(CreateOrUpdateProjectWindowTitle));
            }
        }

        #endregion

        #region METHODS
        private void OpenNewProject()
        {
            SelectedProject = new ModelClient<ProjectModel>(new ProjectModel());
            TypeActionWithProject = ClientAction.Create;
            CreateOrUpdateProjectWindowTitle = "Create Project";

            var window = new CreateOrUpdateProjectWindow();

            _viewService.OpenWindow(window, this);
        }

        private void OpenUpdateProject(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);
            var adminId = _usersRequestService.GetProjectUserAdmin(_token, CurrentUser.Id);
            if (adminId == SelectedProject.Model.AdminId)
            {
                TypeActionWithProject = ClientAction.Update;
                CreateOrUpdateProjectWindowTitle = "Update Project";

                var window = new CreateOrUpdateProjectWindow();
                _viewService.OpenWindow(window, this);
            }
            else
            {
                _viewService.ShowMessage("You are not admin!");
            }
        }

        private void ShowProjectInfo(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);
        }

        private ModelClient<ProjectModel> GetProjectClientById(object projectId)
        {
            try
            {
                int id = (int)projectId;
                ProjectModel project = _projectsRequestService.GetProjectById(_token, id);
                return new ModelClient<ProjectModel>(project);
            }
            catch (FormatException)
            {
                return new ModelClient<ProjectModel>(null);
            }
        }

        private void CreateOrUpdateProject()
        {
            if (TypeActionWithProject == ClientAction.Create)
                CreateProject();
            if (TypeActionWithProject == ClientAction.Update)
                UpdateProject();

            UpdatePage();
        }

        private void CreateProject()
        {
            var resultAction = _projectsRequestService.CreateProject(_token, SelectedProject.Model);
            _viewService.ShowActionResult(resultAction, "New project has been created");
        }

        private void UpdateProject()
        {
            var resultAction = _projectsRequestService.UpdateProject(_token, SelectedProject.Model);
            _viewService.ShowActionResult(resultAction, "Project has been updated");
        }

        private void DeleteProject()
        {
            var resultAction = _projectsRequestService.DeleteProject(_token, SelectedProject.Model.Id);
            _viewService.ShowActionResult(resultAction, "Project has been deleted");

            UpdatePage();
        }

        private List<ModelClient<ProjectModel>> GetProjectsToClient()
        {
            _viewService.CurrentOpenedWindow?.Close();
            return _projectsRequestService.GetAllProjects(_token)?.Select(project => new ModelClient<ProjectModel>(project)).ToList();
        }

        private void SelectPhotoForProject()
        {
            _viewService.SetPhotoForObject(SelectedProject.Model);
            SelectedProject = new ModelClient<ProjectModel>(SelectedProject.Model);
        }

        private void OpenNewUsersToProject()
        {
            var window = new AddUsersToProjectWindow();
            _viewService.OpenWindow(window, this);
        }

        private void AddUsersToProject()
        {
            if (SelectedUsersForProject == null || SelectedUsersForProject?.Count == 0)
            {
                _viewService.ShowMessage("Select users");
                return;
            }
            var resultAction = _projectsRequestService.AddUsersToProjects(_token, SelectedProject.Model.Id, SelectedUsersForProject.Select(user => user.Id).ToList());
            _viewService.ShowActionResult(resultAction, "New users have been added to project ");

            UpdatePage();

            _viewService.CurrentOpenedWindow?.Close();
        }

        private void UpdatePage()
        {
            UserProjects = GetProjectsToClient();
            SelectedProject = null;
            SelectedUsersForProject = new List<UserModel>();
        }

        private void OpenProjectDesksPage()
        {
            if (SelectedProject?.Model != null)
            {
                var page = new ProjectDesksPage();
                _mainWindowVM.OpenPage(page, $"Desks of {SelectedProject.Model.Name}", new ProjectDesksPageViewModel(_token, SelectedProject.Model, _mainWindowVM));
            }
        }

        #endregion
    }
}
