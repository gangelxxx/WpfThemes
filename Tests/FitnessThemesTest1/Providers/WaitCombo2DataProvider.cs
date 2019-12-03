using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ThemesLib.InStat.Helpers.VirtualizingCollection;

namespace FitnessThemesTest1.Providers
{
    public class WaitCombo2DataProvider : IVirtualListProvider<Customer>
    {
        #region Static

        private static readonly object locker = new object();

        #endregion

        #region Private fields

        private readonly int _fetchDelay;
        private readonly List<Customer> _filterList = new List<Customer>();
        private readonly List<Customer> _mainList = new List<Customer>();
        private bool _isBusy;
        private int _count;

        #endregion

        #region Construct

        public WaitCombo2DataProvider(int count, int fetchDelay = 0)
        {
            _count = count;
            _fetchDelay = fetchDelay;
            _isBusy = false;
        }

        public void Create()
        {
            // for (int i = 0; i < _count; i++)
            // {
            //     Customer customer = new Customer {Id = i + 1, FindingName = "Customer " + (i + 1)};
            //     _mainList.Add(customer);
            //     _filterList.Add(customer);
            // }
        }

        public void Create2()
        {
        //     for (int i = 0; i < _count; i++)
        //     {
        //         Customer customer = new Customer { Id = i + 1, FindingName = "Customerrr " + (i + 1) };
        //         _mainList.Add(customer);
        //         _filterList.Add(customer);
        //     }
        }

        #endregion

        #region Public

        public int FetchCount()
        {
            lock (locker)
            {
                return _filterList.Count;
            }
        }


        public List<IVirtualItem> MainList { get; }

        public IList<Customer> FetchRange(int startIndex, int count)
        {
            Trace.WriteLine("FetchRange: " + startIndex + "," + count);
            Thread.Sleep(_fetchDelay);

            List<Customer> list = new List<Customer>();
            lock (locker)
            {
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    if (_filterList.Count > i)
                    {
                        list.Add(_filterList[i]);
                    }
                }
            }

            return list;
        }

        public void Refresh(string str = "")
        {
            if (FilterFunc != null)
            {
                lock (locker)
                {
                    try
                    {
                        Thread.Sleep(1000);

                        _isBusy = true;

                        Debug.WriteLine("UpdateFilter");
                        _filterList.Clear();
                        for (int i = 0; i < _mainList.Count; i++)
                        {
                            if (FilterFunc.Invoke(_mainList[i].FindingName))
                            {
                                _filterList.Add(_mainList[i]);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _isBusy = false;
                    }
                    _isBusy = false;
                }
            }
        }

        public void CreateMainList()
        {
            throw new NotImplementedException();
        }

        public IList<Customer> GetMainList()
        {
            return _mainList;
        }

        public void Clear()
        {
            lock (locker)
            {
                _mainList.Clear();
                _filterList.Clear();
                _isBusy = false;
            }
        }


        public Func<string, bool> FilterFunc { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
        }

        List<Customer> IVirtualListProvider<Customer>.MainList => throw new NotImplementedException();

        #endregion


    }
}