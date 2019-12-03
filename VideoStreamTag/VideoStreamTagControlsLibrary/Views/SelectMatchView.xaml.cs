using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlsLibrary.Helpers;
using Uniso.InStat.ClickAndTag;
using VideoStreamTagControlsLibrary.Models;
using VideoStreamTagControlsLibrary.ViewModels;
using VideoStreamTagControlsLibrary.ViewModels.EditLineUps;
using VideoStreamTagControlsLibrary.ViewModels.Match;
using VideoStreamTagControlsLibrary.Windows;

namespace VideoStreamTagControlsLibrary.Views
{
    /// <summary>
    /// Interaction logic for SelectMatchView.xaml
    /// </summary>
    public partial class SelectMatchView : UserControl
    {
        public SelectMatchViewModel _viewModel;

        public SelectMatchView()
        {
            InitializeComponent();

            _viewModel = new SelectMatchViewModel(new Context(Dispatcher));
            DataContext = _viewModel;
        }

        public event EventHandler<TeamData> CreateMatchOnClickEvent;

      

        public void GoToShowWaitMatchSearchState()
        {
            VisualStateManager.GoToElementState(NMainGrid, "ShowWaitMatchSearchState", true);
        }

        public void GoToHideWaitMatchSearchState()
        {
            VisualStateManager.GoToElementState(NMainGrid, "HideWaitMatchSearchState", true);
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var res = _viewModel.FoundMatches.FirstOrDefault(x => x.Id == -1);
            var match = _viewModel.FoundMatches.FirstOrDefault(x => x.IsSelected);

            if (res != null && res.IsSelected && _viewModel.SelectedTeam != null && _viewModel.SelectedTeam.Tag is TeamData team)
            {
                OnCreateMatchOnClickEvent(team);
            }
            else if (match != null)
            {
                var editLineUpsWindowViewModel = new EditLineUpsWindowViewModel(((IApplication)Application.Current).LineUpsRepository, (int)match.Id);
                var editLineUpsWindow = new EditLineUpsWindow(editLineUpsWindowViewModel);
                editLineUpsWindow.ShowDialog();
                
                Window.GetWindow(this)?.Close();
            }
        }


        protected virtual void OnCreateMatchOnClickEvent(TeamData team)
        {
            CreateMatchOnClickEvent?.Invoke(this, team);
        }
    }
}
