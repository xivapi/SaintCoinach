using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Search {
    using Behaviors;
    using Interfaces;

    [SearchFunctionExport(Function = "param")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ParameterQuery : Services.Search.IntegerRangeQuery {
        [Import]
        private Xiv.XivCollection Data { get; set; }

        private Xiv.BaseParam _BaseParam;
        public Xiv.BaseParam BaseParam {
            get { return _BaseParam; }
            set {
                _BaseParam = value;
                OnPropertyChanged(() => BaseParam);
            }
        }

        public override IEnumerable<Type> MatchedTypes {
            get { yield return typeof(Xiv.IParameterObject); }
        }

        private static Regex SetPattern = new Regex(@"^\s*(?<Param>[^\(]+)(?:\s*\((?<Range>[^\)]+)\))?\s*$", RegexOptions.Compiled | RegexOptions.Singleline);
        public override void Set(string args) {
            var match = SetPattern.Match(args);
            string rangeStr = "";
            if (match.Success) {
                var paramSheet = Data.GetSheet<Xiv.BaseParam>();

                var paramStr = match.Groups["Param"].Value;
                int paramKey;
                if (int.TryParse(paramStr, out paramKey)) {
                    if (paramSheet.ContainsRow(paramKey))
                        BaseParam = paramSheet[paramKey];
                    else
                        BaseParam = null;
                } else
                    BaseParam = paramSheet.FirstOrDefault(_ => string.Equals(_.Name, paramStr, StringComparison.OrdinalIgnoreCase));

                if (match.Groups["Range"].Success)
                    rangeStr = match.Groups["Range"].Value;
            }
            base.Set(rangeStr);
        }

        public override bool IsMatch(object value) {
            if (BaseParam == null)
                return false;

            var asParamObj = value as Xiv.IParameterObject;
            if (asParamObj == null)
                return false;

            var param = asParamObj.Parameters.FirstOrDefault(_ => _.BaseParam == this.BaseParam);
            if (param == null)
                return false;

            return param.Any(_ => IsInRange(_));
        }

        private bool IsInRange(Xiv.ParameterValue paramVal) {
            double check;
            if (paramVal is Xiv.ParameterValueFixed)
                check = ((Xiv.ParameterValueFixed)paramVal).Amount;
            else if (paramVal is Xiv.ParameterValueRelativeLimited)
                check = ((Xiv.ParameterValueRelativeLimited)paramVal).Maximum;
            else if (paramVal is Xiv.ParameterValueRelative)
                check = ((Xiv.ParameterValueRelative)paramVal).Amount;
            else
                return false;
            return IsInRange(check);
        }

        public override string ToString() {
            var baseStr = base.ToString();
            var sb = new StringBuilder();
            sb.AppendFormat("param:{0}", BaseParam);
            if (!string.IsNullOrWhiteSpace(baseStr))
                sb.AppendFormat("({0})", baseStr);
            return sb.ToString();
        }
    }
}
