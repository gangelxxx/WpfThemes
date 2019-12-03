#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ControlsLibrary.Helpers;
using ControlsLibrary.VirtualList.Interface;
using Uniso.InStat.Web;
using VirtlistLib.Interface;

#endregion

namespace VideoStreamTagControlsLibrary.Providers
{
    public class TeamDataProvider : IDataProvider<VirtualListItem>
    {
        #region Static

        private static readonly object _lockerFilter = new object();

        #endregion

        #region Private fields

        private List<VirtualListItem> _filterList = new List<VirtualListItem>();
        private long _gender = 1;

        #endregion

        #region Public

        public async Task InitAsync()
        {
            await Task.Delay(1);
        }

        public async Task<IList<VirtualListItem>> GetPageAsync(int index, int count)
        {
            return await Task<IList<VirtualListItem>>.Factory.StartNew(() =>
            {
                lock (_lockerFilter)
                {
                    var filterCount = _filterList.Count;

                    if (filterCount == 0)
                    {
                        return null;
                    }

                    if (filterCount < index + count)
                    {
                        return _filterList.GetRange(0, filterCount);
                    }

                    return _filterList.GetRange(index, count);
                }
            });
        }

        public async Task<bool> ContainsAsync(string keyword = "")
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                lock (_lockerFilter)
                {
                    _filterList = new List<VirtualListItem>();

                    if (string.IsNullOrEmpty(keyword))
                    {
                        return false;
                    }

                    var lowerKeyword = keyword.ToLower();
                    var gender = Interlocked.Read(ref _gender);
                    var teams = MsSqlService.SearchTeamsForClkTag(lowerKeyword, (int) gender);
                    foreach (var teamData in teams)
                    {
                        var rus = teamData.NameRus;
                        var eng = teamData.NameEng;
                        var rusTrans = Transliteration.Front(rus);

                        var sub = GetSubText(rus, lowerKeyword) ?? (GetSubText(eng, lowerKeyword) ?? GetSubText(rusTrans, lowerKeyword));

                        if (sub == null) continue;

                        var item = new VirtualListItem(teamData.Id, sub.Value.origin, teamData);
                        item.UpdateColorText(sub.Value.left, sub.Value.centr, sub.Value.right);

                        _filterList.Add(item);
                    }

                    return _filterList.Count > 0;
                }
            });
        }

        public int Gender
        {
            get => (int) _gender;
            set => _gender = value;
        }

        public int Count
        {
            get
            {
                lock (_lockerFilter) return _filterList.Count;
            }
        }

        #endregion

        #region Private methods

        private SubLeftText? GetSubText(string str, string keyword)
        {
            int idx = str.ToLower().IndexOf(keyword, StringComparison.Ordinal);

            if (idx == -1)
            {
                return null;
            }

            int end = keyword.Length;

            SubLeftText sub = new SubLeftText
            {
                origin = str,
                left = str.Substring(0, idx),
                centr = str.Substring(idx, end),
                right = str.Substring(idx + end)
            };

            return sub;
        }

        #endregion

        #region Struct

        private struct SubLeftText
        {
            public string origin;
            public string left;
            public string centr;
            public string right;
        }

        #endregion
    }
}