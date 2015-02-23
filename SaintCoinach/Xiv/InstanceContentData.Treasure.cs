using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Xiv {
    partial class InstanceContentData {
        public class Treasure : IItemSource {
            #region Properties
            public IEnumerable<Item> Items { get; private set; }
            public bool HasWeeklyRestriction { get; private set; }
            public System.Drawing.Point? Coordinates { get; private set; }
            #endregion

            #region Constructor
            internal Treasure(JsonReader reader, XivCollection collection) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                var allItems = collection.GetSheet<Item>();

                while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                    if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    switch (reader.Value.ToString()) {
                        case "Currency":
                            ReadCurrency(reader);
                            break;
                        case "Item":
                            ReadItems(reader, allItems);
                            break;
                        case "WeekRestrictionIndex":
                            ReadWeeklyRestriction(reader);
                            break;
                        case "coordinate":
                            ReadCoordinates(reader);
                            break;
                        default:
                            Console.Error.WriteLine("Unknown 'InstanceContent.Treasure' data key: {0}", reader.Value);
                            throw new NotSupportedException();
                    }
                }
            }
            #endregion

            #region Read
            private void ReadCurrency(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();
                if (!reader.Read() || reader.TokenType != JsonToken.EndArray) throw new InvalidOperationException();
            }
            private void ReadItems(JsonReader reader, IXivSheet<Item> allItems) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                var items = new List<Item>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    if (reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();

                    var key = Convert.ToInt32(reader.Value);
                    items.Add(allItems[key]);
                }
                this.Items = items;
            }
            private void ReadWeeklyRestriction(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                var r = Convert.ToInt32(reader.Value);
                this.HasWeeklyRestriction = (r >= 0);
            }
            private void ReadCoordinates(JsonReader reader) {
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                var x = Convert.ToInt32(reader.Value);
                if (!reader.Read() || reader.TokenType != JsonToken.Integer) throw new InvalidOperationException();
                var y = Convert.ToInt32(reader.Value);

                if (x == 0 && y == 0)
                    this.Coordinates = null;
                else
                    this.Coordinates = new System.Drawing.Point(x, y);

                if (!reader.Read() || reader.TokenType != JsonToken.EndArray) throw new InvalidOperationException();
            }
            #endregion
        }
    }
}
