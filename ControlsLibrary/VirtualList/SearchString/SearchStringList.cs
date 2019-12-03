using System;
using System.Collections.Generic;

namespace ControlsLibrary.VirtualList.SearchString
{
    public class SearchStringList
    {
        private readonly List<string> _strings = new List<string>();
        private readonly List<StringPosition> _positions = new List<StringPosition>();
        private bool _dirty;
        private readonly bool _ignoreCase;

        public SearchStringList(bool ignoreCase)
        {
            this._ignoreCase = ignoreCase;
        }

        public void Add(string s)
        {
            if (s.Length > 255) throw new ArgumentOutOfRangeException("string too big.");
            this._strings.Add(s);
            this._dirty = true;
            var listIdx = _strings.Count - 1;
            for (byte i = 0; i < s.Length; i++)
            {
                var stringPosition = new StringPosition(listIdx, i);
                this._positions.Add(stringPosition);
            }
        }

        public string this[int index] { get { return this._strings[index]; } }

        public int Count => _positions.Count;

        public int StringCount => _strings.Count;

        private void EnsureSorted()
        {
            if (_dirty)
            {
                this._positions.Sort(Compare);
                this._dirty = false;
            }
        }

        public IEnumerable<StringPosition> FindAll(string keyword)
        {
            var idx = IndexOf(keyword);
            while ((idx >= 0) && (idx < this._positions.Count)
                && (Compare(keyword, this._positions[idx]) == 0))
            {
                yield return this._positions[idx];
                idx++;
            }
        }

        private int IndexOf(string keyword)
        {
            EnsureSorted();

            int minP = 0;
            int maxP = this._positions.Count - 1;
            while (maxP > minP)
            {
                int midP = minP + ((maxP - minP) / 2);
                if (Compare(keyword, this._positions[midP]) > 0)
                {
                    minP = midP + 1;
                }
                else
                {
                    maxP = midP;
                }
            }
            if ((maxP == minP) && (Compare(keyword, this._positions[minP]) == 0))
            {
                return minP;
            }
            else
            {
                return -1;
            }
        }

        private int Compare(StringPosition pos1, StringPosition pos2)
        {
            int len = Math.Max(this._strings[pos1.ListIndex].Length - pos1.StringIndex, this._strings[pos2.ListIndex].Length - pos2.StringIndex);
            return string.Compare(_strings[pos1.ListIndex], pos1.StringIndex, this._strings[pos2.ListIndex], pos2.StringIndex, len, _ignoreCase);
        }

        private int Compare(string keyword, StringPosition pos2)
        {
            return string.Compare(keyword, 0, this._strings[pos2.ListIndex], pos2.StringIndex, keyword.Length, this._ignoreCase);
        }

        public struct StringPosition
        {
            public static StringPosition NotFound = new StringPosition(-1, 0);
//            private readonly int _position;
            private int _listIndex;
            private byte _stringIndex;

            public StringPosition(int listIndex, byte stringIndex)
            {
                _listIndex = listIndex;
                _stringIndex = stringIndex;
//                this._position = (listIndex < 0) ? -1 : this._position = (listIndex << 8) | stringIndex;
//                this._position = (listIndex < 0) ? -1 : (listIndex << 8) | stringIndex;
//                var ld = (this._position >= 0) ? (this._position >> 8) : -1;
//                var la = (byte)(this._position & 0xFF);
            }

            public int ListIndex
            {
                get => _listIndex;
                set => _listIndex = value;
            }

            public byte StringIndex
            {
                get => _stringIndex;
                set => _stringIndex = value;
            }

//            public int ListIndex { get { return (this._position >= 0) ? (this._position >> 8) : -1; } }
//            public byte StringIndex { get { return (byte)(this._position & 0xFF); } }
            public override string ToString()
            {
                return ListIndex.ToString() + ":" + StringIndex;
            }
        }
    }
}