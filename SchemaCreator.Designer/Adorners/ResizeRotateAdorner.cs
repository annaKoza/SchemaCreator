using SchemaCreator.Designer.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class ResizeRotateAdorner : Adorner
    {
        private ResizeRotateChrome chrome;
        private VisualCollection visuals;

        public ResizeRotateAdorner(ContentControl designerItem) : base(designerItem)
        {
            SnapsToDevicePixels = true;
            chrome = new ResizeRotateChrome
            {
                DataContext = designerItem
            };
            visuals = new VisualCollection(this)
            {
                chrome
            };
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            chrome.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => visuals[index];
        protected override int VisualChildrenCount => visuals.Count;
    }
}