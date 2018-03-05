using System.Windows;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerPanel : ListBox
    {
        public bool SnapItemToGrid
        {
            get => (bool)GetValue(SnapItemToGridProperty);
            set => SetValue(SnapItemToGridProperty, value);
        }

        public static readonly DependencyProperty SnapItemToGridProperty =
            DependencyProperty.Register("SnapItemToGrid",
                                        typeof(bool),
                                        typeof(DesignerPanel),
                                        new PropertyMetadata(true));

        public DesignerPanel() => Focusable = true;

        protected override DependencyObject GetContainerForItemOverride() => new DesignerItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is DesignerItem;
    }
}