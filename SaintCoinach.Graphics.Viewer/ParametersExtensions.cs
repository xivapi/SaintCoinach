using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    static class ParametersExtensions {
        public static T GetValueOrDefault<T>(this Data.ParametersBase self, string key) {
            return self.GetValueOrDefault(key, default(T));
        }
        public static T GetValueOrDefault<T>(this Data.ParametersBase self, string key, T defaultValue) {
            if (self == null)
                return defaultValue;
            T ret;
            if (self.TryGetValue(key, out ret))
                return ret;
            return defaultValue;
        }
    }
}
