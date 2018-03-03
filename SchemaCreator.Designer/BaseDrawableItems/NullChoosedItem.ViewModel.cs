using SchemaCreator.Designer.Common;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.BaseDrawableItems
{
    public class NullChoosedItemViewModel : IBaseChooseAbleItem
    {
        public SelectedItemType SelectedItemType => SelectedItemType.None;

        public void DrawAdorner(DrawingContext drawingContext,
                                Point startPoint,
                                Point endPoint)
        {
        }
    }
}
