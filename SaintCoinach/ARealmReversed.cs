using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Ionic.Zip;
using Newtonsoft.Json;
using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex.Relational.Update;
using SaintCoinach.IO;
using SaintCoinach.Xiv;

using Directory = System.IO.Directory;
using File = System.IO.File;

namespace SaintCoinach {
    /// <summary>
    ///     Central class for accessing game assets.
    /// </summary>
    public class ARealmReversed {
        #region Static

        /// <summary>
        ///     Default file name of the archive containing current and past data mappings.
        /// </summary>
        private const string DefaultStateFile = "SaintCoinach.History.zip";

        /// <summary>
        ///     File name containing the current version string.
        /// </summary>
        private const string VersionFile = "ffxivgame.ver";

        /// <summary>
        ///     Format string to create the file name for update reports in text form. {0} is the previous and {1} the new version.
        /// </summary>
        private const string UpdateReportTextFile = "logs/report-{0}-{1}.log";

        /// <summary>
        ///     Format string to create the file name for update reports in JSON form. {0} is the previous and {1} the new version.
        /// </summary>
        private const string UpdateReportJsonFile = "logs/report-{0}-{1}.json";

        /// <summary>
        ///     Format string to create the file name for update reports in binary form. {0} is the previous and {1} the new
        ///     version.
        /// </summary>
        private const string UpdateReportBinFile = "logs/report-{0}-{1}.bin";

        /// <summary>
        ///     <see cref="Encoding" /> to use inside the <see cref="ZipFile" />.
        /// </summary>
        private static readonly Encoding ZipEncoding = Encoding.UTF8;

        #endregion

        #region Fields

        /// <summary>
        ///     Game data collection for the data files.
        /// </summary>
        private readonly XivCollection _GameData;

        /// <summary>
        ///     Root directory of the game installation.
        /// </summary>
        private readonly DirectoryInfo _GameDirectory;

        /// <summary>
        ///     Version of the game data.
        /// </summary>
        private readonly string _GameVersion;

        /// <summary>
        ///     Pack collection for the data files.
        /// </summary>
        private readonly PackCollection _Packs;

        /// <summary>
        ///     Archive file containing current and past data mappings. 
        /// </summary>
        private readonly FileInfo _StateFile;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the root directory of the game installation.
        /// </summary>
        /// <value>The root directory of the game installation.</value>
        public DirectoryInfo GameDirectory { get { return _GameDirectory; } }

        /// <summary>
        ///     Gets the pack collection for the data files.
        /// </summary>
        /// <value>The pack collection for the data files.</value>
        public PackCollection Packs { get { return _Packs; } }

        /// <summary>
        ///     Gets the game data collection for the data files.
        /// </summary>
        /// <value>The game data collection for the data files.</value>
        public XivCollection GameData { get { return _GameData; } }

        /// <summary>
        ///     Gets the version of the game data.
        /// </summary>
        /// <value>The version of the game data.</value>
        public string GameVersion { get { return _GameVersion; } }

        /// <summary>
        ///     Gets the version of the loaded definition.
        /// </summary>
        /// <value>The version of the loaded definition.</value>
        public string DefinitionVersion { get { return GameData.Definition.Version; } }

        /// <summary>
        ///     Gets a value indicating whether the loaded definition is the same as the game data version.
        /// </summary>
        /// <value>Whether the loaded definition is the same as the game data version.</value>
        public bool IsCurrentVersion { get { return GameVersion == DefinitionVersion; } }

        /// <summary>
        ///     Gets the archive file containing current and past data mappings.
        /// </summary>
        /// <value>The archive file containing current and past data mappings.</value>
        public FileInfo StateFile { get { return _StateFile; } }

        #endregion

        #region Setup

