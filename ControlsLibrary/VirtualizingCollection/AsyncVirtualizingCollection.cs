#region

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Timers;

#endregion

namespace ControlsLibrary.VirtualizingCollection
{
    public class AsyncVirtualizingCollection<T> : VirtualizingCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
        where T : IVirtualItem
    {
        #region Events

        
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private fields

        private readonly SynchronizationContext _synchronizationContext;


        private bool _isLoading;

        #endregion

        #region Protected fields

        protected SynchronizationContext SynchronizationContext
        {
            get { return _synchronizationContext; }
        }

        #endregion

        #region Construct

        public AsyncVirtualizingCollection(IVirtualListProvider<T> virtualListProvider)
            : this(virtualListProvider, -1 , -1)
        {}

        public AsyncVirtualizingCollection(IVirtualListProvider<T> virtualListProvider, int pageSize)
            : this(virtualListProvider, pageSize, -1)
        {}

        private System.Timers.Timer _timerReset;
        private System.Timers.Timer _timerLoadCompleted;

        public AsyncVirtualizingCollection(IVirtualListProvider<T> virtualListProvider, int pageSize, int pageTimeout)
            : base(virtualListProvider, pageSize, pageTimeout)
        {
            _synchronizationContext = SynchronizationContext.Current;

            _timerReset = new System.Timers.Timer();
            _timerReset.AutoReset = false;
            _timerReset.Interval = 5;
            _timerReset.Elapsed += TimerResetOnElapsed;
            _timerReset.Enabled = false;

            _timerLoadCompleted = new System.Timers.Timer();
            _timerLoadCompleted.AutoReset = false;
            _timerLoadCompleted.Interval = 500;
            _timerLoadCompleted.Elapsed += TimerLoadCompletedOnElapsed;
            _timerLoadCompleted.Enabled = false;
        }

        private void TimerResetOnElapsed(object sender, ElapsedEventArgs e)
        {
            SynchronizationContext.Send(UpdateCollection, null);
            _timerLoadCompleted.Stop();
            _timerLoadCompleted.Start();
        }

        private void TimerLoadCompletedOnElapsed(object sender, ElapsedEventArgs e)
        {
            OnLoadCompleted();
        }

        private void UpdateCollection(object state)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region Private methods


        private void LoadCountWork(object args)
        {
            int count = FetchCount();
            SynchronizationContext.Send(LoadCountCompleted, count);
        }


        private void LoadPageWork(object args)
        {
            int pageIndex = (int) args;
            var page = FetchPage(pageIndex);
            SynchronizationContext.Send(LoadPageCompleted, new object[] {pageIndex, page});
        }

        private void LoadCountCompleted(object args)
        {
            Count = (int) args;

            TikCollectionChanged();
        }

        private void LoadPageCompleted(object args)
        {
            int pageIndex = (int) ((object[]) args)[0];
            var page = (IList<T>) ((object[]) args)[1];

            PopulatePage(pageIndex, page);

            TikCollectionChanged();
        }

        private void TikCollectionChanged()
        {
            _timerReset.Stop();
            _timerReset.Start();
        }

        #endregion

        #region Protected Методы

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }


        protected override void LoadCount()
        {
            Count = 0;
            ThreadPool.QueueUserWorkItem(LoadCountWork);
        }

        protected override void LoadPage(int index)
        {
            ThreadPool.QueueUserWorkItem(LoadPageWork, index);
        }

        #endregion
    }
}