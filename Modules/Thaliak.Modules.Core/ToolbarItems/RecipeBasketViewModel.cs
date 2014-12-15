using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Mvvm;

namespace Thaliak.Modules.Core.ToolbarItems {
    [Export(typeof(RecipeBasketViewModel))]
    public class RecipeBasketViewModel : BindableBase {
        #region Fields
        private ICollection<SaintCoinach.Xiv.InventoryItem> _Items = new ObservableCollection<SaintCoinach.Xiv.InventoryItem>();
        #endregion

        #region Properties
        public IEnumerable<SaintCoinach.Xiv.InventoryItem> Items { get { return _Items; } }
        public int Count {
            get { return _Items.Count; }
        }
        #endregion

        #region Constructor
        #endregion

        #region Things
        public void AddItem(SaintCoinach.Xiv.InventoryItem item) {
            _Items.Add(item);
            OnPropertyChanged(() => Count);
        }
        public void ClearItems() {
            _Items.Clear();
            OnPropertyChanged(() => Count);
        }
        public void Navigate() {
            // TODO
        }
        #endregion
    }
}
