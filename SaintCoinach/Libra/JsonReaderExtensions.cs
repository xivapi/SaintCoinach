using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SaintCoinach.Libra {
    public static class JsonReaderExtensions {
        public static Int32 ReadInt32(this JsonReader r) {
            if (!r.Read()) throw new InvalidOperationException();
            if (r.TokenType != JsonToken.Integer && r.TokenType != JsonToken.String) throw new InvalidOperationException();

            return Convert.ToInt32(r.Value);
        }
        public static Int32[] ReadInt32Array(this JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Int32>();

            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                if (r.TokenType != JsonToken.Integer && r.TokenType != JsonToken.String) throw new InvalidOperationException();

                values.Add(Convert.ToInt32(r.Value));
            }

            return values.ToArray();
        }
        public static Int64 ReadInt64(this JsonReader r) {
            if (!r.Read()) throw new InvalidOperationException();
            if (r.TokenType != JsonToken.Integer && r.TokenType != JsonToken.String) throw new InvalidOperationException();

            return Convert.ToInt64(r.Value);
        }
        public static Int64[] ReadInt64Array(this JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Int64>();

            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                if (r.TokenType != JsonToken.Integer && r.TokenType != JsonToken.String) throw new InvalidOperationException();

                values.Add(Convert.ToInt64(r.Value));
            }

            return values.ToArray();
        }
        public static Single ReadSingle(this JsonReader r) {
            if (!r.Read()) throw new InvalidOperationException();
            if (r.TokenType != JsonToken.Integer && r.TokenType != JsonToken.String && r.TokenType != JsonToken.Float) throw new InvalidOperationException();

            return Convert.ToSingle(r.Value);
        }
        public static Single[] ReadSingleArray(this JsonReader r) {
            if (!r.Read() || r.TokenType != JsonToken.StartArray) throw new InvalidOperationException();

            var values = new List<Single>();

            while (r.Read() && r.TokenType != JsonToken.EndArray) {
                if (r.TokenType != JsonToken.Integer && r.TokenType != JsonToken.String && r.TokenType != JsonToken.Float) throw new InvalidOperationException();

                values.Add(Convert.ToSingle(r.Value));
            }

            return values.ToArray();
        }
    }
}
