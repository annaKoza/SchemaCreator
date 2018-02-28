using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SchemaCreator.Designer.Controls
{
    public class RotateChrome : Control
    {
        static RotateChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RotateChrome), new FrameworkPropertyMetadata(typeof(RotateChrome)));
        }
    }
}
