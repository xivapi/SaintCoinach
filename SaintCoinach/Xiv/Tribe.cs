using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Tribe : XivRow {
        #region Static
        // XXX: Magic happening here
        static readonly Dictionary<int, Tuple<short, short>> ModelKeys = new Dictionary<int, Tuple<short, short>> {
            {  1, Tuple.Create<short, short>(0101, 0201) },      // Hyur Midlander
            {  2, Tuple.Create<short, short>(0301, 0401) },      // Hyur Highlander
            {  3, Tuple.Create<short, short>(0501, 0601) },      // Elezen Wildwood
            {  4, Tuple.Create<short, short>(0501, 0601) },      // Elezen Duskwight
            {  5, Tuple.Create<short, short>(1101, 1201) },      // Lalafell Plainsfolk
            {  6, Tuple.Create<short, short>(1101, 1201) },      // Lalafell Dunesfolk
            {  7, Tuple.Create<short, short>(0701, 0801) },      // Miqo'te Seeker of the Sun
            {  8, Tuple.Create<short, short>(0701, 0801) },      // Miqo'te Keeper of the Moon
            {  9, Tuple.Create<short, short>(0901, 1001) },      // Roegadyn Sea Wolf
            { 10, Tuple.Create<short, short>(0901, 1001) },      // Roegadyn Hellsguard
            { 11, Tuple.Create<short, short>(1301, 1401) },      // Au Ra Raen
            { 12, Tuple.Create<short, short>(1301, 1401) },      // Au Ra Xaela
        };
        #endregion

        #region Properties

        public short MaleModelTypeKey { get { return ModelKeys[this.Key].Item1; } }
        public short FemaleModelTypeKey { get { return ModelKeys[this.Key].Item2; } }

        public Text.XivString Masculine { get { return AsString("Masculine"); } }
        public Text.XivString Feminine { get { return AsString("Feminine"); } }

        public int StrengthBonus { get { return AsInt32("STR"); } }
        public int VitalityBonus { get { return AsInt32("VIT"); } }
        public int DexterityBonus { get { return AsInt32("DEX"); } }
        public int IntelligenceBonus { get { return AsInt32("INT"); } }
        public int MindBonus { get { return AsInt32("MND"); } }
        public int PietyBonus { get { return AsInt32("PIE"); } }

        #endregion

        #region Constructor

        public Tribe(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Feminine;
        }
    }
}
