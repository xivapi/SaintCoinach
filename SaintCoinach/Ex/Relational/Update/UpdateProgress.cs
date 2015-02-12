using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update {
    public struct UpdateProgress {
        public int CurrentStep { get; set; }
        public int TotalSteps { get; set; }

        public double Percentage { get { return CurrentStep / (double)TotalSteps; } }

        public string CurrentFile { get; set; }
        public string CurrentOperation { get; set; }

        public override string ToString() {
            var sb = new StringBuilder();

            sb.AppendFormat("{0,4:P0} ({1} / {2}): {3}", Percentage, CurrentStep, TotalSteps, CurrentOperation);
            if (!string.IsNullOrWhiteSpace(CurrentFile))
                sb.AppendFormat(" > {0}", CurrentFile);
            return sb.ToString();
        }
    }
}
