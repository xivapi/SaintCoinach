using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
        ///     File name containing the current version string.
        /// </summary>
        private const string VersionFile = "ffxivgame.ver";

        /// <summary>
        ///     <see cref="Encoding" /> to use inside the <see cref="ZipFile" />.
        /// </summary>
        private static readonly Encoding ZipEncoding = Encoding.UTF8;
        
        private const string DefinitionUrl = "https://github.com/xivdev/EXDSchema/releases/latest/download/{0}.zip";

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

        private readonly HttpClient _httpClient;

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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gamePath">Directory path to the game installation.</param>
        /// <param name="language">Initial language to use.</param>
        public ARealmReversed(string gamePath, Language language) : this(new DirectoryInfo(gamePath), language, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gamePath">Directory path to the game installation.</param>
        /// <param name="storePath">Path to the file used for storing definitions and history.</param>
        /// <param name="language">Initial language to use.</param>
        public ARealmReversed(string gamePath, string storePath, Language language) : this(new DirectoryInfo(gamePath), language, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gamePath">Directory path to the game installation.</param>
        /// <param name="storePath">Path to the file used for storing definitions and history.</param>
        /// <param name="language">Initial language to use.</param>
        /// <param name="libraPath">Path to the Libra Eorzea database file.</param>
        public ARealmReversed(string gamePath, string storePath, Language language, string libraPath) : this(new DirectoryInfo(gamePath), language, new FileInfo(libraPath)) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ARealmReversed" /> class.
        /// </summary>
        /// <param name="gameDirectory">Directory of the game installation.</param>
        /// <param name="storeFile">File used for storing definitions and history.</param>
        /// <param name="language">Initial language to use.</param>
        /// <param name="libraFile">Location of the Libra Eorzea database file, or <c>null</c> if it should not be used.</param>
        public ARealmReversed(DirectoryInfo gameDirectory, Language language, FileInfo libraFile) {

            // Fix for being referenced in a .Net Core 2.1+ application (https://stackoverflow.com/questions/50449314/ibm437-is-not-a-supported-encoding-name => https://stackoverflow.com/questions/44659499/epplus-error-reading-file)
            // PM> dotnet add package System.Text.Encoding.CodePages
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _GameDirectory = gameDirectory;
            _Packs = new PackCollection(Path.Combine(gameDirectory.FullName, "game", "sqpack"));
            _GameData = new XivCollection(Packs, libraFile) {
                ActiveLanguage = language
            };

            _httpClient = new HttpClient();

            _GameVersion = File.ReadAllText(Path.Combine(gameDirectory.FullName, "game", "ffxivgame.ver"));
            _GameData.Definition = ReadDefinition();
            _GameData.Definition.Compile();
        }

        #endregion

        #region Shared

        private RelationDefinition ReadDefinition()
        {
            var definitionPath = "Definitions";
            var versionDirs = Directory.GetDirectories(definitionPath).ToList();
            if (versionDirs.Count == 0)
                throw new DirectoryNotFoundException($"No definition directories found in {definitionPath}.");
            
            var versionDirToUse = versionDirs.Where(v => v.Contains(GameVersion)).FirstOrDefault(versionDirs.Last());
            var version = Path.GetFileName(versionDirToUse);
            
            var def = new RelationDefinition() { Version = version };
            foreach (var sheetFileName in Directory.EnumerateFiles(versionDirToUse, "*.yml")) {
                var json = File.ReadAllText(Path.Combine(sheetFileName), Encoding.UTF8);
                var sheet = SerializeUtil.Deserialize<Sheet>(json);
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

        public bool IsUpdateAvailable()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, string.Format(DefinitionUrl, GameVersion));
                var response = _httpClient.SendAsync(request).Result;

                var latestPath = Path.Combine("Definitions", "latest.zip");

                if (!File.Exists(latestPath))
                    return response.IsSuccessStatusCode;

                var latestBytes = File.ReadAllBytes(latestPath);
                var latestMd5 = MD5.HashData(latestBytes);

                var md5 = response.Content.Headers.GetValues("Content-MD5").FirstOrDefault();
                var md5Bytes = Convert.FromBase64String(md5);

                if (latestMd5.SequenceEqual(md5Bytes))
                    return false;

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return false;
            }
        }

        public static void TestThing()
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Head, string.Format(DefinitionUrl, "2023.10.23.0000.0000"));
            var response = client.Send(request);
            Console.WriteLine("hey");
            var success = response.IsSuccessStatusCode;
            Console.WriteLine(response);
        }

        public void UpdateDefinition()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(DefinitionUrl, GameVersion));
            var response = _httpClient.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download definition for version {GameVersion}.");
            }
            
            using var stream = response.Content.ReadAsStream();
            using var zip = ZipFile.Read(stream);
            zip.Save(Path.Combine("Definitions", "latest.zip"));
            Directory.CreateDirectory(Path.Combine("Definitions", GameVersion));
            zip.ExtractAll("Definitions", ExtractExistingFileAction.OverwriteSilently);
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
