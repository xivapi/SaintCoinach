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
            #endregion

            #region Constructor
            internal RewardItem(JsonReader reader, XivCollection collection) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                var allItems = collection.GetSheet<Item>();
                while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                    if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    switch (reader.Value.ToString()) {
                        case "has_rate_condition":
                            ReadRateCondition(reader);
                            break;
                        case "Item":
                            ReadItem(reader, allItems);
                            break;
                        default:
                            Console.Error.WriteLine("Unknown 'InstanceContent.RewardItem' data key: {0}", reader.Value);
                            throw new NotSupportedException();
                    }
                }
            }
            #endregion

            #region Read
            private void ReadRateCondition(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                var r = Convert.ToInt32(reader.Value);
                this.HasRateCondition = (r != 0);
            }
            private void ReadItem(JsonReader reader, IXivSheet<Item> allItems) {
                if (!reader.Read()) throw new InvalidOperationException();

                var key = Convert.ToInt32(reader.Value);
                this.Item = allItems[key];
            }
            #endregion
        }
    }
}
