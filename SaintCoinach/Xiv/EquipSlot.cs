using System;
using System.Collections.Generic;

using SaintCoinach.Graphics;
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

        #region Model helper class
        private class ModelHelper {
            #region Fields
            public string ImcFileFormat { get; private set; }
            public byte ImcPartKey { get; private set; }
            public string ModelFileFormat { get; private set; }
            public byte VariantIndexWord { get; private set; }
            #endregion

            public ModelHelper(string imcFileFormat, byte imcPartKey, string modelFileFormat, byte variantIndexWord) {
                this.ImcFileFormat = imcFileFormat;
                this.ImcPartKey = imcPartKey;
                this.ModelFileFormat = modelFileFormat;
                this.VariantIndexWord = variantIndexWord;
            }
        }
        #endregion

        private static readonly Dictionary<int, ModelHelper> ModelHelpers = new Dictionary<int, ModelHelper> {
            {  // Main hand
                0,
                new ModelHelper(
                   "chara/weapon/w{0:D4}/obj/body/b{1:D4}/b{1:D4}.imc",
                   0,
                   "chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl",
                   2)
            },
            {  // Off hand
                1,
                new ModelHelper(
                   "chara/weapon/w{0:D4}/obj/body/b{1:D4}/b{1:D4}.imc",
                   0,
                   "chara/weapon/w{0:D4}/obj/body/b{1:D4}/model/w{0:D4}b{1:D4}.mdl",
                   2)
            },
            {  // Head
                2,
                new ModelHelper(
                   "chara/equipment/e{0:D4}/e{0:D4}.imc",
                   0,
                   "chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_met.mdl",
                   1)
            },
            {  // Body
                3,
                new ModelHelper(
                   "chara/equipment/e{0:D4}/e{0:D4}.imc",
                   1,
                   "chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_top.mdl",
                   1)
            },
            {  // Hands
                4,
                new ModelHelper(
                   "chara/equipment/e{0:D4}/e{0:D4}.imc",
                   2,
                   "chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_glv.mdl",
                   1)
            },
            {  // Waist
                5,
                null
            },
            {  // Legs
                6,
                new ModelHelper(
                   "chara/equipment/e{0:D4}/e{0:D4}.imc",
                   3,
                   "chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_dwn.mdl",
                   1)
            },
            {  // Feet
                7,
                new ModelHelper(
                   "chara/equipment/e{0:D4}/e{0:D4}.imc",
                   4,
                   "chara/equipment/e{0:D4}/model/c{4:D4}e{0:D4}_sho.mdl",
                   1)
            },
            {  // Ears
                8,
                new ModelHelper(
                   "chara/accessory/a{0:D4}/a{0:D4}.imc",
                   0,
                   "chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_ear.mdl",
                   1)
            },
            {  // Neck
                9,
                new ModelHelper(
                   "chara/accessory/a{0:D4}/a{0:D4}.imc",
                   1,
                   "chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_nek.mdl",
                   1)
            },
            {  // Wrists
                10,
                new ModelHelper(
                   "chara/accessory/a{0:D4}/a{0:D4}.imc",
                   2,
                   "chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_wrs.mdl",
                   1)
            },
            {  // R.Ring
                11,
                new ModelHelper(
                   "chara/accessory/a{0:D4}/a{0:D4}.imc",
                   3,
                   "chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_rir.mdl",
                   1)
            },
            {  // L.Ring
                12,
                new ModelHelper(
                   "chara/accessory/a{0:D4}/a{0:D4}.imc",
                   4,
                   "chara/accessory/a{0:D4}/model/c{4:D4}a{0:D4}_ril.mdl",
                   1)
            },
            {  // Soul Crystal
                13,
                null
            }
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
            }, {
                1301, 0101
            }, {
                1401, 0201
            }
        };

        /// <summary>
        ///     Get the model for a specific QWord, character type, and the current <see cref="EquipSlot" />.
        /// </summary>
        /// <param name="key">The identifier of the model.</param>
        /// <param name="characterType">Character type to get the model for.</param>
        /// <param name="materialVersion">When this method returns, contains the variant contained within <c>key</c>.</param>
        /// <returns>Returns the <see cref="Model" /> for the specified <c>key</c> and <c>characterType</c>.</returns>
        public ModelDefinition GetModel(Quad key, int characterType, out Graphics.ImcVariant variant) {
            variant = Graphics.ImcVariant.Default;

            ModelHelper helper;
            if (!ModelHelpers.TryGetValue(Key, out helper))
                return null;
            if (helper == null)
                return null;

            var packs = Collection.Collection.PackCollection;

            var variantIndex = (int)((key.ToInt64() >> (helper.VariantIndexWord * 16)) & 0xFFFF);

            var imcPath = string.Format(helper.ImcFileFormat, key.Value1, key.Value2, key.Value3, key.Value4, characterType);
            IO.File imcBase;
            if (!packs.TryGetFile(imcPath, out imcBase))
                return null;

            var imc = new Graphics.ImcFile(imcBase);
            variant = imc.GetVariant(helper.ImcPartKey, variantIndex);

            IO.File modelBase = null;
            while (!packs.TryGetFile(string.Format(helper.ModelFileFormat, key.Value1, key.Value2, key.Value3, key.Value4, characterType), out modelBase) && CharacterTypeFallback.TryGetValue(characterType, out characterType)) { }

            var asModel = modelBase as Graphics.ModelFile;
            if (asModel == null)
                return null;
            return asModel.GetModelDefinition();
        }


        public string GetModelKey(Quad key, int characterType) {
            ModelHelper helper;
            if (!ModelHelpers.TryGetValue(Key, out helper))
                return null;
            if (helper == null)
                return null;

            return string.Format(helper.ModelFileFormat, key.Value1, key.Value2, key.Value3, key.Value4, characterType);
        }
        #endregion
    }
}
