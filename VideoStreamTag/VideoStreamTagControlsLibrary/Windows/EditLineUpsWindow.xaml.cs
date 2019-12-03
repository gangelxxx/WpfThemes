#region

using System.Windows;
using ControlsLibrary.Windows;
using VideoStreamTagControlsLibrary.ViewModels.EditLineUps;

#endregion

namespace VideoStreamTagControlsLibrary.Windows
{
    /// <summary>
    /// Interaction logic for EditLineUpsWindow.xaml
    /// </summary>
    public partial class EditLineUpsWindow : WindowTypeOne
    {
        #region Private fields

        private IEditLineUpsWindowViewModel _viewModel;

        #endregion

        #region Construct

        public EditLineUpsWindow(IEditLineUpsWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            Loaded += OnLoaded;
        }

        #endregion

        #region Public

        private IEditLineUpsWindowViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (value == null) return;

                _viewModel = value;
                DataContext = value;
            }
        }

        #endregion

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.CompleteCommandExecute();
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadLineUps();
        }
    }
}