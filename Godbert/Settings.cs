using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert {
    public class Settings {
        #region IO Boilerplate
        public static Settings Default { get; } = Load();

        private static string FileName => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Godbert", "settings.json");

        private static Settings Load() {
            try {
                if (File.Exists(FileName)) {
                    var text = File.ReadAllText(FileName);
                    return JsonConvert.DeserializeObject<Settings>(text) ?? new Settings();
                }
            } catch (Exception) {
                // Error reading settings.  Return default.
            }

            return new Settings();
        }

        public void Save() {
            var path = Path.GetDirectoryName(FileName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            try {
                File.WriteAllText(FileName, text);
            } catch (IOException) {
                // Error saving settings.  Ignore.
            }
        }
        #endregion

        public double MainWindowLeft;
        public double MainWindowTop;
        public double MainWindowWidth;
        public double MainWindowHeight;

        public string SelectedSheetName;
        public string FilterSheetTerm;
        public string FilterDataTerm;
        public bool ShowOffsets;
        public bool SortByOffsets;
    }
}
