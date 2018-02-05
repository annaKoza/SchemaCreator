using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.Helpers
{
   internal static class ExtensionMethods
    {
        internal static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);

                if (parentObject == null) return null;

                if (parentObject is T parent)
                    return parent;

                child = parentObject;
            }
        }

        internal static T GetVisualChild<T>(this DependencyObject parent) where T : Visual
        {
            T child = default(T);

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);

                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
