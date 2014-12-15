using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel.Composition;

namespace Thaliak.Modules.Core.ToolbarItems {
    /// <summary>
    /// Interaction logic for RecipeBasketToolbarItem.xaml
    /// </summary>
    [Behaviors.ViewExport(RegionName = RegionNames.MainToolBarRegion)]
    public partial class RecipeBasketToolbarItem : UserControl {
        public RecipeBasketToolbarItem() {
            InitializeComponent();
        }

        [Import]
        private Services.IObjectStore ObjectStore { get; set; }
        [Import]
        private RecipeBasketViewModel ViewModel {
            get { return (RecipeBasketViewModel)DataContext; }
            set { DataContext = value; }
        }

        protected override void OnDragEnter(DragEventArgs e) {
            base.OnDragEnter(e);

            var o = ObjectStore.TryGetDragObject(e);
            e.Effects = o is SaintCoinach.Xiv.InventoryItem ? DragDropEffects.Link : DragDropEffects.None;
        }
        protected override void OnDrop(DragEventArgs e) {
            base.OnDrop(e);

            var o = ObjectStore.TryGetDragObject(e);
            if (o is SaintCoinach.Xiv.InventoryItem)
                ViewModel.AddItem((SaintCoinach.Xiv.InventoryItem)o);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Right)
                ViewModel.ClearItems();
            else
                ViewModel.Navigate();
        }
        protected override void OnTouchUp(TouchEventArgs e) {
            base.OnTouchUp(e);

            ViewModel.Navigate();
        }
    }
}
