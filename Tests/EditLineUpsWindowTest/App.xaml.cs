using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VideoStreamTagControlsLibrary.ViewModels.EditLineUps;
using VideoStreamTagControlsLibrary.Windows;

namespace EditLineUpsWindowTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //        EditLineUpsWindow

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //            SetMainWindow(new Window1());

            //            SetMainWindow(new LoginWindowControl());
            EditLineUpsWindow upsWindow = new EditLineUpsWindow(new EditLineUpsWindowViewModel(0));
            upsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            upsWindow.ShowDialog();

            if (upsWindow.DialogResult == true)
            {
                Console.WriteLine();
                //                MessageBox.Show("login:" + createMatchWindow.Login + " pass:" + createMatchWindow.Pass);
            }
        }
    }
}
