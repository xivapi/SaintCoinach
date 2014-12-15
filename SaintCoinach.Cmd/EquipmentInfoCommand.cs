using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd {
    public class EquipmentInfoCommand : ActionCommandBase {
        private Xiv.XivCollection _Data;

        public EquipmentInfoCommand(Xiv.XivCollection data)
            : base("eq", "-") {
            _Data = data;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            var split = paramList.Split('>');
            if (split.Length < 2)
                return false;
            var itemName = split[0].Trim();
            var paramName = split[1].Trim();

            var item = _Data.GetSheet<Xiv.Item>().FirstOrDefault(_ => _.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)) as Xiv.Items.Equipment;
            var param = _Data.GetSheet<Xiv.BaseParam>().FirstOrDefault(_ => _.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase));
            if (item == null || param == null)
                return false;

            OutputInformation("{0} (NQ): {1} +{2}", item.Name, param.Name, item.GetMateriaMeldCap(param, false));
            OutputInformation("{0} (HQ): {1} +{2}", item.Name, param.Name, item.GetMateriaMeldCap(param, true));

            return true;
        }
    }
}
