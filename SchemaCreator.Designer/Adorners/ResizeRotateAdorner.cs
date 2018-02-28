using SchemaCreator.Designer.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class ResizeRotateAdorner : Adorner
    {
        private ResizeRotateChrome _chrome;
        private VisualCollection _visuals;

        public ResizeRotateAdorner(ContentControl designerItem) : base(designerItem)
        {
            SnapsToDevicePixels = true;
            _chrome = new ResizeRotateChrome
            {
                DataContext = designerItem
            };
            _visuals = new VisualCollection(this)
            {
                _chrome
            };
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _chrome.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => _visuals[index];
        protected override int VisualChildrenCount => _visuals.Count;
    }
}