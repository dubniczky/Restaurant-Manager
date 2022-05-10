
using Restaurant.Admin.Command;
using Restaurant.Admin.Model;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Restaurant.Admin.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        //Public members
        public string Username { get; set; }

        //Commands
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        //Events
        public event EventHandler LoginSuccess;
        public event EventHandler LoginFailed;
        public event EventHandler ExitApplication;

        //Private members
        private RestaurantModel model;

        //Constructor
        public LoginViewModel(RestaurantModel model)
        {
            if (model == null)
            {
                throw new ArgumentException(nameof(model));
            }

            this.model = model;
            Username = string.Empty;
            ExitCommand = new DelegateCommand(param => OnExitApplication());
            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        //Methods
        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null) throw new ArgumentNullException(nameof(passwordBox));

            try
            {
                bool result = await model.LoginAsync(Username, passwordBox.Password);
                if (result)
                {
                    OnLoginSuccess();
                }
                else
                {
                    OnLoginFailed();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No connection to the host.");
                OnMessageApplication("No connection to the host.");
            }
        }

        //Events
        private void OnLoginSuccess()
        {
            if (LoginSuccess != null) LoginSuccess(this, EventArgs.Empty);
        }
        private void OnLoginFailed()
        {
            if (LoginFailed != null) LoginFailed(this, EventArgs.Empty);
        }
        private void OnExitApplication()
        {
            if (ExitCommand != null) ExitApplication(this, EventArgs.Empty);
        }
    }
}
