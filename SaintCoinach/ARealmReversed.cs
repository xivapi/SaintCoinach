using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Newtonsoft.Json;
using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.Ex.Relational.Definition.EXDSchema;
using SaintCoinach.IO;
using SaintCoinach.Xiv;

using Directory = System.IO.Directory;
using File = System.IO.File;
using SerializeUtil = SaintCoinach.Ex.Relational.Definition.EXDSchema.SerializeUtil;
using Sheet = SaintCoinach.Ex.Relational.Definition.EXDSchema.Sheet;

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

        private RelationDefinition ReadDefinition()
        {
            var definitionPath = "Definitions";
            var versionDirs = Directory.GetDirectories(definitionPath).ToList();
            var versionDirToUse = versionDirs.Last();
            var version = Path.GetFileName(versionDirToUse);
            
            var def = new RelationDefinition() { Version = version };
            foreach (var sheetFileName in Directory.EnumerateFiles(versionDirToUse, "*.yml")) {
                var json = File.ReadAllText(Path.Combine(sheetFileName), Encoding.UTF8);
                var sheet = SerializeUtil.Deserialize<Sheet>(json);
                // sheet = SchemaUtil.PostProcess(sheet);
                sheet = SchemaUtil.Flatten(sheet);
                var sheetDef = SheetDefinition.FromYaml(sheet);
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

        #endregion
    }
}
