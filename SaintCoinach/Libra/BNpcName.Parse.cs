using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Libra {
    partial class BNpcName {
        #region Fields
        private bool _IsParsed;

        private Tuple<int, Tuple<int, int[]>[]>[] _Regions;
        private int[] _Items;
        private int[] _NonPops;
        private int[] _InstanceContents;
        #endregion

        #region Properties
        public IEnumerable<Tuple<int, Tuple<int, int[]>[]>> Regions { get { Parse(); return _Regions; } }
        public IEnumerable<int> Items { get { Parse(); return _Items; } }
        public IEnumerable<int> NonPops { get { Parse(); return _NonPops; } }
        public IEnumerable<int> InstanceContents { get { Parse(); return _InstanceContents; } }

        public long NameKey { get { return Key % 10000000000; } }
        public long BaseKey { get { return Key / 10000000000; } }
        #endregion

        #region Parse
        private void Parse() {
            if (_IsParsed) return;

            var json = Encoding.UTF8.GetString(this.data);
            using (var strReader = new System.IO.StringReader(json)) {
                using (var r = new JsonTextReader(strReader)) {
                    while (r.Read()) {
                        if (r.TokenType == JsonToken.PropertyName) {
                            switch (r.Value.ToString()) {
                                case "region":
                                    ParseRegions(r);
                                    break;
                                case "nonpop":
                                    ParseNonPops(r);
                                    break;
                                case "item":
                                    ParseItems(r);
                                    break;
                                case "instance_contents":
                                    ParseInstanceContents(r);
                                    break;
                                default:
                                    Console.Error.WriteLine("Unknown 'BNpcName' data key: {0}", r.Value);
                                    throw new NotSupportedException();
                            }
                        }
                    }
                }
            }

            _IsParsed = true;
        }

        private void ParseRegions(JsonTextReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

            var values = new List<Tuple<int, Tuple<int, int[]>[]>>();
            while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                var region = Convert.ToInt32(reader.Value);

                if (!reader.Read() || reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                var zones = new List<Tuple<int, int[]>>();
                while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                    if (reader.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                    var zone = Convert.ToInt32(reader.Value);

                    if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

                    var levels = new List<int>();
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                        if (!"??".Equals(reader.Value.ToString()))
                            levels.Add(Convert.ToInt32(reader.Value));
                    }

                    zones.Add(Tuple.Create(zone, levels.ToArray()));
                }
                values.Add(Tuple.Create(region, zones.ToArray()));
            }
            _Regions = values.ToArray();
        }
        private void ParseNonPops(JsonTextReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<int>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                values.Add(Convert.ToInt32(reader.Value));
            }
            _NonPops = values.ToArray();
        }
        private void ParseItems(JsonTextReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<int>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                values.Add(Convert.ToInt32(reader.Value));
            }
            _Items = values.ToArray();
        }
        private void ParseInstanceContents(JsonTextReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<int>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                values.Add(Convert.ToInt32(reader.Value));
            }
            _InstanceContents = values.ToArray();
        }
        #endregion
    }
}
