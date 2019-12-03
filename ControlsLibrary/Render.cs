using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Uniso.InStat
{
    public class Render
    {
        public static void Init()
        {
            new System.Windows.Application();
        }

        public static void Execute(Action a)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, a);
            }
            catch
            { }
        }
    }
}