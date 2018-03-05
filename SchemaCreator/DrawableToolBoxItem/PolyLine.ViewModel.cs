using SchemaCreator.Designer.DrawingPart;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.UI.ViewModel
{
    public class PolyLineViewModel : DrawingItemViewModel
    {
        private Point _lastPoint;
        private Pen _pen;
        private double _thickness;
        private bool _readFirstTime = true;
        private PointCollection _drawingPoints;
        readonly DrawingVisual drawingVisual = new DrawingVisual();

        public PointCollection DrawingPoints
        {
            get => _drawingPoints;
            set
            {
                _drawingPoints = value;
                Set(ref _drawingPoints, value);
            }
        }

        public PolyLineViewModel() : base()
        {
            DrawingPoints = new PointCollection();
            _pen = new Pen();
            _pen.Brush = new SolidColorBrush(Colors.White);
            _thickness = 2;
            _pen.Thickness = _thickness;
            _pen.StartLineCap = PenLineCap.Round;
            _pen.EndLineCap = PenLineCap.Round;
        }

        public override void DrawAdorner(DrawingContext drawingContext,
                                         Point startPoint,
                                         Point endPoint)
        {
            if(_readFirstTime)
            {
                DrawingPoints.Add(startPoint);
                _lastPoint = startPoint;
                _readFirstTime = false;
            }
            DrawingPoints.Add(endPoint);

            drawingContext.DrawLine(_pen, _lastPoint, endPoint);
            _lastPoint = endPoint;
        }
    }
}
