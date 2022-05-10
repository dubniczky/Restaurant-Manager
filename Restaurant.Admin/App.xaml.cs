using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Restaurant.Admin.Model;
using Restaurant.Admin.Persistence;
using Restaurant.Admin.ViewModel;
using Restaurant.Admin.View;

namespace Restaurant.Admin
{
    public partial class App : Application
    {
        private RestaurantModel model;

        private LoginViewModel loginViewModel;
        private LoginWindow loginView;

        private MainViewModel mainViewModel;
        private MainWindow mainView;

        private FoodEditWindow foodEditView;
        private OrderEditWindow orderEditView;
        private CategoryEditWindow categoryEditView;

        public App()
        {
            Startup += new StartupEventHandler(AppStartup);
            Exit += new ExitEventHandler(AppExit);
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            model = new RestaurantModel(new RestaurantPersistence("https://localhost:44395/"));

            //Init Login View
            loginViewModel = new LoginViewModel(model);
            loginViewModel.ExitApplication += new EventHandler(ExitApplication);
            loginViewModel.LoginSuccess += new EventHandler(LoginSuccess);
            loginViewModel.LoginFailed += new EventHandler(LoginFailed);

            loginView = new LoginWindow();
            loginView.DataContext = loginViewModel;
            loginView.Show();
        }

        private async void AppExit(object sender, ExitEventArgs e)
        {
            if (model.IsLoggedIn)
            {
                await model.LogoutAsync();
            }
        }
        public void ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }
        public void MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Restaurant", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public void LoginSuccess(object sender, EventArgs e)
        {
            mainViewModel = new MainViewModel(model);
            mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(MessageApplication);

            mainViewModel.FoodEditStarted += new EventHandler(MainFoodEditStarted);
            mainViewModel.FoodEditEnded += new EventHandler(MainFoodEditEnded);
            mainViewModel.OrderEditStarted += new EventHandler(MainOrderEditStarted);
            mainViewModel.OrderEditEnded += new EventHandler(MainOrderEditEnded);
            mainViewModel.CategoryEditStarted += new EventHandler(MainCategoryEditStarted);
            mainViewModel.CategoryEditEnded += new EventHandler(MainCategoryEditEnded);

            mainView = new MainWindow();
            mainView.DataContext = mainViewModel;
            mainView.Show();

            loginView.Close();
        }
        public void LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Could not log in!", "Restaurant", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public void MainFoodEditStarted(object sender, EventArgs e)
        {
            foodEditView = new FoodEditWindow();
            foodEditView.DataContext = mainViewModel;
            foodEditView.ShowDialog();
        }
        public void MainFoodEditEnded(object sender, EventArgs e)
        {
            foodEditView.Close();
        }

        public void MainCategoryEditStarted(object sender, EventArgs e)
        {
            categoryEditView = new CategoryEditWindow();
            categoryEditView.DataContext = mainViewModel;
            categoryEditView.ShowDialog();
        }
        public void MainCategoryEditEnded(object sender, EventArgs e)
        {
            categoryEditView.Close();
        }

        public void MainOrderEditStarted(object sender, EventArgs e)
        {
            orderEditView = new OrderEditWindow();
            orderEditView.DataContext = mainViewModel;
            orderEditView.ShowDialog();
        }
        public void MainOrderEditEnded(object sender, EventArgs e)
        {
            orderEditView.Close();
        }
    }
}
