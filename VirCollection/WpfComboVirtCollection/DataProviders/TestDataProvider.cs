using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControlsLibrary.VirtualList.Interface;
using ControlsLibrary.VirtualList.SearchString;
using VirtlistLib;
using VirtlistLib.Interface;
using WpfComboVirtCollection.Data;

namespace WpfComboVirtCollection.DataProviders
{
    public class TestDataProvider : IDataProvider<VirtualListItem>
    {
        private ConcurrentDictionary<int, VirtualListItem> _list;
        private List<VirtualListItem> _filterlist = new List<VirtualListItem>();
        private SearchStringList _indexSearchStringList;
        private static readonly object _lockerFilter = new object();

        public async Task InitAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                lock (_lockerFilter)
                {
                    _indexSearchStringList = new SearchStringList(true);
                    _list = new ConcurrentDictionary<int, VirtualListItem>();

//                    int cc = 30;
//                    int idx = 0;
//
//                    for (int i = 0; i < cc; i++)
//                    {
//                        for (int j = 0; j < cc; j++)
//                        {
//                            for (int z = 0; z < cc; z++)
//                            {
//                                var user = new User(i.ToString() + j + z);
//                                _list.TryAdd(idx++, user);
//                                _indexSearchStringList.Add(user.ToString());
//                            }
//                        }
//                    }

                    Uniso.InStat.Web.MsSqlService.GetUserList();
                    for (var index = 0; index < Uniso.InStat.User.List.Count; index++)
                    {
                        Uniso.InStat.User item = Uniso.InStat.User.List[index];
                        if (!string.IsNullOrEmpty(item.Login))
                        {
                            var user = new User(index, item.Login);
                            _list.TryAdd(index, user);
                            _indexSearchStringList.Add(user.ToString());
                        }
                    }

                }
            });

            await ContainsAsync();
        }

        public int Count
        {
            get { lock (_lockerFilter) return _filterlist.Count; }
        }

        public async Task<IList<VirtualListItem>> GetPageAsync(int index, int count)
        {
            return await Task<IList<VirtualListItem>>.Factory.StartNew(() =>
            {
                lock (_lockerFilter)
                {
                    var filterCount = _filterlist.Count;

                    if (filterCount == 0)
                    {
                        return null;
                    }

                    if (filterCount < index + count)
                    {
                        return _filterlist.GetRange(0, filterCount);
                    }

                    return _filterlist.GetRange(index, count);
                }
            });
        }

        public async Task<bool> ContainsAsync(string keyword = "")
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                lock (_lockerFilter)
                {
                    _filterlist?.Clear();

                    if (string.IsNullOrEmpty(keyword))
                    {
                        _filterlist = new List<VirtualListItem>(_list.Select(x => x.Value));
                        return true;
                    }

                    _filterlist = new List<VirtualListItem>();

                    foreach (SearchStringList.StringPosition pos in _indexSearchStringList.FindAll(keyword))
                    {
                        var tempUser = _list[pos.ListIndex];
                        var str = tempUser.ToString();
                        var left = str.Substring(0, pos.StringIndex);
                        var sub = str.Substring(pos.StringIndex, keyword.Length);
                        var right = str.Substring(pos.StringIndex + keyword.Length);

                        tempUser.UpdateColorText(left, sub, right);

                        _filterlist.Add(tempUser);
                    }

                    return _filterlist.Count > 0;
                }
            });
        }
    }
}