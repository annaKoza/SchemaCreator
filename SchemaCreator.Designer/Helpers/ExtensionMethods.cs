using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.Helpers
{
    internal static class ExtensionMethods
    {
        public static double NearestFactor(this double Value, double Factor) => Math.Round(Value /
                Factor,
                                                                                           MidpointRounding.AwayFromZero) *
            Factor;

        public static int RemoveAll<T>(this ObservableCollection<T> collection,
                                       Func<T, bool> condition)
        {
            var itemsToRemove = collection.Where(condition).ToList();

            foreach(var itemToRemove in itemsToRemove)
            {
                collection.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        internal static T GetVisualParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            while(true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);

                if(parentObject == null) return null;

                if(parentObject is T parent)
                    return parent;

                child = parentObject;
            }
        }

        internal static T GetVisualChild<T>(this DependencyObject parent)
            where T : Visual
        {
            var child = default(T);

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for(var i = 0; i < childrenCount; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);

                if(child != null)
                {
                    break;
                }
            }
            return child;
        }

        internal static IEnumerable<T> GetVisualChildren<T>(this DependencyObject parent)
            where T : Visual
        {
            if(parent != null)
            {
                for(int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent,
                                                                       i);
                    if(child != null && child is T)
                        yield return (T)child;

                    foreach(T childOfChild in GetVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}