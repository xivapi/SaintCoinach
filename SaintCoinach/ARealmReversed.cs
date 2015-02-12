using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ionic.Zip;

using YamlDotNet.Serialization;

using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaintCoinach {
    using Ex.Relational.Definition;
    using Ex.Relational.Update;

    /// <summary>
    /// Central class for accessing game assets.
    /// </summary>
    public class ARealmReversed {
        /// <summary>
        /// File name of the archive containing current and past data mappings.
        /// </summary>
        private const string StateFile = "SaintCoinach.History.zip";

        /// <summary>
        /// File name inside the archive of the data mappings.
        /// </summary>
        private const string DefinitionFile = "ex.yaml";

        /// <summary>
        /// File name containg the current version string.
        /// </summary>
        private const string VersionFile = "ffxivgame.ver";

        /// <summary>
        /// Format string to create the file name for update reports in text form. {0} is the previous and {1} the new version.
        /// </summary>
        private const string UpdateReportTextFile = "logs/report-{0}-{1}.log";

        /// <summary>
        /// Format string to create the file name for update reports in YAML form. {0} is the previous and {1} the new version.
        /// </summary>
        private const string UpdateReportYamlFile = "logs/report-{0}-{1}.yaml";

        /// <summary>
        /// Format string to create the file name for update reports in binary form. {0} is the previous and {1} the new version.
        /// </summary>
        private const string UpdateReportBinFile = "logs/report-{0}-{1}.bin";

        private static readonly Encoding ZipEncoding = Encoding.UTF8;

        #region Fields

        /// <summary>
        /// Root directory of the game installation.
        /// </summary>
        private DirectoryInfo _GameDirectory;

        /// <summary>
        /// Pack collection for the data files.
        /// </summary>
        private IO.PackCollection _Packs;

        /// <summary>
        /// Game data collection for the data files.
        /// </summary>
        private Xiv.XivCollection _GameData;

        /// <summary>
        /// Version of the game data.
        /// </summary>
        private string _GameVersion;

        #endregion

        #region Properties

        /// <summary>
        /// Root directory of the game installation.
        /// </summary>
        public DirectoryInfo GameDirectory { get { return _GameDirectory; } }

        /// <summary>
        /// Pack collection for the data files.
        /// </summary>
        public IO.PackCollection Packs { get { return _Packs; } }

        /// <summary>
        /// Game data collection for the data files.
        /// </summary>
        public Xiv.XivCollection GameData { get { return _GameData; } }

        /// <summary>
        /// Version of the game data.
        /// </summary>
        public string GameVersion { get { return _GameVersion; } }

        /// <summary>
        /// Version of the loaded definition.
        /// </summary>
        public string DefinitionVersion { get { return GameData.Definition.Version; } }

        public bool IsCurrentVersion { get { return GameVersion == DefinitionVersion; } }

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
                if (zipFile.ContainsEntry(VersionFile)) {
                    RelationDefinition def;
                    if (!TryGetDefinitionVersion(zipFile, GameVersion, out def))
                        def = ReadDefinition(zipFile);

                    _GameData.Definition = def;
                } else
                    _GameData.Definition = Setup(zipFile);
            }

            _GameData.Definition.Compile();
        }

        #endregion

        #region Shared

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

        private RelationDefinition ReadDefinition(ZipFile zip) {
            return ReadDefinition(zip, DefinitionFile);
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
            zip.UpdateEntry(textTarget, string.Join(Environment.NewLine, report.ExChanges.Select(_ => _.ToString())),
                ZipEncoding);

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

        private bool TryGetDefinitionVersion(ZipFile zip, string version, out RelationDefinition definition) {
            var storedVersionEntry = zip[VersionFile];
            string storedVersion;
            using (var s = storedVersionEntry.OpenReader()) {
                using (var r = new StreamReader(s))
                    storedVersion = r.ReadToEnd();
            }

            if (storedVersion != version) {
                var existingDefPath = string.Format("{0}/{1}", version, DefinitionFile);
                if (zip.ContainsEntry(existingDefPath)) {
                    ZipCopy(zip, existingDefPath, DefinitionFile);
                    UpdateVersion(zip);
                    zip.Save();

                    definition = ReadDefinition(zip);
                    return true;
                }

                definition = null;
                return false;
            }

            definition = ReadDefinition(zip);
            return true;
        }

        public UpdateReport Update(bool detectDataChanges, IProgress<UpdateProgress> progress) {
            System.Diagnostics.Trace.WriteLine(string.Format("Beginning update from {0} to {1}.", DefinitionVersion,
                GameVersion));
            var previousVersion = DefinitionVersion;

            string tempPath = null;
            UpdateReport report;
            try {
                using (var zip = new ZipFile(StateFile, ZipEncoding)) {
                    tempPath = ExtractPreviousPacks(zip, previousVersion);
                    var previousPack = new IO.PackCollection(Path.Combine(tempPath, previousVersion));
                    var previousDefinition = ReadDefinition(zip, DefinitionFile);

                    var updater = new RelationUpdater(previousPack, previousDefinition, Packs, GameVersion, progress);

                    var changes = updater.Update(detectDataChanges);
                    report = new SaintCoinach.UpdateReport(previousVersion, GameVersion, changes);

                    var definition = updater.Updated;

                    StorePacks(zip);
                    StoreDefinition(zip, definition, DefinitionFile);
                    StoreDefinition(zip, definition, string.Format("{0}/{1}", definition.Version, DefinitionFile));
                    StoreReport(zip, report);
                    zip.Save();

                    GameData.Definition = definition;
                    GameData.Definition.Compile();

                    System.Diagnostics.Trace.WriteLine(string.Format("Update complete (version {0})", GameVersion));
                }
            } finally {
                if (tempPath != null) {
                    try {
                        Directory.Delete(tempPath, true);
                    } catch {
                        Console.Error.WriteLine("Failed to delete temporary directory {0}.", tempPath);
                    }
                }
            }
            return report;
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

            var def = ReadDefinition(zip, DefinitionFile);
            if (def.Version != GameVersion)
                System.Diagnostics.Trace.WriteLine(string.Format("Definition and game version mismatch ({0} != {1})",
                    def.Version, GameVersion));

            def.Version = GameVersion;
            StoreDefinition(zip, def, string.Format("{0}/{1}", def.Version, DefinitionFile));
            StorePacks(zip);
            UpdateVersion(zip);

            zip.Save();

            System.Diagnostics.Trace.WriteLine(string.Format("First-time setup complete (version {0})", GameVersion));

            return def;
        }

        #endregion
    }
}
