using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchemaCreator.Designer
{
    class DesignerCanvas : Canvas
    {
        private Point _selectionStartPoint; 
        public DesignerCanvas()
        {
            AllowDrop = true;
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source == this)
                {
                    _selectionStartPoint = e.MouseDevice.GetPosition(this);
                }
            }
        }
    }
}
