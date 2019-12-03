using System.Windows;
using ControlsLibrary.VirtualizingCollection;
using TeamSelectionWindowTest.Model;
using TeamSelectionWindowTest.Providers;
using VideoStreamTagControlsLibrary.ViewModels;
using VideoStreamTagControlsLibrary.ViewModels.Match;
using VideoStreamTagControlsLibrary.Windows;

namespace TeamSelectionWindowTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //            SetMainWindow(new LoginWindowControl());

            MatchSelectionWindow window = new MatchSelectionWindow();

            IVirtualListProvider<Team> customerProvider = new TeamProvider();
            customerProvider.CreateMainList();

            MatchSelectionWindowModel<Team> model = new MatchSelectionWindowModel<Team>
            {
                AutoComboSource = new AsyncVirtualizingCollection<Team>(customerProvider, 100, 300 * 1000)
            };

            window.DataContext = model;

            SetMainWindow(window);
        }

        private static void SetMainWindow(Window window)
        {
            window.Show();
            Current.MainWindow = window;
        }
    }

}
