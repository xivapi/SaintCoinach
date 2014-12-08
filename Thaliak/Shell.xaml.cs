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
using System.Windows.Shapes;

using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak {
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : MahApps.Metro.Controls.MetroWindow {
        public Shell() {
            InitializeComponent();
        }

        [Import]
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
        ShellViewModel ViewModel {
            set {
                this.DataContext = value;
            }
        }

    }
}
