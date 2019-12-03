#region

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

#endregion

namespace ControlsLibrary.Controls
{
    public class ComboBoxOne : ComboBox
    {
        #region Construct

        static ComboBoxOne()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxOne), new FrameworkPropertyMetadata(typeof(ComboBoxOne)));
        }

        public ComboBoxOne()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style) FindResource(new ComponentResourceKey(typeof(ComboBoxOne), "KComboBoxOneStyle"));
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

//            GetParts();
        }

        private void GetParts()
        {
            throw new NotImplementedException();
        }
    }
}