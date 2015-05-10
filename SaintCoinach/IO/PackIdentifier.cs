using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.IO {
    public struct PackIdentifier {
        #region Static
        public const string DefaultExpansion = "ffxiv";

        public static readonly Dictionary<string, byte> TypeToKeyMap =
            new Dictionary<string, byte> {
                { "common", 0x00 },
                { "bgcommon", 0x01 },
                { "bg", 0x02 }, 
                { "cut", 0x03 }, 
                { "chara", 0x04 },
                { "shader", 0x05 }, 
                { "ui", 0x06 }, 
                { "sound", 0x07 },
                { "vfx", 0x08 }, 
                /*{ "ui_script", 0x09 },*/
                { "exd", 0x0a },
                { "game_script", 0x0b },
                { "music", 0x0c },
                { "_sqpack_test", 0x12 }, 
                { "_debug", 0x13 },
            };

        public static readonly Dictionary<byte, string> KeyToTypeMap = TypeToKeyMap.ToDictionary(_ => _.Value, _ => _.Key);

        public static readonly Dictionary<string, byte> ExpansionToKeyMap =
            new Dictionary<string, byte> {
                { "ffxiv", 0x00 },
                { "ex1", 0x01 },
            };
        public static readonly Dictionary<byte, string> KeyToExpansionMap = ExpansionToKeyMap.ToDictionary(_ => _.Value, _ => _.Key);
        #endregion

        private readonly string _Type;
        private readonly byte _TypeKey;
        private readonly string _Expansion;
        private readonly byte _ExpansionKey;
        private readonly byte _Number;

        public string Type { get { return _Type; } }
        public byte TypeKey { get { return _TypeKey; } }
        public string Expansion { get { return _Expansion; } }
        public byte ExpansionKey { get { return _ExpansionKey; } }
        public byte Number { get { return _Number; } }

        public PackIdentifier(byte type, byte expansion, byte number) {
            _TypeKey = type;
            _ExpansionKey = expansion;
            _Number = number;

            _Type = KeyToTypeMap[type];
            _Expansion = KeyToExpansionMap[expansion];
        }
        public PackIdentifier(string type, string expansion, byte number) {
            _Type = type;
            _Expansion = expansion;
            _Number = number;

            _TypeKey = TypeToKeyMap[type];
            _ExpansionKey = ExpansionToKeyMap[expansion];
        }

        public override int GetHashCode() {
            return TypeKey << 16 | ExpansionKey << 8 | Number;
        }
        public override bool Equals(object obj) {
            if (obj is PackIdentifier)
                return Equals((PackIdentifier)obj);
            return false;
        }
        public bool Equals(PackIdentifier other) {
            return other.TypeKey == this.TypeKey && other.ExpansionKey == this.ExpansionKey && other.Number == this.Number;
        }

        public static PackIdentifier Get(string fullPath) {
            PackIdentifier id;
            if (!TryGet(fullPath, out id))
                throw new ArgumentException();
            return id;
        }
        public static bool TryGet(string fullPath, out PackIdentifier value) {
            value = default(PackIdentifier);

            var typeSep = fullPath.IndexOf('/');
            if (typeSep <= 0)
                return false;
            var type = fullPath.Substring(0, typeSep);
            if (!TypeToKeyMap.ContainsKey(type))
                return false;

            var expSep = fullPath.IndexOf('/', typeSep + 1);

            string expansion = null;
            byte number = 0;
            if (expSep > typeSep) {
                expansion = fullPath.Substring(typeSep + 1, expSep - typeSep - 1);
                var numberEnd = fullPath.IndexOf('_', expSep);
                if (numberEnd - expSep == 3) {
                    if (!byte.TryParse(
                        fullPath.Substring(expSep + 1, 2),
                        System.Globalization.NumberStyles.HexNumber | System.Globalization.NumberStyles.AllowHexSpecifier,
                        System.Globalization.CultureInfo.CurrentCulture,
                        out number))
                        number = 0;
                }
            }

            if (expansion == null || !ExpansionToKeyMap.ContainsKey(expansion))
                expansion = DefaultExpansion;

            value = new PackIdentifier(type, expansion, number);
            return true;
        }
    }
}
