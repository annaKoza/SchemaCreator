using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerCanvas : Canvas
    {
        private SelectionService _selectionService;
        public SelectionService SelectionService
        {
            get =>
                _selectionService ?? (_selectionService = new SelectionService());
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (e.Source != this) return;
            SelectionService.ClearSelection();
            Focus();
            e.Handled = true;
        }
    }
}