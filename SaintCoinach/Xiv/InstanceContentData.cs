using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Xiv {
    public partial class InstanceContentData : IItemSource {
        #region Properties

        public InstanceContent InstanceContent { get; private set; }
        public IEnumerable<Fight> AllBosses { get; private set; }
        public Fight Boss { get; private set; }
        public IEnumerable<Fight> MidBosses { get; private set; }
        public IEnumerable<Treasure> MapTreasures { get; private set; }

        #endregion

        #region Constructor

        public InstanceContentData(InstanceContent instanceContent) {
            this.InstanceContent = instanceContent ?? throw new ArgumentNullException("instanceContent");

            var coll = instanceContent.Sheet.Collection;
            if (!coll.IsLibraAvailable) return;

            var libraRow = coll.Libra.InstanceContents.FirstOrDefault(i => i.Key == instanceContent.Key);
            if (libraRow != null)
                Parse(libraRow);
        }
        #endregion

        #region Parse
        private void Parse(Libra.InstanceContent libraRow) {
            var json = Encoding.UTF8.GetString(libraRow.data);
            using (var strReader = new System.IO.StringReader(json)) {
                using (var r = new JsonTextReader(strReader)) {
                    if (!r.Read() || r.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                    while (r.Read() && r.TokenType != JsonToken.EndObject) {
                        if (r.TokenType != JsonToken.PropertyName) throw new InvalidOperationException();

                        switch (r.Value.ToString()) {
                            case "Boss":
                                ReadBoss(r);
                                break;
                            case "MiddleBoss":
                                ReadMidBosses(r);
                                break;
                            case "Map":
                                ReadMapTreasures(r);
                                break;
                            default:
                                Console.Error.WriteLine("Unknown 'InstanceContent' data key: {0}", r.Value);
                                throw new NotSupportedException();
                        }
                    }
                }
            }
            var allBosses = new List<Fight>();
            if (MidBosses != null)
                allBosses.AddRange(MidBosses);
            if (Boss != null)
                allBosses.Add(Boss);
            this.AllBosses = allBosses;
        }

        private void ReadBoss(JsonReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

            this.Boss = new Fight(reader, InstanceContent.Sheet.Collection);
        }
        private void ReadMidBosses(JsonReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Fight>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                values.Add(new Fight(reader, InstanceContent.Sheet.Collection));
            }
            this.MidBosses = values;
        }
        private void ReadMapTreasures(JsonReader reader) {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Treasure>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                if (reader.TokenType != JsonToken.StartObject) throw new InvalidOperationException();

                values.Add(new Treasure(reader, InstanceContent.Sheet.Collection));
            }
            this.MapTreasures = values;
        }
        #endregion

        #region IItemSource Members

        private Item[] _ItemSourceItems;
        IEnumerable<Item> IItemSource.Items {
            get {
                if (_ItemSourceItems != null)
                    return _ItemSourceItems;

                IEnumerable<Item> v = new Item[0];

                if (Boss != null) {
                    v = v.Concat(Boss.RewardItems.Select(i => i.Item));
                    v = v.Concat(Boss.Treasures.SelectMany(i => i.Items));
                }
                if (MidBosses != null) {
                    v = v.Concat(MidBosses.SelectMany(f => f.RewardItems.Select(i => i.Item)));
                    v = v.Concat(MidBosses.SelectMany(f => f.Treasures.SelectMany(i => i.Items)));
                }
                if (MapTreasures != null)
                    v.Concat(MapTreasures.SelectMany(i => i.Items));

                return _ItemSourceItems = v.Distinct().ToArray();
            }
        }

        #endregion
    }
}
