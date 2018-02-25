using System;
using System.Windows;

namespace SchemaCreator.Designer
{
    internal class DragObject
    {
        internal Type DataContextType
        {
            get; set;
        }

        internal Size? DesiredSize
        {
            get; set;
        }
    }
}