using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class RotateAdorner : BaseAdorner
    {

        public RotateAdorner(ContentControl designerItem) : base(designerItem, new RotateChrome() { DataContext = designerItem })
        {
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            Chrome.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
            return finalSize;
        }
    }
}