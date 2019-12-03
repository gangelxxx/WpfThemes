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
using ThemesLib.InStat.Helpers;

namespace ColorComboBoxTextWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

//            NListBox.ItemsSource = new List<string>
//            {
//                "asdf",
//                "asdf",
//                "asdf",
//                "asdf"
//            };
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NComboBox.Selected = int.Parse(NTextBox.Text);

//            NComboBox.SelectedColor = "#FFCD00".ToSolidColorBrush();
        }
    }
}
