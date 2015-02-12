using System;
using System.Collections.Generic;

using SaintCoinach.Graphics.Assets;
using SaintCoinach.IO;
using SaintCoinach.Xiv.Collections;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing an equipment slot.
    /// </summary>
    /// <remarks>
    ///     Equipment slots are not present in the game data, this class exists to make things nicer in code.
    /// </remarks>
    public class EquipSlot {
        #region Static

        /// <summary>
        ///     Row key offset in the <c>Addon</c> sheet to get an equipment slot's name from.
        /// </summary>
        /// <remarks>
        ///     This might change in updates, you never know with SE.
        /// </remarks>
        private const int AddonKeyOffset = 738;

        /// <summary>
        ///     Mappings of <see cref="EquipSlot" />s to the formats used to get the file name of models.
        /// </summary>
        /// <remarks>
        ///     First item in the values is the format string, the second value is which Word in a QWord is the material variant to
        ///     be used.
        ///     Parameters 0-3 in the format string are the respective Words in the supplied QWord, parameter 4 is the character
        ///     type.
        /// </remarks>
        private static readonly Dictionary<int, Tuple<string, int>> ModelNameFormats =
            new Dictionary<int, Tuple<string, int>> {
                {
                    0, Tuple.Create("chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl", 2)
                }, // MH
                {
                    1, Tuple.Create("chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl", 2)
                }, // OH
                {
                    2, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_met.mdl", 1)
                }, // Head
                {
                    3, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_top.mdl", 1)
                }, // Body
                {
                    4, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_glv.mdl", 1)
                }, // Hands
                {
                    5, null
                }, // Waist (has nothing)
                {
                    6, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_dwn.mdl", 1)
                }, // Legs
                {
                    7, Tuple.Create("chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_sho.mdl", 1)
                }, // Feet
                {
                    8, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_ear.mdl", 1)
                }, // Ears
                {
                    9, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_nek.mdl", 1)
                }, // Neck
                {
                    10, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_wrs.mdl", 1)
                }, // Wrists
                {
                    11, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_rir.mdl", 1)
                }, // R.Ring
                {
                    12, Tuple.Create("chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_ril.mdl", 1)
                }, // L.Ring
                {
                    13, null
                } // Soul crystal (has nothing)
            };

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the key of the <see cref="EquipSlot" />.
        /// </summary>
        /// <remarks>
        ///     The keys are based on the columns in <see cref="EquipSlotCategory" />.
        /// </remarks>
        /// <value>The key of the <see cref="EquipSlot" />.</value>
        public int Key { get; private set; }

        /// <summary>
        ///     Gets the <see cref="EquipSlotCollection" />.
        /// </summary>
        /// <value>The <see cref="EquipSlotCollection" />.</value>
        public EquipSlotCollection Collection { get; private set; }

        /// <summary>
        ///     Gets the format string for models for this <see cref="EquipSlot" />.
        /// </summary>
        /// <value>The format string for models for this <see cref="EquipSlot" />; or <c>null</c> if the slot has no models.</value>
        public string ModelNameFormat {
            get {
                Tuple<string, int> fmt;
                if (!ModelNameFormats.TryGetValue(Key, out fmt) || fmt == null)
                    return null;
                return fmt.Item1;
            }
        }

        /// <summary>
        ///     Gets the name of the <see cref="EquipSlot" />.
        /// </summary>
        /// <value>The name of the <see cref="EquipSlot" />.</value>
        public string Name { get { return Collection.Collection.GetSheet<Addon>()[AddonKeyOffset + Key].Text; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EquipSlot" /> class.
        /// </summary>
        /// <param name="collection"><see cref="EquipSlotCollection" /> for this <see cref="EquipSlot" />.</param>
        /// <param name="key">Key of the <see cref="EquipSlot" />.</param>
        protected internal EquipSlot(EquipSlotCollection collection, int key) {
            Key = key;
            Collection = collection;
        }

        #endregion

        /// <summary>
        ///     Returns a string that represents the current <see cref="EquipSlot" />.
        /// </summary>
        /// <returns>Returns the value of <see cref="Name" />.</returns>
        public override string ToString() {
            return Name;
        }

        #region Model

        /// <summary>
        ///     Character type fallbacks in case the requested one does not exist.
        /// </summary>
        private static readonly Dictionary<int, int> CharacterTypeFallback = new Dictionary<int, int> {
            {
                0201, 0101
            }, {
                0301, 0101
            }, {
                0401, 0201
            }, {
                0501, 0101
            }, {
                0601, 0201
            }, {
                0701, 0101
            }, {
                0801, 0201
            }, {
                0901, 0101
            }, {
                1001, 0201
            }, {
                1101, 0101
            }, {
                1201, 0201
            }
        };

        /// <summary>
        ///     Default character type to use.
        /// </summary>
        public const int DefaultCharacterType = 0101;

        /// <summary>
        ///     Get the model for a specific QWord and the current <see cref="EquipSlot" /> using the default character type.
        /// </summary>
        /// <param name="key">The identifier of the model.</param>
        /// <param name="materialVersion">When this method returns, contains the variant contained within <c>key</c>.</param>
        /// <returns>Returns the <see cref="Model" /> for the specified <c>key</c> and default character type.</returns>
        public Model GetModel(long key, out int materialVersion) {
            return GetModel(key, DefaultCharacterType, out materialVersion);
        }

        /// <summary>
        ///     Get the model for a specific QWord, character type, and the current <see cref="EquipSlot" />.
        /// </summary>
        /// <param name="key">The identifier of the model.</param>
        /// <param name="characterType">Character type to get the model for.</param>
        /// <param name="materialVersion">When this method returns, contains the variant contained within <c>key</c>.</param>
        /// <returns>Returns the <see cref="Model" /> for the specified <c>key</c> and <c>characterType</c>.</returns>
        public Model GetModel(long key, int characterType, out int materialVersion) {
            materialVersion = 0;

            Tuple<string, int> format;
            if (!ModelNameFormats.TryGetValue(Key, out format))
                return null;
            if (format == null)
                return null;

            var a = key & 0xFFFF;
            var b = (key >> 16) & 0xFFFF;
            var c = (key >> 32) & 0xFFFF;
            var d = (key >> 48) & 0xFFFF;
            materialVersion = (int)((key >> (format.Item2 * 16)) & 0xFFFF);

            File file;
            var pack = Collection.Collection.PackCollection;
            while (!pack.TryGetFile(string.Format(format.Item1, a, b, c, d, characterType), out file)) {
                if (!CharacterTypeFallback.TryGetValue(characterType, out characterType))
                    return null;
            }

            return ((ModelFile)file).GetModel();
        }

        #endregion
    }
}
