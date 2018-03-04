using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerPanel : ListBox
    {
        public bool SnapItemToGrid
        {
            get { return (bool)GetValue(SnapItemToGridProperty); }
            set { SetValue(SnapItemToGridProperty, value); }
        }
        public static readonly DependencyProperty SnapItemToGridProperty =
            DependencyProperty.Register("SnapItemToGrid", typeof(bool), typeof(DesignerPanel), new PropertyMetadata(true));

        public DesignerPanel()
        {
            Focusable = true;
        }

        protected override DependencyObject GetContainerForItemOverride() => new DesignerItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is DesignerItem;
    }
}