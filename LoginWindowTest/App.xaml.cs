using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LoginWindowTest.Properties;
using VideoStreamTagControlsLibrary.Controls;
using VideoStreamTagControlsLibrary.Windows;

namespace LoginWindowTest
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetMainWindow(new Window1());

            //            SetMainWindow(new LoginWindowControl());
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginWindow.LoadUserName = LoadUserName;
            loginWindow.SaveUserName = SaveUserName;
            loginWindow.TestLoginDataAsync = TestLoginDataAsync;

            loginWindow.ShowDialog();

            if (loginWindow.DialogResult == true)
            {
                MessageBox.Show("login:" + loginWindow.Login + " pass:" + loginWindow.Pass);
            }
        }

        private async Task<bool> TestLoginDataAsync(string login, string pass)
        {
            await Task.Delay(1000);

            if (pass == "1")
            {
                return true;
            }

            return false;
        }

        private void SaveUserName(string login)
        {
            Settings.Default.LAST_USER_NAME = login;
            Settings.Default.Save();
        }

        private string LoadUserName()
        {
            return Settings.Default.LAST_USER_NAME;
        }

        private static void SetMainWindow(Window window)
        {
            window.Show();
            Current.MainWindow = window;
        }
    }
}
