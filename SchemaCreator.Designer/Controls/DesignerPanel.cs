using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerPanel : ListBox
    {
        public DesignerPanel() => Focusable = true;

        protected override DependencyObject GetContainerForItemOverride() => new DesignerItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is DesignerItem;
    }
}