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
using FitnessThemesTest1.TestViewModels;
using ThemesLib.InStat.Controls;

namespace FitnessThemesTest1.TestViews
{
    /// <summary>
    /// Interaction logic for WaitComboBoxView.xaml
    /// </summary>
    public partial class WaitComboBoxView : UserControl
    {
        public WaitComboBoxView()
        {
            InitializeComponent();

            DataContext = new WaitComboBoxViewModel();
        }

        private void NWaitComboBox_OnRefreshed(object sender, EventArgs e)
        {
            if (sender is WaitComboBox comboBox)
            {
                comboBox.Focus();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NWaitComboBox.Refresh();
        }

        private void OkSetNumberButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((WaitComboBoxViewModel)DataContext).SelectedIndexForWaitComboBox = Convert.ToInt32(NTextBox.Text);
        }
    }
}
