using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class EquipSlot {
        // XXX: Might change on updates, who knows!
        const int AddonKeyOffset = 738;

        #region Static
        private static Dictionary<int, Tuple<string, int>> ModelNameFormats = new Dictionary<int, Tuple<string, int>> {
            {  0, Tuple.Create("chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl", 2) },      // MH
            {  1, Tuple.Create("chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl", 2) },      // OH
            {  2, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_met.mdl", 1) },                // Head
            {  3, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_top.mdl", 1) },                // Body
            {  4, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_glv.mdl", 1) },                // Hands
            {  5, null },    // Waist (has nothing)
            {  6, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_dwn.mdl", 1) },                // Legs
            {  7, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_sho.mdl", 1) },                // Feet
            {  8, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_ear.mdl", 1) },    // Ears
            {  9, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_nek.mdl", 1) },    // Neck
            { 10, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_wrs.mdl", 1) },    // Wrists
            { 11, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_rir.mdl", 1) },    // R.Ring
            { 12, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_ril.mdl", 1) },    // L.Ring
            { 13, null },    // Soul crystal (has nothing)
        };
        #endregion

        #region Fields
        private int _Key;
        private Collections.EquipSlotCollection _Collection;
        #endregion

        #region Properties
        public int Key { get { return _Key; } }
        public Collections.EquipSlotCollection Collection { get { return _Collection; } }
        public string ModelNameFormat {
            get {
                Tuple<string, int> fmt;
                if (!ModelNameFormats.TryGetValue(Key, out fmt) || fmt == null)
                    return null;
                return fmt.Item1;
            }
        }
        public string Name {
            get {
                return Collection.Collection.GetSheet<Addon>()[AddonKeyOffset + Key].Text;
            }
        }
        #endregion

        #region Constructor
        protected internal EquipSlot(Collections.EquipSlotCollection collection, int key) {
            _Key = key;
            _Collection = collection;
        }
        #endregion

        #region Model
        private static readonly Dictionary<int, int> CharacterTypeFallback = new Dictionary<int, int> {
            { 0201, 0101 },
            { 0301, 0101 },
            { 0401, 0201 },
            { 0501, 0101 },
            { 0601, 0201 },
            { 0701, 0101 },
            { 0801, 0201 },
            { 0901, 0101 },
            { 1001, 0201 },
            { 1101, 0101 },
            { 1201, 0201 },
        };
        public const int DefaultCharacterType = 0101;
        public Graphics.Assets.Model GetModel(long key, out int materialVersion) {
            return GetModel(key, DefaultCharacterType, out materialVersion);
        }
        public Graphics.Assets.Model GetModel(long key, int characterType, out int materialVersion) {
            materialVersion = 0;

            Tuple<string, int> format;
            if (!ModelNameFormats.TryGetValue(this.Key, out format))
                return null;
            if (format == null)
                return null;

            var a = (key) & 0xFFFF;
            var b = (key >> 16) & 0xFFFF;
            var c = (key >> 32) & 0xFFFF;
            var d = (key >> 48) & 0xFFFF;
            materialVersion = (int)((key >> (format.Item2 * 16)) & 0xFFFF);

            IO.File file;
            var pack = Collection.Collection.PackCollection;
            while (!pack.TryGetFile(string.Format(format.Item1, a, b, c, d, characterType), out file)) {
                if (!CharacterTypeFallback.TryGetValue(characterType, out characterType))
                    return null;
            }

            return ((Graphics.Assets.ModelFile)file).GetModel();
        }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
