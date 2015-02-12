using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class StatusRemoval : ItemAction {
        #region Static

        private const int StatusCount = 9;
        private const int StatusOffset = 0;

        #endregion

        #region Fields

        private FriendlyEffect[] _Statuses;
        private FriendlyEffect[] _StatusesHq;

        #endregion

        #region Properties

        public IEnumerable<FriendlyEffect> Statuses { get { return _Statuses ?? (_Statuses = BuildStatuses(false)); } }

        public IEnumerable<FriendlyEffect> StatusesHq {
            get { return _StatusesHq ?? (_StatusesHq = BuildStatuses(true)); }
        }

        #endregion

        #region Constructors

        #region Constructor

        public StatusRemoval(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private FriendlyEffect[] BuildStatuses(bool hq) {
            var statuses = new List<FriendlyEffect>();
            var statusSheet = Sheet.Collection.GetSheet<FriendlyEffect>();

            for (var i = 0; i < StatusCount; ++i) {
                var statusKey = hq ? GetHqData(StatusOffset + i) : GetData(StatusOffset + i);

                if (statusKey == 0 || statuses.Any(_ => _.Key == statusKey))
                    continue;

                statuses.Add(statusSheet[statusKey]);
            }

            return statuses.ToArray();
        }

        #endregion
    }
}
