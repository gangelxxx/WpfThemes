#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

#endregion

namespace KeysBinding
{
    /// <summary>
    /// Содержит клавиши
    /// Биндит когда нужно
    /// Сохраняет клавиши бинда
    /// Загружает клавиши бинда
    /// </summary>
    public class KeyProvider : Singleton<KeyProvider>
    {
        #region Private fields

        private ConcurrentDictionary<string, IKeySet> _keySets = new ConcurrentDictionary<string, IKeySet>();
        public IProperties Properties { get; set; }
        public Func<IKeySet, bool> ActivateCallBack { get; set; }
        public Func<string, UIElement> getUiElementByNameCallBack { get; set; }

        #endregion

        #region Construct

        public KeyProvider(IEnumerable<IKeySet> keySets, IProperties properties)
        {
            if (keySets == null) throw new ArgumentNullException(nameof(keySets));
            foreach (IKeySet keySet in keySets)
            {
                _keySets.TryAdd(keySet.Name, keySet);
            }
            if(properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            Properties = properties;
        }

        private KeyProvider()
        {}

        #endregion

        #region Public

        public void AddKeySet(IKeySet keySet)
        {
            if (_keySets.ContainsKey(keySet.Name))
            {
                IKeySet _;
                _keySets.TryRemove(keySet.Name, out _);
            }

            _keySets.TryAdd(keySet.Name, keySet);
        }

        public void KeySetActivate(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (!_keySets.ContainsKey(name)) throw new Exception("_keySets not contains key " + name);

            var keySet = _keySets[name];
            if (keySet.UiElement == null)
            {
                keySet.UiElement = getUiElementByNameCallBack.Invoke(name);
            }

            ActivateCallBack.Invoke(keySet);
        }

        public IKeySet GetKeySetByName(string name)
        {
            if (_keySets.ContainsKey(name))
            {
                return _keySets[name];
            }

            return null;
        }

        public void Load()
        {
            _keySets.Clear();

            if (Properties == null)
            {
                return;
            }

            string loadStr = Properties.Load();
            if (string.IsNullOrEmpty(loadStr))
            {
                return;
            }

            string[] res = loadStr.Split(';');

            foreach (string item in res)
            {
                IKeySet keySet = KeySet.FromString(item);
                if (keySet == null)
                {
                    continue;
                }

                var element = getUiElementByNameCallBack.Invoke(keySet.Name);
                if (element == null)
                {
                    continue;
                }

                keySet.UiElement = element;
                _keySets.TryAdd(keySet.Name, keySet);
            }
        }

        public void Save()
        {
            Properties.Save(ToString());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, IKeySet> pair in _keySets)
            {
                sb.Append(pair.Value.ToString());
                sb.Append(";");
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public void Clear()
        {
            _keySets.Clear();
        }


        #endregion
    }
}