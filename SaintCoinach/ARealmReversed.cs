using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ionic.Zip;
using YamlDotNet.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaintCoinach {
    using Ex.Relational.Definition;
    using Ex.Relational.Update;
    
    public class ARealmReversed {
        const string StateFile = "SaintCoinach.History.zip";
        const string DefinitionFile = "ex.yaml";
        const string VersionFile = "ffxivgame.ver";
        const string UpdateReportTextFile = "logs/report-{0}-{1}.log";
        const string UpdateReportYamlFile = "logs/report-{0}-{1}.yaml";
        const string UpdateReportBinFile = "logs/report-{0}-{1}.bin";

        static readonly Encoding ZipEncoding = Encoding.UTF8;
        
        #region Fields
        private DirectoryInfo _GameDirectory;
        private IO.PackCollection _Packs;
        private Xiv.XivCollection _GameData;

        private string _GameVersion;
        private UpdateReport _UpdateReport;
        #endregion

        #region Properties
        public DirectoryInfo GameDirectory { get { return _GameDirectory; } }

        public IO.PackCollection Packs { get { return _Packs; } }
        public Xiv.XivCollection GameData { get { return _GameData; } }

        public string GameVersion { get { return _GameVersion; } }
        public UpdateReport UpdateReport { get { return _UpdateReport; } }
        public bool WasUpdated { get { return _UpdateReport != null; } }
        #endregion

        #region Constructor
        public ARealmReversed(string gamePath, Ex.Language language) : this(new DirectoryInfo(gamePath), language) { }
        public ARealmReversed(DirectoryInfo gameDirectory, Ex.Language language) {
            _GameDirectory = gameDirectory;
            _Packs = new IO.PackCollection(Path.Combine(gameDirectory.FullName, "game", "sqpack", "ffxiv"));
            _GameData = new Xiv.XivCollection(Packs);
            _GameData.ActiveLanguage = language;

            _GameVersion = File.ReadAllText(Path.Combine(gameDirectory.FullName, "game", "ffxivgame.ver"));

            using (var zipFile = new ZipFile(StateFile, ZipEncoding)) {
                if (zipFile.ContainsEntry(VersionFile))
                    _GameData.Definition = CheckForUpdate(zipFile);
                else
                    _GameData.Definition = Setup(zipFile);
            }

            _GameData.Definition.Compile();
        }
        #endregion

        #region Shared
        private void StoreDefinitionHistory(ZipFile zip) {
            ZipCopy(zip, DefinitionFile, string.Format("{0}/{1}", GameVersion, DefinitionFile));
        }
        private void StorePacks(ZipFile zip) {
            const string ExdPackPattern = "0a0000.*";

            foreach (var file in Packs.DataDirectory.EnumerateFiles(ExdPackPattern))
                zip.UpdateFile(file.FullName, GameVersion);
        }
        private void UpdateVersion(ZipFile zip) {
            zip.UpdateEntry(VersionFile, GameVersion);
        }
        private void ZipCopy(ZipFile zip, string source, string target) {
            var entry = zip[source];

            byte[] buffer;
            using (var s = entry.OpenReader()) {
                using (var ms = new MemoryStream()) {
                    s.CopyTo(ms);
                    buffer = ms.ToArray();
                }
            }

            zip.UpdateEntry(target, buffer);
        }
        private RelationDefinition ReadDefinition(ZipFile zip, string entry) {
            RelationDefinition def;

            var zipEntry = zip[entry];
            using (var s = zipEntry.OpenReader()) {
                using (var r = new StreamReader(s, ZipEncoding))
                    def = RelationDefinition.Deserialize(r);
            }

            return def;
        }
        private void StoreDefinition(ZipFile zip, RelationDefinition definition, string path) {
            using (var ms = new MemoryStream()) {
                using (var writer = new StreamWriter(ms, ZipEncoding)) {
                    definition.Serialize(writer);
                    writer.Flush();
                    zip.UpdateEntry(path, ms.ToArray());
                }
            }
        }
        private void StoreReport(ZipFile zip, UpdateReport report) {
            var textTarget = string.Format(UpdateReportTextFile, report.PreviousVersion, report.UpdateVersion);
            zip.UpdateEntry(textTarget, string.Join(Environment.NewLine, report.ExChanges.Select(_ => _.ToString())), ZipEncoding);

            var yamlTarget = string.Format(UpdateReportYamlFile, report.PreviousVersion, report.UpdateVersion);
            var serializer = new Serializer();
            byte[] yamlBuffer;
            using (var ms = new MemoryStream()) {
                using (var writer = new StreamWriter(ms, ZipEncoding)) {
                    serializer.Serialize(writer, report);
                    writer.Flush();
                    yamlBuffer = ms.ToArray();
                }
            }
            zip.UpdateEntry(yamlTarget, yamlBuffer);

            var binTarget = string.Format(UpdateReportBinFile, report.PreviousVersion, report.UpdateVersion);
            var formatter = new BinaryFormatter();
            byte[] binBuffer;
            using (var ms = new MemoryStream()) {
                formatter.Serialize(ms, report);
                binBuffer = ms.ToArray();
            }
            zip.UpdateEntry(binTarget, binBuffer);
        }
        #endregion

        #region Update
        private RelationDefinition CheckForUpdate(ZipFile zip) {
            var previousVersionEntry = zip[VersionFile];
            string previousVersion;
            using (var s = previousVersionEntry.OpenReader()) {
                using (var r = new StreamReader(s))
                    previousVersion = r.ReadToEnd();
            }

            RelationDefinition def;
            if (previousVersion != GameVersion) {
                var existingDef = string.Format("{0}/{1}", GameVersion, DefinitionFile);
                if (zip.ContainsEntry(existingDef)) {
                    System.Diagnostics.Trace.WriteLine("Version changed, but information already present.");

                    ZipCopy(zip, existingDef, DefinitionFile);
                    UpdateVersion(zip);
                    zip.Save();

                    def = ReadDefinition(zip, DefinitionFile);
                } else {
                    def = Update(zip, previousVersion);
                }
            } else {
                System.Diagnostics.Trace.WriteLine(string.Format("Up-to-date (version {0})", GameVersion));
                def = ReadDefinition(zip, DefinitionFile);
            }

            return def;
        }
        private RelationDefinition Update(ZipFile zip, string previousVersion) {
            System.Diagnostics.Trace.WriteLine(string.Format("Update required (from {0} to {1})", previousVersion, GameVersion));

            string tempPath = null;
            RelationDefinition definition;
            try {
                tempPath = ExtractPreviousPacks(zip, previousVersion);
                var previousPack = new IO.PackCollection(Path.Combine(tempPath, previousVersion));
                var previousDefinition = ReadDefinition(zip, DefinitionFile);

                var updater = new RelationUpdater(previousPack, previousDefinition, Packs);

                System.Diagnostics.Trace.WriteLine("Detecting structure changes.");
                var structureChanges = updater.Update();

                foreach (var c in structureChanges.Where(_ => _.ChangeType.HasFlag(ChangeType.Breaking)))
                    Console.Error.WriteLine(c);

                System.Diagnostics.Trace.Write("Detecting data changes.");
                var dataChanges = updater.DetectDataChanges();

                _UpdateReport = new SaintCoinach.UpdateReport(previousVersion, GameVersion, structureChanges.Concat(dataChanges));

                definition = updater.Updated;

                StorePacks(zip);
                StoreDefinition(zip, definition, DefinitionFile);
                StoreDefinition(zip, definition, string.Format("{0}/{1}", GameVersion, DefinitionFile));
                StoreReport(zip, _UpdateReport);
                zip.Save();

                System.Diagnostics.Trace.WriteLine(string.Format("Update complete (version {0})", GameVersion));
            } finally {
                if (tempPath != null) {
                    try { Directory.Delete(tempPath, true); } catch { Console.Error.WriteLine("Failed to delete temporary directory {0}.", tempPath); }
                }
            }

            return definition;
        }
        private string ExtractPreviousPacks(ZipFile zip, string previousVersion) {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            Directory.CreateDirectory(tempPath);

            zip.ExtractSelectedEntries("type = F", previousVersion, tempPath);

            return tempPath;
        }
        #endregion

        #region Setup
        private RelationDefinition Setup(ZipFile zip) {
            System.Diagnostics.Trace.WriteLine("No version information present, performing first-time setup.");
            StoreDefinitionHistory(zip);
            StorePacks(zip);
            UpdateVersion(zip);

            zip.Save();

            System.Diagnostics.Trace.WriteLine(string.Format("First-time setup complete (version {0})", GameVersion));
            return ReadDefinition(zip, DefinitionFile);
        }
        #endregion
    }
}
