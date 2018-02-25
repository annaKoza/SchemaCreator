using System.Windows;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Controls
{
    public class ToolBox : ItemsControl
    {
        static ToolBox() => DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToolBox),
                new FrameworkPropertyMetadata(typeof(ToolBox)));

        public Size ItemSize
        {
            get => itemSize;
            set => itemSize = value;
        }

        private Size itemSize = new Size(50, 50);

        protected override DependencyObject GetContainerForItemOverride() => new ToolboxItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is ToolboxItem;
    }
}