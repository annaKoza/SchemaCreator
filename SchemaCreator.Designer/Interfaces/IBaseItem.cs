using SchemaCreator.Designer.Common;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IBaseChoosableItem
    {
        SelectedItemType SelectedItemType { get; }
        void DrawAdorner(DrawingContext drawingContext, Point startPoint, Point endPoint);
    }
}