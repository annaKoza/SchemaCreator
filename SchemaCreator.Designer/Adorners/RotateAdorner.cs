using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class RotateAdorner : Adorner
    {
        private RotateChrome _chrome;
        private VisualCollection _visuals;
        private ContentControl _designerItem;

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var matrix = transform as MatrixTransform;

            if (_visuals == null || matrix == null) return base.GetDesiredTransform(transform);
            var vec1 = new Vector(matrix.Matrix.M11, matrix.Matrix.M12).Length;
            var  vec2 = new Vector(matrix.Matrix.M21, matrix.Matrix.M22).Length;
            _chrome.LayoutTransform = new ScaleTransform(1 / vec1, 1 / vec2);
            return base.GetDesiredTransform(transform);
        }
        
        protected override int VisualChildrenCount => _visuals.Count;

        public RotateAdorner(ContentControl designerItem) : base(designerItem)
        {
            SnapsToDevicePixels = true;
            _designerItem = designerItem;
            _chrome = new RotateChrome() { DataContext = designerItem };
            _visuals = new VisualCollection(this)
            {
                _chrome
            };
        }

        protected override Visual GetVisualChild(int index) => _visuals[index];

        protected override Size ArrangeOverride(Size finalSize)
        {
            _chrome.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
            return finalSize;
        }
    }
}