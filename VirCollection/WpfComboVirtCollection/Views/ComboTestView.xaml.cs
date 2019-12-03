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
using WpfComboVirtCollection.ViewModels;

namespace WpfComboVirtCollection.Views
{
    /// <summary>
    /// Interaction logic for ComboTestView.xaml
    /// </summary>
    public partial class ComboTestView : UserControl
    {
        public ComboTestView()
        {
            InitializeComponent();

            DataContext = new ComboTestViewModel();
        }
    }
}
