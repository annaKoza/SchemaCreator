using SchemaCreator.Designer.Common;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IBaseChooseAbleItem
    {
        SelectedItemType SelectedItemType
        {
            get;
        }

        void DrawAdorner(DrawingContext drawingContext,
                         Point startPoint,
                         Point endPoint);
    }
}