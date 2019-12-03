#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

#endregion

namespace KeysBinding
{
    public class KeySet : IKeySet
    {
        #region Private fields

        private readonly ConcurrentDictionary<string, Key> _keys = new ConcurrentDictionary<string, Key>();

        #endregion

        #region Construct

        public KeySet()
        { }

        public KeySet(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("string is empty");
            }

            Name = name;
        }

        #endregion

        #region Public

        public void SetKeys(Func<UIElement, Key, bool> keyCallBack)
        {
            UiElement.InputBindings.Clear();

            foreach (KeyValuePair<string, Key> pair in _keys)
            {
                KeysBinding.Key key = pair.Value;

                if (key.MKey == System.Windows.Input.Key.None)
                {
                    continue;
                }

                keyCallBack.Invoke(UiElement, key);
            }
        }

        public void Add(Key key)
        {
            if (_keys.ContainsKey(key.Name))
            {
                _keys[key.Name] = key;
                return;
            }

            _keys.TryAdd(key.Name, key);
        }

        public Dictionary<string, Key> GetKeys()
        {
            return _keys.ToDictionary(x => x.Key, y => y.Value);
        }

        public void Clear()
        {
            _keys.Clear();
        }

        public static KeySet FromString(string keyBindStr)
        {
            if (!Regex.IsMatch(keyBindStr, "\\w+=.+"))
            {
                return null;
            }

            KeySet keySet = new KeySet();

            string[] keySetStrings = keyBindStr.Split('=');
            if (keySetStrings.Length == 2)
            {
                keySet.Name = keySetStrings[0];
                string[] strs = keySetStrings[1].Split(',');
                foreach (string str in strs)
                {
                    Key key = Key.FromString(str);
                    keySet.Add(key);
                }
            }

            return keySet;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append("=");

            foreach (KeyValuePair<string, Key> pair in _keys)
            {
                sb.Append(pair.Value);
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public UIElement UiElement { get; set; }
        public string Name { get; set; }

        #endregion
    }
}