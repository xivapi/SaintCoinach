using System.Text;
using System.Threading;

namespace SaintCoinach.Ex.Relational.Update {
    public struct UpdateProgress {
        private int _CurrentStep;

        #region Properties

        public int CurrentStep { get { return _CurrentStep; } }
        public int TotalSteps { get; set; }
        public double Percentage { get { return _CurrentStep / (double)TotalSteps; } }
        public string CurrentFile { get; set; }
        public string CurrentOperation { get; set; }

        #endregion

        public void IncrementStep()
        {
            Interlocked.Increment(ref _CurrentStep);
        }

        public override string ToString() {
            var sb = new StringBuilder();

            sb.AppendFormat("{0,4:P0} ({1} / {2}): {3}", Percentage, CurrentStep, TotalSteps, CurrentOperation);
            if (!string.IsNullOrWhiteSpace(CurrentFile))
                sb.AppendFormat(" > {0}", CurrentFile);
            return sb.ToString();
        }
    }
}
