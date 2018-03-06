using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class ResizeRotateAdorner : BaseAdorner
    {
        internal ResizeRotateAdorner(UIElement itemsPanel) : base(itemsPanel, new ResizeRotateChrome() { DataContext = itemsPanel })
        {
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Chrome.Arrange(new Rect(finalSize));
            return finalSize;
        }

    }
}