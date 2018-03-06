using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class SizeAdorner : BaseAdorner
    {
        public SizeAdorner(ContentControl designerItem) : base(designerItem, new SizeChrome() { DataContext = designerItem })
        {
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            Chrome.Arrange(new Rect(new Point(0.0, 0.0),
                                     new Size(finalSize.Width,
                                              finalSize.Height)));
            return finalSize;
        }
    }
}