        /// <summary>
        ///     Perform first-time setup on the archive.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> used for storage.</param>
        /// <returns>Returns the initial <see cref="RelationDefinition" /> object.</returns>
        private void Setup(ZipFile zip) {
            var def = _GameData.Definition;
            if (def.Version != GameVersion)
                System.Diagnostics.Trace.WriteLine(string.Format("Definition and game version mismatch ({0} != {1})", def.Version, GameVersion));

            def.Version = GameVersion;
            StoreDefinitionInZip(zip, def);
            StorePacks(zip);
            UpdateVersion(zip);

            zip.Save();
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gamePath">Directory path to the game installation.</param>
        /// <param name="language">Initial language to use.</param>
        public ARealmReversed(string gamePath, Language language) : this(new DirectoryInfo(gamePath), new FileInfo(DefaultStateFile), language, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gamePath">Directory path to the game installation.</param>
        /// <param name="storePath">Path to the file used for storing definitions and history.</param>
        /// <param name="language">Initial language to use.</param>
        public ARealmReversed(string gamePath, string storePath, Language language) : this(new DirectoryInfo(gamePath), new FileInfo(storePath), language, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gamePath">Directory path to the game installation.</param>
        /// <param name="storePath">Path to the file used for storing definitions and history.</param>
        /// <param name="language">Initial language to use.</param>
        /// <param name="libraPath">Path to the Libra Eorzea database file.</param>
        public ARealmReversed(string gamePath, string storePath, Language language, string libraPath) : this(new DirectoryInfo(gamePath), new FileInfo(storePath), language, new FileInfo(libraPath)) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gameDirectory">Directory of the game installation.</param>
        /// <param name="storeFile">File used for storing definitions and history.</param>
        /// <param name="language">Initial language to use.</param>
        /// <param name="libraFile">Location of the Libra Eorzea database file, or <c>null</c> if it should not be used.</param>
        public ARealmReversed(DirectoryInfo gameDirectory, FileInfo storeFile, Language language, FileInfo libraFile) {

            // Fix for being referenced in a .Net Core 2.1+ application (https://stackoverflow.com/questions/50449314/ibm437-is-not-a-supported-encoding-name => https://stackoverflow.com/questions/44659499/epplus-error-reading-file)
            // PM> dotnet add package System.Text.Encoding.CodePages
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _GameDirectory = gameDirectory;
            _Packs = new PackCollection(Path.Combine(gameDirectory.FullName, "game", "sqpack"));
            _GameData = new XivCollection(Packs, libraFile) {
                ActiveLanguage = language
            };

            _GameVersion = File.ReadAllText(Path.Combine(gameDirectory.FullName, "game", "ffxivgame.ver"));
            _StateFile = storeFile;
            _GameData.Definition = ReadDefinition();

            using (var zipFile = new ZipFile(StateFile.FullName, ZipEncoding)) {
                if (!zipFile.ContainsEntry(VersionFile))
                    Setup(zipFile);
            }

            _GameData.Definition.Compile();
        }

        #endregion

        #region Shared

        private RelationDefinition ReadDefinition() {
            var versionPath = Path.Combine("Definitions", "game.ver");
            if (!File.Exists(versionPath))
                throw new InvalidOperationException("Definitions\\game.ver must exist.");

            var version = File.ReadAllText(versionPath).Trim();
            var def = new RelationDefinition() { Version = version };
            foreach (var sheetFileName in Directory.EnumerateFiles("Definitions", "*.json")) {
                var json = File.ReadAllText(Path.Combine(sheetFileName), Encoding.UTF8);
                var obj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
                var sheetDef = SheetDefinition.FromJson(obj);
                def.SheetDefinitions.Add(sheetDef);

                if (!_GameData.SheetExists(sheetDef.Name)) {
                    var msg = $"Defined sheet {sheetDef.Name} is missing.";
                    Debug.WriteLine(msg);
                    Console.WriteLine(msg);
                }
            }

            return def;
        }

        /// <summary>
        ///     Deserialize a <see cref="RelationDefinition" /> file inside a <see cref="ZipFile" />.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> to read from.</param>
        /// <param name="version">Version of the definition to read.</param>
        /// <returns>Returns the read <see cref="RelationDefinition" />.</returns>
        private static RelationDefinition ReadDefinition(ZipFile zip, string version) {
            var def = new RelationDefinition() { Version = version };
            var entries = zip.SelectEntries("*.json", Path.Combine(version, "Definitions"));
            foreach (var entry in entries) {
                using (var stream = entry.OpenReader())
                using (var reader = new StreamReader(stream)) {
                    var json = reader.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
                    def.SheetDefinitions.Add(SheetDefinition.FromJson(obj));
                }
            }
            return def;
        }

        /// <summary>
        ///     Store the current pack files in storage.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> to store the current packs in.</param>
        private void StorePacks(ZipFile zip) {
            const string ExdPackPattern = "0a*.*";

            foreach (var file in Packs.DataDirectory.EnumerateFiles(ExdPackPattern, SearchOption.AllDirectories)) {
                string targetDir = GameVersion + "/" + file.Directory.Name;
                zip.UpdateFile(file.FullName, targetDir);
            }
        }

        /// <summary>
        ///     Updating the current version string in storage.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> to store the version string in.</param>
        private void UpdateVersion(ZipFile zip) {
            zip.UpdateEntry(VersionFile, GameVersion);
        }

        /// <summary>
        ///     Serialize a <see cref="RelationDefinition" /> into a <see cref="ZipFile" />.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> to write to.</param>
        /// <param name="definition"><see cref="RelationDefinition" /> to store.</param>
        /// <param name="version">Version these definitions are for.</param>
        private static void StoreDefinitionInZip(ZipFile zip, RelationDefinition definition) {
            // Since this method is only ever called to update the zip with the
            // lateset definitions, store these for both the given version *and*
            // root.

            // todo: prior to storage, delete everything under "Definitions" to prevent
            // dead sheets from resurrecting.

            var versionBasePath = Path.Combine(definition.Version, "Definitions");
            foreach (var sheetDef in definition.SheetDefinitions) {
                var json = SheetToJson(sheetDef);
                var sheetFileName = sheetDef.Name + ".json";
                zip.UpdateEntry(Path.Combine(versionBasePath, sheetFileName), json);
                zip.UpdateEntry(Path.Combine("Definitions", sheetFileName), json);
            }

            // Store version in root definition path for quick copying.
            var versionPath = Path.Combine("Definitions", "game.ver");
            zip.UpdateEntry(versionPath, definition.Version);
        }

        private static void StoreDefinitionOnFilesystem(RelationDefinition definition, string basePath) {
            foreach (var sheetDef in definition.SheetDefinitions) {
                var sheetDefPath = Path.Combine(basePath, "Definitions", sheetDef.Name + ".json");
                File.WriteAllText(sheetDefPath, SheetToJson(sheetDef));
            }

            var versionPath = Path.Combine(basePath, "Definitions", "game.ver");
            File.WriteAllText(versionPath, definition.Version);
        }

        private static string SheetToJson(SheetDefinition sheetDef) {
            var obj = sheetDef.ToJson();
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        ///     Store a <see cref="UpdateReport" /> in a <see cref="ZipFile" />.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> to write to.</param>
        /// <param name="report"><see cref="UpdateReport" /> to store.</param>
        private static void StoreReport(ZipFile zip, UpdateReport report) {
            var textTarget = string.Format(UpdateReportTextFile, report.PreviousVersion, report.UpdateVersion);
            zip.UpdateEntry(textTarget, string.Join(Environment.NewLine, report.Changes.Select(_ => _.ToString())),
                ZipEncoding);

            var jsonTarget = string.Format(UpdateReportJsonFile, report.PreviousVersion, report.UpdateVersion);
            var obj = report.ToJson();
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            zip.UpdateEntry(jsonTarget, json);
        }

        #endregion

        #region Update

        /// <summary>
        ///     Update to the current version.
        /// </summary>
        /// <param name="detectDataChanges">Boolean indicating whether the update should also look for changes in data.</param>
        /// <param name="progress">Optional object to which update progress is reported.</param>
        /// <returns>Returns the <see cref="UpdateReport" /> containing all changes.</returns>
        /// <exception cref="InvalidOperationException">Definition is up-to-date.</exception>
        public UpdateReport Update(bool detectDataChanges, IProgress<UpdateProgress> progress = null) {
            if (DefinitionVersion == GameVersion)
                throw new InvalidOperationException();

            var previousVersion = DefinitionVersion;

            var exdPackId = new PackIdentifier("exd", PackIdentifier.DefaultExpansion, 0);
            var exdPack = Packs.GetPack(exdPackId);
            var exdOldKeepInMemory = exdPack.KeepInMemory;
            exdPack.KeepInMemory = true;

            string tempPath = null;
            UpdateReport report;
            try {
                using (var zip = new ZipFile(StateFile.FullName, ZipEncoding)) {
                    tempPath = ExtractPacks(zip, previousVersion);
                    var previousPack = new PackCollection(Path.Combine(tempPath, previousVersion));
                    previousPack.GetPack(exdPackId).KeepInMemory = true;

                    RelationDefinition previousDefinition;
                    if (previousVersion == _GameData.Definition.Version) {
                        // Override previous definition when current definition version matches.
                        // Definitions may have changed since this was recorded and we want to compare that.
                        previousDefinition = _GameData.Definition;

                    } else {
                        // Otherwise, read the previous definition from the zip.
                        previousDefinition = ReadDefinition(zip, previousVersion);
                    }

                    var updater = new RelationUpdater(previousPack, previousDefinition, Packs, GameVersion, progress);

                    var changes = updater.Update(detectDataChanges);
                    report = new UpdateReport(previousVersion, GameVersion, changes);

                    var definition = updater.Updated;

                    StorePacks(zip);
                    StoreDefinitionInZip(zip, definition);
                    StoreDefinitionOnFilesystem(definition, "");

                    if (Debugger.IsAttached) {
                        // Little QOL path - when updating with the debugger attached,
                        // also write to the project definitions path so no need to copy
                        // them manually afterward.
                        var projectDefinitionsPath = "../../../SaintCoinach";
                        if (Directory.Exists(projectDefinitionsPath))
                            StoreDefinitionOnFilesystem(definition, projectDefinitionsPath);
                    }

                    StoreReport(zip, report);
                    UpdateVersion(zip);
                    zip.Save();

                    GameData.Definition = definition;
                    GameData.Definition.Compile();
                }
            } finally {
                if (exdPack != null)
                    exdPack.KeepInMemory = exdOldKeepInMemory;
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

        /// <summary>
        ///     Extract the packs of a specific version from storage into a temporary directory.
        /// </summary>
        /// <param name="zip"><see cref="ZipFile" /> to read from.</param>
        /// <param name="previousVersion">Version of the packs to extract.</param>
        /// <returns>Returns the path to the directory containing the extracted packs.</returns>
        private static string ExtractPacks(ZipFile zip, string previousVersion) {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            Directory.CreateDirectory(tempPath);

            foreach (var entry in zip.Entries.Where(e => e.FileName.StartsWith(previousVersion)))
                    entry.Extract(tempPath);

            return tempPath;
        }

        #endregion
    }
}
