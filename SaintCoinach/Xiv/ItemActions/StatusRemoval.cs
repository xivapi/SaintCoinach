using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class StatusRemoval : ItemAction {
        const int StatusCount = 9;
        const int StatusOffset = 0;

        #region Fields
        private FriendlyEffect[] _Statuses = null;
        private FriendlyEffect[] _StatusesHq = null;
        #endregion

        #region Properties
        public IEnumerable<FriendlyEffect> Statuses { get { return _Statuses ?? (_Statuses = BuildStatuses(false)); } }
        public IEnumerable<FriendlyEffect> StatusesHq { get { return _StatusesHq ?? (_StatusesHq = BuildStatuses(true)); } }
        #endregion

        #region Constructor
        public StatusRemoval(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private FriendlyEffect[] BuildStatuses(bool hq) {
            var statuses = new List<FriendlyEffect>();
            var statusSheet = Sheet.Collection.GetSheet<FriendlyEffect>();

            for (var i = 0; i < StatusCount; ++i) {
                int statusKey;
                if (hq)
                    statusKey = GetHqData(StatusOffset + i);
                else
                    statusKey = GetData(StatusOffset + i);

                if (statusKey == 0 || statuses.Any(_ => _.Key == statusKey))
                    continue;

                statuses.Add(statusSheet[statusKey]);
            }

            return statuses.ToArray();
        }
        #endregion
    }
}