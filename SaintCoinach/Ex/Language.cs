using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public enum Language {
        [Description("")]
        None,
        [Description("ja")]
        Japanese,
        [Description("en")]
        English,
        [Description("de")]
        German,
        [Description("fr")]
        French,

        [Description("?")]
        Unsupported
    }

    public static class LanguageExtensions {
        private static readonly Language[] _Languages = new Language[] { Language.Japanese, Language.English, Language.French, Language.German };
        private static readonly Dictionary<Language, string> _LangToCode;
        private static readonly Dictionary<string, Language> _CodeToLang;

        static LanguageExtensions() {
            _LangToCode = new Dictionary<Language, string>();
            _CodeToLang = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);

            foreach (var lang in _Languages) {
                var code = GetCode(lang);
                _LangToCode.Add(lang, code);
                _CodeToLang.Add(code, lang);
            }
        }

        public static IEnumerable<Language> Languages { get { return _Languages; } }

        public static string GetCode(this Language self) {
            if (_LangToCode.ContainsKey(self))
                return _LangToCode[self];
            var type = typeof(Language);
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var fi = fields.First(_ => (Language)_.GetValue(null) == self);
            var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (attr == null)
                return self.ToString();
            return attr.Description;
        }
        public static string GetSuffix(this Language self) {
            var code = self.GetCode();
            if (code.Length > 0)
                return "_" + code;
            return string.Empty;
        }
        public static Language GetFromCode(string code) {
            if (_CodeToLang.ContainsKey(code))
                return _CodeToLang[code];
            return Language.Unsupported;
        }
    }
}
