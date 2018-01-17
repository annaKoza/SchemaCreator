using SchemaCreator.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SchemaCreator.UI.Resources.Selectors
{
    public class MenuStyleSelector : StyleSelector
    {
        public Style StyleMenuSeparator { get; set; }
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is MenuSeparator)
            {
                return StyleMenuSeparator;
            }
            return base.SelectStyle(item, container);
        }
    }
}
