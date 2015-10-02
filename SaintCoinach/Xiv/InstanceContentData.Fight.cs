using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Xiv {
    partial class InstanceContentData {
        public class Fight {
            #region Properties
            public IEnumerable<RewardItem> RewardItems { get; private set; }
            public IEnumerable<BNpc> PrimaryBNpcs { get; private set; }
            public IEnumerable<BNpc> SecondaryBNpcs { get; private set; }
            public IEnumerable<Treasure> Treasures { get; private set; }

            public int CurrencyA { get; private set; }
            public int CurrencyB { get; private set; }
            public int CurrencyC { get; private set; }
            #endregion

            #region Constructor
            internal Fight(JsonReader reader, XivCollection collection) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                var bnpcs = collection.BNpcs;

                while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                    if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    switch (reader.Value.ToString()) {
                        case "RewardItems":
                            ReadRewardItems(reader, collection);
                            break;
                        case "ClearB":
                            ReadCurrencyB(reader);
                            break;
                        case "SubBNpcNames":
                            ReadSecondaryBNpcs(reader, bnpcs);
                            break;
                        case "ClearA":
                            ReadCurrencyA(reader);
                            break;
                        case "Treasure":
                            ReadTreasure(reader, collection);
                            break;
                        case "BNpcNames":
                            ReadPrimaryBNpcs(reader, bnpcs);
                            break;
                        case "ClearC":
                            ReadCurrencyC(reader);
                            break;
                        default:
                            Console.Error.WriteLine("Unknown 'InstanceContent.Fight' data key: {0}", reader.Value);
                            throw new NotSupportedException();
                    }
                }
            }
            #endregion

            #region Read
            private void ReadRewardItems(JsonReader reader, XivCollection collection) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                var values = new List<RewardItem>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    values.Add(new RewardItem(reader, collection));
                }
                this.RewardItems = values;
            }
            private void ReadCurrencyA(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                this.CurrencyA = Convert.ToInt32(reader.Value);
            }
            private void ReadCurrencyB(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                this.CurrencyB = Convert.ToInt32(reader.Value);
            }
            private void ReadCurrencyC(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                this.CurrencyC = Convert.ToInt32(reader.Value);
            }
            private void ReadSecondaryBNpcs(JsonReader reader, Collections.BNpcCollection bnpcs) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                var values = new List<BNpc>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    if (reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();

                    var key = Convert.ToInt64(reader.Value);
                    values.Add(bnpcs[key]);
                }
                this.SecondaryBNpcs = values;
            }
            private void ReadPrimaryBNpcs(JsonReader reader, Collections.BNpcCollection bnpcs) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                var values = new List<BNpc>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    if (reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();

                    var key = Convert.ToInt64(reader.Value);
                    values.Add(bnpcs[key]);
                }
                this.PrimaryBNpcs = values;
            }
            private void ReadTreasure(JsonReader reader, XivCollection collection) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                var values = new List<Treasure>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    values.Add(new Treasure(reader, collection));
                }
                this.Treasures = values;
            }
            #endregion
        }
    }
}
