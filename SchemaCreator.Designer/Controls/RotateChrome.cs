using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Controls
{
    public class RotateChrome : Control
    {
        static RotateChrome() => DefaultStyleKeyProperty.OverrideMetadata(typeof(RotateChrome),
                                                                          new FrameworkPropertyMetadata(typeof(RotateChrome)));
    }
}
