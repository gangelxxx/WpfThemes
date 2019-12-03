using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ThemesLib.InStat.Helpers
{
    public static class Helpler
    {
//        public static T FindVisualParent<T>(this DependencyObject sender, string name = null) where T : DependencyObject
//        {
//            if (sender == null)
//            {
//                return null;
//            }
//
//            var parent = VisualTreeHelper.GetParent(sender) as FrameworkElement;
//
//            if (!string.IsNullOrEmpty(name))
//            {
//                if (parent is T && !string.IsNullOrEmpty(name) && parent.Name == name)
//                {
//                    return VisualTreeHelper.GetParent(sender) as T;
//                }
//            }
//            else
//            {
//                if (parent is T)
//                {
//                    return VisualTreeHelper.GetParent(sender) as T;
//                }
//            }
//
//
//            return FindVisualParent<T>(parent);
//        }

//        public static T FindChild<T>(this DependencyObject parent, string name = "") where T : DependencyObject
//        {
//            if (parent == null)
//            {
//                return default(T);
//            }
//
//            T foundChild = default(T);
//
//            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
//
//            for (int i = 0; i < childrenCount; i++)
//            {
//                var child = VisualTreeHelper.GetChild(parent, i);
//
//                T childType = child as T;
//                if (childType != null)
//                {
//                    if (string.IsNullOrEmpty(name))
//                    {
//                        foundChild = childType;
//                        return foundChild;
//                    }
//
//                    var frameworkElement = child as FrameworkElement;
//                    if (frameworkElement != null && frameworkElement.Name == name)
//                    {
//                        foundChild = childType;
//                        return foundChild;
//                    }
//                }
//
//                foundChild = FindChild<T>(child, name);
//                if (foundChild != null) break;
//            }
//
//            return foundChild;
//        }

        //public static T FindChildByName<T>(this DependencyObject sender, string name) where T : FrameworkElement
        //{
        //    if (sender == null)
        //    {
        //        return null;
        //    }

        //    var t = typeof(T);

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(sender);
        //    for (int i = 0; i < childrenCount; i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(sender, i);
        //        var ast = child as T;

        //        if (ast == null && VisualTreeHelper.GetChildrenCount(child) > 0)
        //        {
        //            var res = FindChildByName<T>(child, name);
        //            if (res != null)
        //            {
        //                return res;
        //            }
        //        }

        //        if (ast != null && ast.Name == name)
        //        {
        //            return ast;
        //        }
        //    }

        //    return null;
        //}

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(this Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                {
                    ch = stringBuilder[0];
                    break;
                }
                default:
                {
                    ch = stringBuilder[0];
                    break;
                }
            }
            return ch;
        }

        public static SolidColorBrush ToSolidColorBrush(this string str)
        {
            return (SolidColorBrush) ToBrush(str);
        }

        public static Brush ToBrush(this string str)
        {
            var brushConverter = new BrushConverter();
            var brush = (SolidColorBrush)brushConverter.ConvertFrom(str);
            if (brush != null)
                return brush;

            throw new Exception("Не удалось конвертировать строку " + str + " в SolidColorBrush");
        }

    }
}