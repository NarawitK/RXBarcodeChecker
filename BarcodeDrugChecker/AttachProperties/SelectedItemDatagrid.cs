using BarcodeDrugCheckerLib.Model.Interface;
using System.Windows;
using System.Windows.Controls;

namespace BarcodeDrugChecker.AttachProperties
{
    public class SelectingItemDatagrid
    {
        public static readonly DependencyProperty SelectingItemProperty = DependencyProperty.RegisterAttached(
            "SelectingItem",
            typeof(IDrugVisit),
            typeof(SelectingItemDatagrid),
            new PropertyMetadata(default(IDrugVisit), OnSelectingItemChanged));

        public static IDrugVisit GetSelectingItem(DependencyObject target)
        {
            return (IDrugVisit)target.GetValue(SelectingItemProperty);
        }

        public static void SetSelectingItem(DependencyObject target, IDrugVisit value)
        {
            target.SetValue(SelectingItemProperty, value);
        }

        static void OnSelectingItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if ((grid != null || grid.SelectedItem != null) && grid.Items.Count > 0)
            {
                // Works with .Net 4.5
                grid.Dispatcher.InvokeAsync(() =>
                {
                    grid.UpdateLayout();
                    grid.ScrollIntoView(grid.SelectedItem, null);
                });

                // Works with .Net 4.0
                /*
                grid.Dispatcher.BeginInvoke((Action)(() =>
                {
                    grid.UpdateLayout();
                    grid.ScrollIntoView(grid.SelectedItem, null);
                }));
                */
            }
        }
    }
}
