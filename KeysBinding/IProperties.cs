using System.Collections.Generic;
using System.Windows;

namespace KeysBinding
{
    public interface IProperties
    {
        void Save(string str);
        string Load();
    }
}