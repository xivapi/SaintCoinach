using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Xiv {
    partial class InstanceContentData {
        public class RewardItem {
            #region Properties
            public Item Item { get; private set; }
            public bool HasRateCondition { get; private set; }
            public Quest RequiredQuest { get; private set; }
            public bool HasWeekRestriction { get; private set; }
            #endregion

            #region Constructor
            internal RewardItem(JsonReader reader, XivCollection collection) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                var allItems = collection.GetSheet<Item>();
                var allQuests = collection.GetSheet<Quest>();
                while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                    if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    switch (reader.Value.ToString()) {
                        case "has_rate_condition":
                            this.HasRateCondition = ReadCondition(reader);
                            break;
                        case "Item":
                            ReadItem(reader, allItems);
                            break;
                        case "Quest":
                            ReadRequiredQuest(reader, allQuests);
                            break;
                        case "is_week_restriction_one":
                            this.HasWeekRestriction = ReadCondition(reader);
                            break;
                        default:
                            Console.Error.WriteLine("Unknown 'InstanceContent.RewardItem' data key: {0}", reader.Value);
                            throw new NotSupportedException();
                    }
                }
            }
            #endregion

            #region Read
            private bool ReadCondition(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                var r = Convert.ToInt32(reader.Value);
                return r != 0;
            }
            private void ReadItem(JsonReader reader, IXivSheet<Item> allItems) {
                if (!reader.Read()) throw new InvalidOperationException();

                var key = Convert.ToInt32(reader.Value);
                this.Item = allItems[key];
            }
            private void ReadRequiredQuest(JsonReader reader, IXivSheet<Quest> allQuests) {
                if (!reader.Read()) throw new InvalidOperationException();

                var key = Convert.ToInt32(reader.Value);
                this.RequiredQuest = allQuests[key];
            }
            #endregion
        }
    }
}
