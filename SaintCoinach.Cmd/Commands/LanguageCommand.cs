using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    using Ex;

    public class LanguageCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;

        public LanguageCommand(ARealmReversed realm)
            : base("lang", "Change the language.") {
            _Realm = realm;
        }

        public override async Task InvokeAsync(string[] paramList) {
            if (paramList.Length == 0) {
                OutputInformation($"Current language: {_Realm.GameData.ActiveLanguage}");
                return;
            }
            var param = paramList[0].Trim();
            if (!Enum.TryParse<Language>(param, out var newLang)) {
                newLang = LanguageExtensions.GetFromCode(param);
                if (newLang == Language.Unsupported) {
                    OutputError($"Unknown language: {param}");
                    return;
                }
            }
            _Realm.GameData.ActiveLanguage = newLang;
        }
    }
}
