﻿using SchemaCreator.Designer.Services;
using SchemaCreator.Designer.UserControls;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerPanel : ItemsControl, ISelectionPanel
    {
        public DesignerPanel()
        {
            Focusable = true;
        }
        private SelectionService _selectionService;
        public SelectionService SelectionService => _selectionService ?? (_selectionService = new SelectionService());

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            SelectionService.ClearSelection();
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DesignerItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is DesignerItem);
        }
        
    }

   
}
