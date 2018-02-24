using System.Windows;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerPanel : ItemsControl
    {
        public DesignerPanel()
        {
            Focusable = true;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DesignerItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DesignerItem;
        }
    }
}