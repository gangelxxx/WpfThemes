#region

using System;
using System.Collections.Generic;
using System.Linq;
using ThemesLib.InStat.Helpers.VirtualizingCollection;

#endregion

namespace ThemesLib.InStat.Providers
{
    public class WaitComboProvider<T> : IVirtualListProvider<T> where T : IVirtualItem
    {
        #region Construct

        protected WaitComboProvider()
        {
            CreateMainList();
        }

        #endregion

        #region Locker

        private static readonly object Locker = new object();

        #endregion

        #region Other members

        protected readonly List<T> _filterList = new List<T>();

        #endregion

        #region Public

        public virtual void CreateMainList()
        {
            throw new System.NotImplementedException();
        }

        public IList<T> FetchRange(int startIndex, int count)
        {
            List<T> list = new List<T>();
            lock (Locker)
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

        public int FetchCount()
        {
            lock (Locker)
            {
                return _filterList.Count;
            }
        }

        public virtual void Refresh(string searchLine)
        {
            lock (Locker)
            {
                try
                {
                    _filterList.Clear();

                    if (string.IsNullOrEmpty(searchLine))
                    {
                        _filterList.AddRange(MainList);
                    }
                    else
                    {
                        List<T> firstList = MainList.AsParallel().Where(_ => _.ToString().ToUpper().Contains(searchLine.ToUpper())).ToList();
                        Dictionary<int, List<T>> dict = new Dictionary<int, List<T>>();

                        foreach (var item in firstList)
                        {
                            var idx = item.ToString().IndexOf(searchLine, 0, StringComparison.CurrentCultureIgnoreCase);

                            if (!dict.ContainsKey(idx))
                            {
                                dict.Add(idx, new List<T>() {item});
                            }
                            else
                            {
                                dict[idx].Add(item);
                            }
                        }

                        for (var i = 0; i < dict.Count; i++)
                        {
                            if (dict.ContainsKey(i))
                                _filterList.AddRange(dict[i]);
                        }
                    }
                }
                catch
                {
//                    Logging.Log.Write(ex);
                }
            }
        }

        public void Clear()
        {
            lock (Locker)
            {
                MainList.Clear();
                _filterList.Clear();
            }
        }

        public List<T> MainList { get; } = new List<T>();

        #endregion
    }
}