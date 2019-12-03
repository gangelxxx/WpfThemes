using System.Windows;
using System.Windows.Controls;
using ControlsLibrary.Controls;

namespace ControlsLibrary.Theme.IntUpDownRes
{
    public partial class IntUpDownRes
    {
        private void IncreaseButtonClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).TemplatedParent is IntUpDown)
                ((IntUpDown)((Button)sender).TemplatedParent).Increase();
            else
            {
                (((Button) sender).TemplatedParent as IntUpDownNegativeDefault)?.Increase();
            }
        }

        private void DecreaseButtonClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).TemplatedParent is IntUpDown)
                ((IntUpDown)((Button)sender).TemplatedParent).Decrease();
            else
            {
                (((Button) sender).TemplatedParent as IntUpDownNegativeDefault)?.Decrease();
            }
        }
    }
}