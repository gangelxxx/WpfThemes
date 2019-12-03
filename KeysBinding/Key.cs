using System;
using System.Windows.Input;
using InputKey = System.Windows.Input.Key;

namespace KeysBinding
{
    public class Key
    {
        public Key(System.Windows.Input.Key mKey, string name, ModifierKeys modifierKey = ModifierKeys.None)
        {
            MKey = mKey;
            Name = name;
            ModifierKey = modifierKey;
        }

        public InputKey MKey { get; }
        public ModifierKeys ModifierKey { get; }
        public string Name { get; }

        public static Key FromString(string keyString)
        {
            string[] split = keyString.Split('$');
            if (split.Length != 3)
            {
                throw new Exception("Input string " + keyString + " not match key");
            }

            var key = (InputKey) Convert.ToInt32(split[0]);
            var modif = (ModifierKeys) Convert.ToInt32(split[1]);
            var name = split[2];

            return new Key(key, name, modif);
        }

        public override string ToString()
        {
            return ((int) MKey) + "$" + ((int) ModifierKey) + "$" + Name;
        }
    }
}