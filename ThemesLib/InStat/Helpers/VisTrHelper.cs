using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ThemesLib.InStat.Helpers
{
    public static class VisTrHelper
    {
        #region Public

//        public static T TemplateFindName<T>(this DependencyObject parent, string name = "") where T : DependencyObject
//        {
//            var res = FindChild<T>(parent, name);
//            if (res == null)
//                ThreadPool.QueueUserWorkItem(_ => throw new NullReferenceException("FindChildEx: " + typeof(T) + (string.IsNullOrEmpty(name) ? "" : " by " + name) + " not found"));
//
//            return res;
//        }

        //        _ellipse = (Ellipse) Template.FindName("PART_EllipseContent", this);
        //            if (_ellipse == null)
        //        throw new NullReferenceException(nameof(ColorComboBox) + " PART_EllipseContent not found");

        public static T FindChildEx<T>(this DependencyObject parent, string name = "") where T : DependencyObject
        {
            var res = FindChild<T>(parent, name);
            if (res == null)
                ThreadPool.QueueUserWorkItem(_ => throw new NullReferenceException("FindChildEx: " + typeof(T) + (string.IsNullOrEmpty(name) ? "" : " by " + name) + " not found"));

            return res;
        }

        public static T FindNameEx<T>(this Control control, string name = "") where T : DependencyObject
        {
            var res = (T) control.Template.FindName(name, control);

            if (res == null)
                ThreadPool.QueueUserWorkItem(_ => throw new NullReferenceException("FindNameEx: " + typeof(T) + (string.IsNullOrEmpty(name) ? "" : " by " + name) + " not found"));

            return res;
        }



        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T variable)
                    return variable;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }

        public static T FindVisualParent<T>(DependencyObject sender) where T : DependencyObject
        {
            if (sender == null)
            {
                return null;
            }

            var parent = VisualTreeHelper.GetParent(sender);

            if (parent is T)
            {
                return VisualTreeHelper.GetParent(sender) as T;
            }

            return FindVisualParent<T>(parent);
        }

        public static T FindChild<T>(this DependencyObject parent, string name = "") where T : DependencyObject
        {
            if (parent == null)
            {
                return default(T);
            }

            var foundChild = default(T);

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                switch (child)
                {
                    case T childType when string.IsNullOrEmpty(name):
                        foundChild = childType;
                        return foundChild;

                    case T childType:
                        if (child is FrameworkElement frameworkElement && frameworkElement.Name == name)
                        {
                            foundChild = childType;
                            return foundChild;
                        }

                        break;
                }

                foundChild = FindChild<T>(child, name);
                if (foundChild != null) break;
            }

            return foundChild;
        }

        public static T FindChildByName<T>(this DependencyObject sender, string name) where T : FrameworkElement
        {
            if (sender == null)
            {
                return null;
            }

            int childrenCount = VisualTreeHelper.GetChildrenCount(sender);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(sender, i);
                var ast = child as T;

                if (ast == null && VisualTreeHelper.GetChildrenCount(child) > 0)
                {
                    var res = FindChildByName<T>(child, name);
                    if (res != null)
                    {
                        return res;
                    }
                }

                if (ast != null && ast.Name == name)
                {
                    return ast;
                }
            }

            return null;
        }

        public static T FindVisualParent<T>(this DependencyObject sender, string name = null) where T : DependencyObject
        {
            if (sender == null)
            {
                return null;
            }

            var parent = VisualTreeHelper.GetParent(sender) as FrameworkElement;

            if (!string.IsNullOrEmpty(name))
            {
                if (parent is T && !string.IsNullOrEmpty(name) && parent.Name == name)
                {
                    return VisualTreeHelper.GetParent(sender) as T;
                }
            }
            else
            {
                if (parent is T)
                {
                    return VisualTreeHelper.GetParent(sender) as T;
                }
            }


            return FindVisualParent<T>(parent);
        }

        public static string GetWebHeaderCollectionValue(this WebHeaderCollection headers, string name)
        {
            for (int i = 0; i < headers.Count; ++i)
                if (headers.Keys[i] == name)
                {
                    return headers[i];
                }

            return null;
        }

        public static void SetBinding(this object source, string path, DependencyObject target, DependencyProperty dependencyProperty)
        {
            Binding binding = new Binding
            {
                Source = source,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(target, dependencyProperty, binding);
        }

        #endregion
    }
}