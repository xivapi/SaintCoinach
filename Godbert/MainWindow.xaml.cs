using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Godbert {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var settings = Settings.Default;
            if (settings.MainWindowWidth > 0)
                Width = Settings.Default.MainWindowWidth;
            if (settings.MainWindowHeight > 0)
                Height = Settings.Default.MainWindowHeight;
            if (settings.MainWindowLeft > 0)
                Left = Settings.Default.MainWindowLeft;
            if (settings.MainWindowTop > 0)
                Top = Settings.Default.MainWindowTop;
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);

            Settings.Default.MainWindowHeight = Height;
            Settings.Default.MainWindowWidth = Width;
            Settings.Default.MainWindowLeft = Left;
            Settings.Default.MainWindowTop = Top;
        }
    }

}
