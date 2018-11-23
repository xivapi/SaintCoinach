using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Ookii.Dialogs.Wpf;

namespace Godbert {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            if (!RequestGamePath()) {
                MainWindow = null;
                Shutdown(1);
                return;
            }

            SaintCoinach.Graphics.Viewer.Interop.HavokInterop.InitializeMTA();

            base.OnStartup(e);

            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e) {
            Settings.Default.Save();
        }

        #region Startup

        private static bool RequestGamePath() {
            string path = Godbert.Properties.Settings.Default.GamePath;
            if (!IsValidGamePath(path)) {
                string programDir;
                if (Environment.Is64BitProcess)
                    programDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                else
                    programDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

                path = System.IO.Path.Combine(programDir, "SquareEnix", "FINAL FANTASY XIV - A Realm Reborn");

                if (IsValidGamePath(path)) {
                    var msgResult = System.Windows.MessageBox.Show(string.Format("Found game installation at \"{0}\". Is this correct?", path), "Confirm game installation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    if (msgResult == MessageBoxResult.Yes) {
                        Godbert.Properties.Settings.Default.GamePath = path;
                        Godbert.Properties.Settings.Default.Save();

                        return true;
                    }

                    path = null;
                }
            }

            VistaFolderBrowserDialog dlg = null;

            while (!IsValidGamePath(path)) {
                var result = (dlg ?? (dlg = new VistaFolderBrowserDialog {
                    Description = "Please select the directory of your FFXIV:ARR game installation (should contain 'boot' and 'game' directories).",
                    ShowNewFolderButton = false,
                })).ShowDialog();

                if (!result.GetValueOrDefault(false)) {
                    var msgResult = System.Windows.MessageBox.Show("Cannot continue without a valid game installation, quit the program?", "That's no good", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);
                    if (msgResult == MessageBoxResult.Yes)
                        return false;
                }

                path = dlg.SelectedPath;
            }

            Godbert.Properties.Settings.Default.GamePath = path;
            Godbert.Properties.Settings.Default.Save();
            return true;
        }
        public static bool IsValidGamePath(string path) {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (!Directory.Exists(path))
                return false;

            return File.Exists(Path.Combine(path, "game", "ffxivgame.ver"));
        }
        #endregion
    }
}
