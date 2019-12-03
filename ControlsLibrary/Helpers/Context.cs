using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace ControlsLibrary.Helpers
{
    public sealed class Context : IContext
    {
        private readonly Dispatcher _dispatcher;

        public bool IsSynchronized => this._dispatcher.Thread == Thread.CurrentThread;

        public Context() : this(Dispatcher.CurrentDispatcher)
        {}

        public Context(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            this._dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            this._dispatcher.BeginInvoke(action);
        }
    }
}