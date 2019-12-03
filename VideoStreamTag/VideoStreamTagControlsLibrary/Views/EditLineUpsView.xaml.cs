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
using VideoStreamTagControlsLibrary.ViewModels;
using VideoStreamTagControlsLibrary.ViewModels.EditLineUps;

namespace VideoStreamTagControlsLibrary.Views
{
    /// <summary>
    /// Interaction logic for EditLineUpsView.xaml
    /// </summary>
    public partial class EditLineUpsView : UserControl
    {
        public IEditLineUpsViewModel ViewModel { get; set; }

        public EditLineUpsView()
        {
            InitializeComponent();

            ViewModel = new EditLineUpsViewModel();
            DataContext = ViewModel;
        }
    }
}
