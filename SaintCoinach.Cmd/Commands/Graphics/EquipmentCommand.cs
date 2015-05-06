using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;
using G = SaintCoinach.Graphics;

namespace SaintCoinach.Cmd.Commands.Graphics {
    class EquipmentCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public EquipmentCommand(ARealmReversed realm)
            : base("eq", "Display equipment.") {
            _Realm = realm;
        }

        static readonly int[] GenderSpecificSlots = new[] { 2, 3, 4, 5, 6, 7 };
        static readonly Dictionary<string, int> GenderCharTypes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
            { string.Empty, 101 },
            { "M", 101 },
            { "F", 201 },
        };
        public async override Task<bool> InvokeAsync(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                OutputError("No equipment specified.");
                return false;
            }

            paramList = paramList.Trim();
            var eq = _Realm.GameData.GetSheet<Xiv.Item>().FirstOrDefault(_ => string.Equals(_.Name, paramList, StringComparison.OrdinalIgnoreCase)) as Xiv.Items.Equipment;
            if (eq == null) {
                OutputError("Equipment could not be found.");
                return false;
            }

            int charType = 101;
            var charTypeName = "M";
            if (eq.EquipSlotCategory.PossibleSlots.Any(_ => GenderSpecificSlots.Contains(_.Key))) {
                var charTypeStr = QueryParam<string>("M or F?");
                if (!GenderCharTypes.TryGetValue(charTypeStr, out charType)) {
                    OutputError("Invalid input.");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(charTypeStr))
                    charTypeName = charTypeStr;
            }

            int stainKey = 0;
            var stainName = "Undyed";

            if (eq.IsDyeable) {
                var stainStr = QueryParam<string>("Dye?");
                if (string.IsNullOrWhiteSpace(stainStr))
                    stainKey = 0;
                else {
                    Xiv.Stain stain;
                    if (int.TryParse(stainStr, out stainKey)) {
                        stain = _Realm.GameData.GetSheet<Xiv.Stain>().FirstOrDefault(_ => _.Key == stainKey);
                    } else {
                        stain = _Realm.GameData.GetSheet<Xiv.Stain>().FirstOrDefault(_ =>
                            string.Equals(_.Name, stainStr, StringComparison.OrdinalIgnoreCase) ||
                            (_.Item != null && string.Equals(_.Item.Name, stainStr, StringComparison.OrdinalIgnoreCase)));
                    }

                    if (stain == null) {
                        OutputError("Colour not found.");
                        return false;
                    }
                    stainKey = stain.Key;
                    stainName = stain.Name;
                }
            }

            int v;
            var mdl = eq.GetModel(charType, out v);
            var subMdl = mdl.GetSubModel(0);
            var component = new G.Model(subMdl);

            var matParam = new string[]{
                (v & 0x7F).ToString(),
                stainKey.ToString()
            };
            foreach (var msg in GraphicsHelper.ApplyMaterial(component, matParam, 0))
                OutputWarning(msg);

            GraphicsHelper.RunViewer(component, string.Format("{0} ({1}, {2})", eq.Name, charTypeName, stainName));

            return true;
        }
    }
}
