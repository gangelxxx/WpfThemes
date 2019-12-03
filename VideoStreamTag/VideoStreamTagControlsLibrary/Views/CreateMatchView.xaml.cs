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
using Uniso.InStat.ClickAndTag;
using VideoStreamTagControlsLibrary.Models;
using VideoStreamTagControlsLibrary.ViewModels;
using VideoStreamTagControlsLibrary.ViewModels.EditLineUps;
using VideoStreamTagControlsLibrary.ViewModels.Match;
using VideoStreamTagControlsLibrary.Windows;

namespace VideoStreamTagControlsLibrary.Views
{
    /// <summary>
    /// Interaction logic for CreateMatchView.xaml
    /// </summary>
    public partial class CreateMatchView : UserControl
    {
        private ICreateMatchViewModel _viewModel;

        public ICreateMatchViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (value == null) return;

                _viewModel = value;
                DataContext = value;
            }
        }

        public CreateMatchView()
        {
            InitializeComponent();

            ViewModel = new CreateMatchViewModel();
        }

        private async void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var matchId = await ViewModel.CreateMatchAsync();

            if (matchId != null)
            {
                var editLineUpsWindowViewModel = new EditLineUpsWindowViewModel(((IApplication)Application.Current).LineUpsRepository, (int)matchId);
                var editLineUpsWindow = new EditLineUpsWindow(editLineUpsWindowViewModel);
                editLineUpsWindow.ShowDialog();
                
                Window.GetWindow(this)?.Close();
            }
            else
            {
                //todo: анимация тряски
            }
        }
    }
}
