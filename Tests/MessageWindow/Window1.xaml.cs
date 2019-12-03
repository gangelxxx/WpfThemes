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
using System.Windows.Shapes;
using VideoStreamTagControlsLibrary.Difthin;
using VideoStreamTagControlsLibrary.Windows;

namespace MessageWindow
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();


        }

        private void Info_OnChecked(object sender, RoutedEventArgs e)
        {
            var mw = new VideoStreamTagControlsLibrary.Windows.MessageWindow();
            mw.SetMessage(TypeMessage.Info, "Ahtung", "VideoAPI Exception: O servidor remoto retornou um error: (502) Классические черные дыры обладают столь большой гравитацией, что попав в ее гравитационную ловушку, ни материя, ни свет уже не могут покинуть пределы горизонта событий. Лабораторная черная дыра была создана на основе так называемого конденсата Бозе-Эйнштейна, и ее горизонт событий никак не влияет на свет, зато его свойства не дают выбраться за пределы черной дыры звуковым волнам. Другими словами, лабораторная черная дыра является акустической черной дырой.");
            mw.ShowDialog();
        }

        private void Warning_OnChecked(object sender, RoutedEventArgs e)
        {
            var mw = new VideoStreamTagControlsLibrary.Windows.MessageWindow();
            mw.SetMessage(TypeMessage.Warning, "Ahtung", "VideoAPI Exception: O servidor remoto retornou um error: (502) ");
            mw.ShowDialog();
        }

        private void Error_OnChecked(object sender, RoutedEventArgs e)
        {
            var mw = new VideoStreamTagControlsLibrary.Windows.MessageWindow();
            mw.SetMessage(TypeMessage.Error, "Ahtung", "VideoAPI ");
            mw.ShowDialog();
        }
    }
}
