using System.Windows;
using System.Windows.Media;
using SchemaCreator.Designer.Common;
using SchemaCreator.Designer.Interfaces;

namespace SchemaCreator.Designer.BaseDrawableItems
{
    public class SelectionViewModel : ISelectionItem
    {
        private Pen _rubberbandPen;
        public SelectionViewModel()
        {
            _rubberbandPen = new Pen(Brushes.LightSlateGray, 1)
            {
                DashStyle = new DashStyle(new double[] { 2 }, 1)
            };
        }

        public SelectedItemType SelectedItemType => SelectedItemType.SelectItem;

        public void DrawAdorner(DrawingContext drawingContext, Point startPoint, Point endPoint)
        {
            drawingContext.DrawRectangle(Brushes.Transparent, _rubberbandPen, new Rect(startPoint, endPoint));
        }
    }
}