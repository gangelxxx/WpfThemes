using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VideoStreamTagControlsLibrary.Windows;

namespace MessageWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

//            VideoStreamTagControlsLibrary.Windows.MessageWindow window = new VideoStreamTagControlsLibrary.Windows.MessageWindow();
//            window.SetMessage(TypeMessage.Error, "Ahtung", "VideoAPI Exception: O servidor remoto retornou um error: (502) ");
//            SetMainWindow(window);
        }

        private static void SetMainWindow(Window window)
        {
            window.Show();
            Current.MainWindow = window;
        }
    }
}
