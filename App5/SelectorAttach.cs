using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI.ViewManagement;
namespace App5
{
    public class SelectorAttach
    {
        public static SelectionMode GetSelectionMode(DependencyObject obj)
        {
            return (SelectionMode)obj.GetValue(SelectionModeProperty);
        }

        public static void SetSelectionMode(DependencyObject obj, SelectionMode value)
        {
            obj.SetValue(SelectionModeProperty, value);
        }

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.RegisterAttached("SelectionMode", typeof(SelectionMode), typeof(SelectorAttach), new PropertyMetadata(SelectionMode.Single, OnSelectionModeChanged));

        private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selectorBar = d as SelectorBar;
            var selectionMode = (SelectionMode)e.NewValue;
            void SetMode()
            {
                ItemsView itemsView = FindChild<ItemsView>(selectorBar, "PART_ItemsView");

                if (itemsView != null)
                {
                    switch (selectionMode)
                    {
                        case SelectionMode.Single:
                            itemsView.SelectionMode = ItemsViewSelectionMode.Single;
                            break;
                        case SelectionMode.Multiple:
                            itemsView.SelectionMode = ItemsViewSelectionMode.Multiple;
                            break;
                        case SelectionMode.Extended:
                            itemsView.SelectionMode = ItemsViewSelectionMode.Extended;
                            break;
                    }
                }
            }

            if (selectorBar != null)
            {
                selectorBar.Loaded -= (sender, args) => { };
                selectorBar.Loaded += (sender, args) =>
                {
                    SetMode();
                };
                SetMode();
            }
        }

        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T childType && (childType as FrameworkElement)?.Name == childName)
                {
                    foundChild = childType;
                    break;
                }

                foundChild = FindChild<T>(child, childName);

                if (foundChild != null) break;
            }

            return foundChild;
        }
    }
}
