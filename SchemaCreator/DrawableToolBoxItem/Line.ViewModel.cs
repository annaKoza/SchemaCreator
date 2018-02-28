using System;
using System.Windows;
using System.Windows.Media;
using SchemaCreator.Designer.DrawingPart;

namespace SchemaCreator.UI.ViewModel
{
    public class LineViewModel : DrawingItemViewModel
    {
        public override void DrawAdorner(DrawingContext drawingContext, Point startPoint, Point endPoint )
        {
            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 2), startPoint, endPoint);
        }
    }
}