﻿using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Services
{
    public class SelectionService
    {
        private IEnumerable<ISelectable> _selectedItems;
        internal IEnumerable<ISelectable> SelectedItems
        {
            get => 
                _selectedItems ?? (_selectedItems = new List<ISelectable>() );
        }
        
        internal void SelectItem(ISelectable item)
        {
           ClearSelection();
           AddToSelection(item);
        }

        internal void ClearSelection()
        {
            (SelectedItems as List<ISelectable>).ForEach(item => item.IsSelected = false);
            (_selectedItems as List<ISelectable>).Clear();
        }

        internal void AddToSelection(ISelectable item)
        {
            (_selectedItems as List<ISelectable>).Add(item);
            item.IsSelected = true;
        }

        internal void RemoveFromSelection(ISelectable designerItem)
        {
            (SelectedItems as List<ISelectable>).Remove(designerItem);
            designerItem.IsSelected = false;
        }
        
    }
}
