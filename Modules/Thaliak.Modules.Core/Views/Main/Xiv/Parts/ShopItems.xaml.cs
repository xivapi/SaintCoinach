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

using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Modules.Core.Views.Main.Xiv.Parts {
    /// <summary>
    /// Interaction logic for ItemShopsView.xaml
    /// </summary>
    public partial class ShopItems : UserControl {
        public ShopItems() {
            InitializeComponent();
        }

        public object Header {
            get { return _Expander.Header; }
            set { _Expander.Header = value; }
        }
        public bool IsExpanded {
            get { return _Expander.IsExpanded; }
            set { _Expander.IsExpanded = value; }
        }
    }
}
