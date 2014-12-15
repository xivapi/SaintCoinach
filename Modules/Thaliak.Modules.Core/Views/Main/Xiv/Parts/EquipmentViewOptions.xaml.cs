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
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;

namespace Thaliak.Modules.Core.Views.Main.Xiv.Parts {
    /// <summary>
    /// Interaction logic for EquipmentViewOptions.xaml
    /// </summary>
    public partial class EquipmentViewOptions : UserControl {
        public EquipmentViewOptions() {
            InitializeComponent();

            _StainCombo.ItemsSource = ServiceLocator.Current.GetInstance<SaintCoinach.Xiv.XivCollection>().GetSheet<SaintCoinach.Xiv.Stain>();
        }

        private void View_Click(object sender, RoutedEventArgs e) {
            var eq = DataContext as SaintCoinach.Xiv.Items.Equipment;

            int matVersion;
            int characterType;
            switch (_GenderCombo.SelectedItem.ToString()) {
                case "M":
                    characterType = 0101;
                    break;
                case "F":
                default:
                    characterType = 0201;
                    break;
            }

            var mdl = eq.GetModel(characterType, out matVersion);
            if (mdl != null) {
                var subMdl = mdl.GetSubModel(0);
                var component = new SaintCoinach.Graphics.Model(subMdl);
                component.SetMaterialVersion(matVersion);

                var stain = _StainCombo.SelectedItem as SaintCoinach.Xiv.Stain;
                if (stain != null)
                    component.SetMaterialStain(stain.Key);

                var evt = ServiceLocator.Current.GetInstance<IEventAggregator>().GetEvent<Events.GraphicsViewRequestEvent>();
                evt.Publish(new Events.GraphicsViewRequestArguments {
                    Component = component,
                    Title = eq.Name
                });
            }
        }
    }
}
