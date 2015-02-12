using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SaintCoinach.Ex {
    public enum Language {
        [Description("")] None,
        [Description("ja")] Japanese,
        [Description("en")] English,
        [Description("de")] German,
        [Description("fr")] French,

        [Description("?")] Unsupported
    }

    public static class LanguageExtensions {
        #region Static

        private static readonly Dictionary<Language, string> LangToCode;
        private static readonly Dictionary<string, Language> CodeToLang;

        #endregion

        #region Properties

        public static IEnumerable<Language> Languages {
            get {
                yield return Language.Japanese;
                yield return Language.English;
                yield return Language.French;
                yield return Language.German;
            }
        }

        #endregion

        #region Constructors

        static LanguageExtensions() {
            LangToCode = new Dictionary<Language, string>();
            CodeToLang = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);

            foreach (var lang in Languages) {
                var code = GetCode(lang);
                LangToCode.Add(lang, code);
                CodeToLang.Add(code, lang);
            }
        }

        #endregion

        public static string GetCode(this Language self) {
            if (LangToCode.ContainsKey(self))
                return LangToCode[self];
            var type = typeof(Language);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            var fi = fields.First(_ => (Language)_.GetValue(null) == self);
            var attr =
                fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            return attr == null ? self.ToString() : attr.Description;
        }

        public static string GetSuffix(this Language self) {
            var code = self.GetCode();
            if (code.Length > 0)
                return "_" + code;
            return string.Empty;
        }

        public static Language GetFromCode(string code) {
            return CodeToLang.ContainsKey(code) ? CodeToLang[code] : Language.Unsupported;
        }
    }
}
