using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ControlsLibrary.VirtualizingCollection;
using TeamSelectionWindowTest.Model;

namespace TeamSelectionWindowTest.Providers
{
    public abstract class WaitComboProvider<T> : IVirtualListProvider<T> where T : IVirtualItem
    {
        protected ConcurrentStack<T> MainList = new ConcurrentStack<T>();
        private readonly ConcurrentDictionary<int, T> _backList = new ConcurrentDictionary<int, T>();

        public IList<T> FetchRange(int startIndex, int count)
        {
            var res = _backList.IsEmpty
                ? new List<T>()
                : (from i in _backList where i.Key >= startIndex && i.Key < startIndex + count select i.Value).ToList();

            return res;
        }

        public int FetchCount()
        {
            return _backList.Count;
        }

        public void Clear()
        {
            _backList.Clear();
            MainList.Clear();
        }

        private string oldSearchLine;

        public void Refresh(string searchLine)
        {
            try
            {
                if (string.IsNullOrEmpty(searchLine))
                {
                    if (_backList.IsEmpty || _backList.Count != MainList.Count)
                    {
                        _backList.Clear();
                        AddRange(MainList);
                    }
                }
                else
                {
                    _backList.Clear();
                    var str = searchLine.ToUpper();
                    List<T> firstList = MainList.AsParallel().Where(_ => _.UpperString.Contains(str)).ToList();
                    Dictionary<int, List<T>> dict = new Dictionary<int, List<T>>();

                    foreach (var item in firstList)
                    {
                        var idx = item.UpperString.IndexOf(searchLine, 0, StringComparison.CurrentCultureIgnoreCase);

                        if (!dict.ContainsKey(idx))
                        {
                            dict.Add(idx, new List<T>() { item });
                        }
                        else
                        {
                            dict[idx].Add(item);
                        }
                    }

                    for (var i = 0; i < dict.Count; i++)
                    {
                        if (dict.ContainsKey(i))
                        {
                            AddRange(dict[i]);
                        }
                    }
                }
            }
            catch
            {
                //                    Logging.Log.Write(ex);
            }

            oldSearchLine = searchLine;
        }

        private void AddRange(IEnumerable<T> range)
        {
            var idx = _backList.Count;
            foreach (var team in range)
            {
                _backList.TryAdd(idx++, team);
            }
        }

        public abstract void CreateMainList();
    }
}