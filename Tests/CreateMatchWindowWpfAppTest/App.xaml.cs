using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VideoStreamTagControlsLibrary.Windows;

namespace CreateMatchWindowWpfAppTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //        CreateMatchWindow
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //            SetMainWindow(new Window1());

            //            SetMainWindow(new LoginWindowControl());
            MatchWindow matchWindow = new MatchWindow();
            matchWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            matchWindow.ShowDialog();

            if (matchWindow.DialogResult == true)
            {
                Console.WriteLine();
//                MessageBox.Show("login:" + createMatchWindow.Login + " pass:" + createMatchWindow.Pass);
            }
        }

//        private async Task<bool> TestLoginDataAsync(string login, string pass)
//        {
//            await Task.Delay(1000);
//
//            if (pass == "1")
//            {
//                return true;
//            }
//
//            return false;
//        }
//
//        private void SaveUserName(string login)
//        {
//            Settings.Default.LAST_USER_NAME = login;
//            Settings.Default.Save();
//        }
//
//        private string LoadUserName()
//        {
//            return Settings.Default.LAST_USER_NAME;
//        }
//
//        private static void SetMainWindow(Window window)
//        {
//            window.Show();
//            Current.MainWindow = window;
//        }
    }
}
