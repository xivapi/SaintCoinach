using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Libra {
    partial class ENpcResident {
        #region Fields
        private int[] _AsQuestClient;
        private Tuple<int, System.Drawing.PointF[]>[] _Coordinates;
        private int[] _Quests;
        private Tuple<int, int[]>[] _Shops;

        private bool _IsParsed = false;
        #endregion

        #region Properties
        public IEnumerable<int> AsQuestClient { get { Parse(); return _AsQuestClient; } }
        public IEnumerable<Tuple<int, System.Drawing.PointF[]>> Coordinates { get { Parse(); return _Coordinates; } }
        public IEnumerable<int> Quests { get { Parse(); return _Quests; } }
        public IEnumerable<Tuple<int, int[]>> Shops { get { Parse(); return _Shops; } }
        #endregion

        #region Parse
        public void Parse() {
            if (_IsParsed)
                return;

            var json = Encoding.UTF8.GetString(this.data);
            using (var strReader = new System.IO.StringReader(json)) {
                using (var r = new JsonTextReader(strReader)) {
                    while (r.Read()) {
                        if (r.TokenType == JsonToken.PropertyName) {
                            switch (r.Value.ToString()) {
                                case "client_quest":
                                    _AsQuestClient = r.ReadInt32Array();
                                    break;
                                case "coordinate":
                                    ParseCoordinate(r);
                                    break;
                                case "quest":
                                    _Quests = r.ReadInt32Array();
                                    break;
                                case "shop":
                                    ParseShops(r);
                                    break;
                                default:
                                    Console.Error.WriteLine("Unknown 'ENpcResident' data key: {0}", r.Value);
                                    throw new NotSupportedException();
                            }
                        }
                    }
                }
            }

            _IsParsed = true;
        }

        private void ParseCoordinate(JsonTextReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartObject)
                throw new InvalidOperationException();

            var allCoord = new List<Tuple<int, System.Drawing.PointF[]>>();
            while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                var key = Convert.ToInt32(reader.Value);

                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                var coordinates = new List<System.Drawing.PointF>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    if (reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                    if (!reader.Read() || reader.TokenType != JsonToken.String) throw new InvalidOperationException();
                    var x = float.Parse((string)reader.Value, CultureInfo.InvariantCulture);

                    if (!reader.Read() || reader.TokenType != JsonToken.String) throw new InvalidOperationException();
                    var y = float.Parse((string)reader.Value, CultureInfo.InvariantCulture);

                    if (!reader.Read() || reader.TokenType != JsonToken.EndArray) throw new InvalidOperationException();

                    coordinates.Add(new System.Drawing.PointF(x, y));
                }

                allCoord.Add(Tuple.Create(key, coordinates.ToArray()));
            }

            _Coordinates = allCoord.ToArray();
        }
        private void ParseShops(JsonTextReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var shops = new List<Tuple<int, int[]>>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();
                if (!reader.Read() || reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                var key = Convert.ToInt32(reader.Value);

                var items = new List<int>();
                if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();
                    if (!reader.Read() || reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    var itemKey = Convert.ToInt32(reader.Value);

                    if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray) { }

                    items.Add(itemKey);

                    if (!reader.Read() || reader.TokenType != JsonToken.EndObject) throw new InvalidOperationException();
                }

                shops.Add(Tuple.Create(key, items.ToArray()));

                if (!reader.Read() || reader.TokenType != JsonToken.EndObject) throw new InvalidOperationException();
            }

            _Shops = shops.ToArray();
        }
        #endregion

    }
}
