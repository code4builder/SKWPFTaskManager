using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using SKWPFTaskManager.Client.Models;
using SKWPFTaskManager.Client.Services;
using SKWPFTaskManager.Client.Views;
using SKWPFTaskManager.Common.Models;

namespace SKWPFTaskManager.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        UsersRequestService _usersRequestService;

        #region COMMANDS
        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        public DelegateCommand<object> LoginFromCacheCommand { get; private set; }

        #endregion

        public LoginViewModel()
        {
            _usersRequestService = new UsersRequestService();
            CurrentUserCache = GetUserCache();

            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
            LoginFromCacheCommand = new DelegateCommand<object>(LoginFromCache);
        }

        #region PROPERTIES
        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }
        private string _cachePath = Path.GetTempPath() + "userTaskManager.txt";

        private UserModel _currentUser;
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private UserCache _currentUserCache;

        public UserCache CurrentUserCache
        {
            get => _currentUserCache;
            set
            {
                _currentUserCache = value;
                RaisePropertyChanged(nameof(CurrentUserCache));
            }
        }

        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }

        private Window _currentWindow;
        #endregion

        #region METHODS
        private void GetUserFromDB(object parameter)
        {
            var passBox = parameter as PasswordBox;

            bool isNewUser = false;

            _currentWindow = Window.GetWindow(passBox);

            if (UserLogin != CurrentUserCache?.Login || UserPassword != CurrentUserCache?.Password)
                isNewUser = true;

            UserPassword = passBox.Password;

            AuthToken = _usersRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken == null)
                return;

            CurrentUser = _usersRequestService.GetCurrentUser(AuthToken);

            if (CurrentUser != null)
            {
                if (isNewUser)
                {
                    var saveUserCacheMessage = MessageBox.Show("Do you want to save login and password?", "Saving login and password", MessageBoxButton.YesNo);
                    if (saveUserCacheMessage == MessageBoxResult.Yes)
                    {
                        UserCache newUserCache = new UserCache()
                        {
                            Login = UserLogin,
                            Password = UserPassword
                        };
                        CreateUserCache(newUserCache);
                    }
                }
                OpenMainWindow();
            }
        }

        private void CreateUserCache(UserCache userCache)
        {
            string jsonUserCache = JsonConvert.SerializeObject(userCache);

            using (StreamWriter sw = new StreamWriter(_cachePath, false, Encoding.Default))
            {
                sw.Write(jsonUserCache);
                MessageBox.Show("Login successful");
            }
        }

        private UserCache GetUserCache()
        {
            bool isCacheExist = File.Exists(_cachePath);
            if (isCacheExist && File.ReadAllText(_cachePath).Length > 0)
                return JsonConvert.DeserializeObject<UserCache>(File.ReadAllText(_cachePath));
            else
                return null;
        }

        private void LoginFromCache(object wnd)
        {
            _currentWindow = wnd as Window;
            UserLogin = CurrentUserCache.Login;
            UserPassword = CurrentUserCache.Password;
            AuthToken = _usersRequestService.GetToken(UserLogin, UserPassword);

            CurrentUser = _usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                OpenMainWindow();
            }
        }

        private void OpenMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Show();

            _currentWindow.Close();
        }
        #endregion
    }
}
