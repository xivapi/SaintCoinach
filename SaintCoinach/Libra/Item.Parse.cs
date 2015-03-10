using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Libra {
    partial class Item {
        #region Fields
        private bool _IsParsed;

        private bool _OnlyOne;
        private bool _DisablePassedOthers;
        private bool _Crest;
        private long[] _BNpcs = new long[0];
        private int[] _ShopENpcs = new int[0];
        private int[] _InstanceContents = new int[0];
        private int[] _Recipes = new int[0];
        private int[] _Quests = new int[0];
        private int[] _ClassJobs = new int[0];
        private int[] _Achievements = new int[0];
        private int _SellPrice;
        private int _RepairClassJob;
        private int _RepairItem;
        private int _RepairPrice;
        private int _MateriaSocket;
        private int _MaterializeType;
        private int _Stain;
        private int _CondClassJob;
        private int _Series;
        private int _RecastTime;
        private BasicParam[] _BasicParams = new BasicParam[0];
        private BasicParam[] _BasicParamsHq = new BasicParam[0];
        private Action[] _Actions = new Action[0];
        private Action[] _ActionsHq = new Action[0];
        private Bonus[] _Bonuses = new Bonus[0];
        private Bonus[] _BonusesHq = new Bonus[0];
        private SeriesBonus _SeriesBonuses;
        private Bonus _Effect;
        private System.Drawing.Color _Color;
        #endregion

        #region Helper structs
        public class Bonus {
            public int BaseParam;
            public int Value;
        }
        public class BasicParam {
            public string Param;
            public float Value;
        }
        public abstract class Action {
            public int BaseParam;
        }
        public class FixedAction : Action {
            public int Value;
        }
        public class RelativeAction : Action {
            public int Rate;
            public int Limit;
        }
        public class SeriesBonus {
            public string Series;
            public string SpecialBonus;

            public List<KeyValuePair<string, Bonus>> Bonuses = new List<KeyValuePair<string, Bonus>>();
        }
        #endregion

        #region Properties
        public bool OnlyOne { get { Parse(); return _OnlyOne; } }
        public bool DisablePassedOthers { get { Parse(); return _DisablePassedOthers; } }
        public bool Crest { get { Parse(); return _Crest; } }

        public IEnumerable<long> BNpcs { get { Parse(); return _BNpcs; } }

        public IEnumerable<int> ShopENpcs { get { Parse(); return _ShopENpcs; } }
        public IEnumerable<int> InstanceContents { get { Parse(); return _InstanceContents; } }
        public IEnumerable<int> Recipes { get { Parse(); return _Recipes; } }
        public IEnumerable<int> Quests { get { Parse(); return _Quests; } }
        public IEnumerable<int> ClassJobs { get { Parse(); return _ClassJobs; } }
        public IEnumerable<int> Achievements { get { Parse(); return _Achievements; } }

        public int SellPrice { get { Parse(); return _SellPrice; } }
        public int RepairClassJob { get { Parse(); return _RepairClassJob; } }
        public int RepairItem { get { Parse(); return _RepairItem; } }
        public int RepairPrice { get { Parse(); return _RepairPrice; } }
        public int MateriaSocket { get { Parse(); return _MateriaSocket; } }
        public int MaterializeType { get { Parse(); return _MaterializeType; } }
        public int Stain { get { Parse(); return _Stain; } }
        public int CondClassJob { get { Parse(); return _CondClassJob; } }

        // There's actually a column in the table for this.
        // public int Series { get { Parse(); return _Series; } }
        public int RecastTime { get { Parse(); return _RecastTime; } }

        public IEnumerable<BasicParam> BasicParams { get { Parse(); return _BasicParams; } }
        public IEnumerable<BasicParam> BasicParamsHq { get { Parse(); return _BasicParamsHq; } }
        public IEnumerable<Action> Actions { get { Parse(); return _Actions; } }
        public IEnumerable<Action> ActionsHq { get { Parse(); return _ActionsHq; } }
        public IEnumerable<Bonus> Bonuses { get { Parse(); return _Bonuses; } }
        public IEnumerable<Bonus> BonusesHq { get { Parse(); return _BonusesHq; } }

        public SeriesBonus SeriesBonuses { get { Parse(); return _SeriesBonuses; } }
        public Bonus Effect { get { Parse(); return _Effect; } }
        public System.Drawing.Color Color { get { return _Color; } }
        #endregion

        #region Parse
        public void Parse() {
            if (_IsParsed) return;

            var json = Encoding.UTF8.GetString(this.data);
            using (var strReader = new System.IO.StringReader(json)) {
                using (var r = new JsonTextReader(strReader)) {
                    while (r.Read()) {
                        if (r.TokenType == JsonToken.PropertyName) {
                            switch (r.Value.ToString()) {
                                #region Boolean
                                case "OnlyOne":
                                    _OnlyOne = r.ReadInt32() != 0;
                                    break;
                                case "DisablePassedOthers":
                                    _DisablePassedOthers = r.ReadInt32() != 0;
                                    break;
                                case "Crest":
                                    _Crest = r.ReadInt32() != 0;
                                    break;
                                #endregion

                                #region Int64[]
                                case "bnpc":
                                    _BNpcs = r.ReadInt64Array();
                                    break;
                                #endregion

                                #region Int32[]
                                case "shopnpc":
                                    _ShopENpcs = r.ReadInt32Array();
                                    break;
                                case "instance_content":
                                    _InstanceContents = r.ReadInt32Array();
                                    break;
                                case "recipe":
                                    _Recipes = r.ReadInt32Array();
                                    break;
                                case "quest":
                                    _Quests = r.ReadInt32Array();
                                    break;
                                case "classjob":
                                    _ClassJobs = r.ReadInt32Array();
                                    break;
                                case "achievement":
                                    _Achievements = r.ReadInt32Array();
                                    break;
                                #endregion

                                #region Int32
                                case "sell_price":
                                    _SellPrice = r.ReadInt32();
                                    break;
                                case "Repair":
                                    _RepairClassJob = r.ReadInt32();
                                    break;
                                case "RepairItem":
                                    _RepairItem = r.ReadInt32();
                                    break;
                                case "repair_price":
                                    _RepairPrice = r.ReadInt32();
                                    break;
                                case "MateriaSocket":
                                    _MateriaSocket = r.ReadInt32();
                                    break;
                                case "MaterializeType":
                                    _MaterializeType = r.ReadInt32();
                                    break;
                                case "Stain":
                                    _Stain = r.ReadInt32();
                                    break;
                                case "CondClassJob":
                                    _CondClassJob = r.ReadInt32();
                                    break;
                                case "Series":
                                    _Series = r.ReadInt32();
                                    break;
                                case "RecastTime":
                                    _RecastTime = r.ReadInt32();
                                    break;
                                #endregion

                                #region Obj
                                case "basic_param":
                                    _BasicParams = ParseBasicParam(r);
                                    break;
                                case "basic_param_hq":
                                    _BasicParamsHq = ParseBasicParam(r);
                                    break;
                                case "action":
                                    _Actions = ParseActions(r);
                                    break;
                                case "action_hq":
                                    _ActionsHq = ParseActions(r);
                                    break;
                                case "bonus":
                                    _Bonuses = ParseBonuses(r);
                                    break;
                                case "bonus_hq":
                                    _BonusesHq = ParseBonuses(r);
                                    break;
                                case "series_bonus":
                                    _SeriesBonuses = ParseSeriesBonuses(r);
                                    break;
                                case "effect":
                                    if (!r.Read()) throw new InvalidOperationException();
                                    _Effect = ParseBonus(r);
                                    break;
                                case "color":
                                    _Color = ParseColor(r);
                                    break;
                                #endregion

                                default:
                                    Console.Error.WriteLine("Unknown 'Item' data key: {0}", r.Value);
                                    throw new NotSupportedException();
                            }
                        }
                    }
                }
            }

            _IsParsed = true;
        }
        private System.Drawing.Color ParseColor(JsonReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var r = reader.ReadInt32();
            var g = reader.ReadInt32();
            var b = reader.ReadInt32();

            if (!reader.Read() || reader.TokenType != JsonToken.EndArray) throw new InvalidOperationException();

            return System.Drawing.Color.FromArgb(r, g, b);
        }
        private Action[] ParseActions(JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Action>();
            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                values.Add(ParseAction(r));
            }
            return values.ToArray();
        }
        private Action ParseAction(JsonReader r) {
            if (r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();
            if (!r.Read() || r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

            var paramKey = Convert.ToInt32(r.Value);

            if (!r.Read()) throw new InvalidOperationException();

            Action value;
            if (r.TokenType == JsonToken.StartObject) {
                var act = new RelativeAction { BaseParam = paramKey };

                while (r.Read() && r.TokenType != JsonToken.EndObject) {
                    if (r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    switch (r.Value.ToString()) {
                        case "rate":
                            act.Rate = r.ReadInt32();
                            break;
                        case "limit":
                            act.Limit = r.ReadInt32();
                            break;
                        default:
                            Console.Error.WriteLine("Unknown 'Item'.'action' data key: {0}", r.Value);
                            throw new NotSupportedException();
                    }
                }

                value = act;
            } else if (r.TokenType == JsonToken.Integer || r.TokenType == JsonToken.String) {
                value = new FixedAction { BaseParam = paramKey, Value = Convert.ToInt32(r.Value) };
            } else
                throw new InvalidOperationException();


            if (!r.Read() || r.TokenType != JsonToken.EndObject) throw new InvalidOperationException();

            return value;
        }
        private Bonus[] ParseBonuses(JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Bonus>();
            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                values.Add(ParseBonus(r));
            }
            return values.ToArray();
        }
        private Bonus ParseBonus(JsonReader r) {
            if (r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();
            if (!r.Read() || r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

            var key = Convert.ToInt32(r.Value);
            var value = r.ReadInt32();

            if (!r.Read() || r.TokenType != JsonToken.EndObject) throw new InvalidOperationException();

            return new Bonus { BaseParam = key, Value = value };
        }
        private BasicParam[] ParseBasicParam(JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<BasicParam>();
            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                if (r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();
                if (!r.Read() || r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                var key = r.Value.ToString();
                var value = r.ReadSingle();

                values.Add(new BasicParam { Param = key, Value = value });

                if (!r.Read() || r.TokenType != JsonToken.EndObject) throw new InvalidOperationException();
            }
            return values.ToArray();
        }
        private SeriesBonus ParseSeriesBonuses(JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

            var bonus = new SeriesBonus();
            while (r.Read() && r.TokenType != JsonToken.EndObject) {
                if (r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                switch (r.Value.ToString()) {
                    case "SpecialBonus":
                        bonus.SpecialBonus = r.ReadAsString();
                        break;
                    case "Series":
                        bonus.Series = r.ReadAsString();
                        break;
                    case "bonus":
                        ParseSeriesBonus(r, bonus);
                        break;
                    default:
                        Console.Error.WriteLine("Unknown 'Item'.'series_bonus' data key: {0}", r.Value);
                        throw new NotSupportedException();
                }
            }
            return bonus;
        }
        private void ParseSeriesBonus(JsonReader r, SeriesBonus bonus) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                if (r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                if (!r.Read() || r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                var bonusKey = r.Value.ToString();

                if (!r.Read() || r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();
                if (!r.Read() || r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                var paramKey = Convert.ToInt32(r.Value);
                var value = r.ReadInt32();

                bonus.Bonuses.Add(new KeyValuePair<string, Bonus>(bonusKey, new Bonus { BaseParam = paramKey, Value = value }));

                if (!r.Read() || r.TokenType != JsonToken.EndObject) throw new InvalidOperationException();
                if (!r.Read() || r.TokenType != JsonToken.EndObject) throw new InvalidOperationException();
            }
        }
        #endregion
    }
}
