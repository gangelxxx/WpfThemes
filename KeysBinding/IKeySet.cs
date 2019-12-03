using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace KeysBinding
{
    public interface IKeySet
    {
        UIElement UiElement { get; set; }
        string Name { get; set; }

        Dictionary<string, Key> GetKeys();
        void SetKeys(Func<UIElement, Key, bool> keyCallBack);
        void Add(Key key);
        void Clear();
        string ToString();
    }
}