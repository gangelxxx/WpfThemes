using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ControlsLibrary.Controls;
using ControlsLibrary.Windows;
using VideoStreamTagControlsLibrary.ViewModels;
using VideoStreamTagControlsLibrary.ViewModels.Login;

namespace VideoStreamTagControlsLibrary.Windows
{
    public partial class LoginWindow : WindowTypeOne
    {
        public static readonly DependencyProperty PassProperty =
            DependencyProperty.Register(nameof(Pass), typeof(string), typeof(LoginWindow),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        public string Pass
        {
            get { return (string)GetValue(PassProperty); }
            set { SetValue(PassProperty, value); }
        }

        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register(nameof(Login), typeof(string), typeof(LoginWindow),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        public LoginWindow()
        {
            InitializeComponent();

            DataContext = new LoginWindowViewModel();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (TestLoginDataAsync == null) throw new NullReferenceException(nameof(TestLoginDataAsync));
            if (LoadUserName == null) throw new NullReferenceException(nameof(LoadUserName));
            if (SaveUserName == null) throw new NullReferenceException(nameof(SaveUserName));

            NTextBoxSearches.EndInitEvent += NTextBoxSearchesOnEndInitEvent;
        }

        private void NTextBoxSearchesOnEndInitEvent(object sender, EventArgs e)
        {
            Login = LoadUserName();
            NTextBoxSearches.Text = Login;
            NPasswordBox.SetFocus();
        }

        public delegate Task<bool> TestLoginDataAsyncDelegate(string login, string pass);
        public delegate string LoadUserNameDelegate();
        public delegate void SaveUserNameDelegate(string login);

        public TestLoginDataAsyncDelegate TestLoginDataAsync { get; set; }
        public LoadUserNameDelegate LoadUserName { get; set; }
        public SaveUserNameDelegate SaveUserName { get; set; }

        private async void NEnterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ButtonTypeOne buttonTypeOne)
            {
                buttonTypeOne.WaitState();

                if (!await TestLoginDataAsync(Login, Pass))
                {
                    VisualStateManager.GoToElementState(NMainGrid, "PasswordWrongState", false);
                }
                else
                {
                    Login = NTextBoxSearches.Text;
                    SaveUserName(Login);

                    if(System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                    {
                        DialogResult = true;
                    }
                }

                buttonTypeOne.NormalState();
            }
        }

        private void Timeline_OnCompleted(object sender, EventArgs e)
        {
            VisualStateManager.GoToElementState(NMainGrid, "PasswordNormalState", false);
        }

        private void NPasswordBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NEnterButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void NTextBoxSearches_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(NTextBoxSearches.Text))
            {
                NPasswordBox.SetFocus();
            }   
        }
    }
}
