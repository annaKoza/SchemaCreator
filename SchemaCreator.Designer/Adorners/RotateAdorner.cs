using SchemaCreator.Designer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
            }
        }

        public RotateAdorner(ContentControl designerItem)
            : base(designerItem)
        {
            SnapsToDevicePixels = true;
            _designerItem = designerItem;
            _chrome = new RotateChrome() { DataContext = designerItem };
            _visuals = new VisualCollection(this)
            {
                _chrome
            };
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _chrome.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
            return finalSize;
        }
    }
}

