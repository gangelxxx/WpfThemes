using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ControlsLibrary.Helpers;
using ControlsLibrary.Windows;
using VideoStreamTagControlsLibrary.Difthin;

namespace VideoStreamTagControlsLibrary.Windows
{
    public partial class MessageWindow : WindowTypeOne
    {
        public MessageWindow()
        {
            InitializeComponent();

            //            var fontFamilies = Fonts.GetFontFamilies(new Uri("pack://application:,,,/ControlsLibrary;component/Fonts/PFBeauSansPro/"), "./");
//            SetMessage(TypeMessage.Warning, "Ahtung", "VideoAPI Exception: O servidor remoto retornou um error: (502) ");
        }

        public void SetMessage(TypeMessage typeMessage, string title, string message)
        {
            switch (typeMessage)
            {
                case TypeMessage.Info:
                    Dispatcher.Invoke(() =>
                    {
                        VisualStateManager.GoToElementState(PART_MainGrid, "WarningSignInfoState", false);
                        BorderBrush = (Brush) FindResource(new ComponentResourceKey(typeof(WindowTypeOne), "KWindowTypeOneBackground_Cb"));
                    }, DispatcherPriority.Loaded);
                    break;
                case TypeMessage.Warning:
                    Dispatcher.Invoke(() =>
                    {
                        VisualStateManager.GoToElementState(PART_MainGrid, "WarningSignWarningState", false);
                        BorderBrush = "#CC9900".ToSolidColorBrush();
                    }, DispatcherPriority.Loaded);
                    break;
                case TypeMessage.Error:
                    Dispatcher.Invoke(() =>
                    {
                        VisualStateManager.GoToElementState(PART_MainGrid, "WarningSignErrorState", false);
                        BorderBrush = "#FF0000".ToSolidColorBrush();
                    }, DispatcherPriority.Loaded);
                    break;
            }

            Dispatcher.Invoke(() =>
            {
                Title = title;
//                Console.WriteLine(NMessageTextBlock.FontFamily);
                NMessageTextBlock.Text = message;
            });
        }

        private void ButtonCopyToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Clipboard.SetText(NMessageTextBlock.Text);
            });
        }


        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
    