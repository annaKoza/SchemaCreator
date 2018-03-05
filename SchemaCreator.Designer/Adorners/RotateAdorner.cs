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
            var scaleFactor = GetCurrentScaleFactor();

            if(_visuals != null)
            {
                _chrome.LayoutTransform = scaleFactor.Inverse as Transform;
            }
            return base.GetDesiredTransform(transform);
        }

        private Transform GetCurrentScaleFactor()
        {
            var p = AdornedElement.GetVisualParent<DesignerPanel>();
            var dc = p.DataContext as DesignerViewModel;

            return dc.PanelSettings.Transform;
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