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

namespace FitnessThemesTest1.TestViews
{
    /// <summary>
    /// Interaction logic for TextBoxRegexView.xaml
    /// </summary>
    public partial class TextBoxRegexView : UserControl
    {
        public TextBoxRegexView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            NTextBoxRegex1.IsEnabled = false;
        }

        private void Button2_OnClick(object sender, RoutedEventArgs e)
        {
            NTextBoxRegex1.IsEnabled = true;
        }
    }
}
