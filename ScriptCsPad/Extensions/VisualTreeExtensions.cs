using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ScriptCsPad.Extensions
{
    public static class VisualTreeExtensions
    {
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                yield return VisualTreeHelper.GetChild(parent, i);
            }
        }
    }
}