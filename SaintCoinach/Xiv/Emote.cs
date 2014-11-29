using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Emote : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public EmoteCategory EmoteCategory { get { return As<EmoteCategory>(); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public LogMessage TargetedLogMessage { get { return As<LogMessage>("LogMessage{Targeted}"); } }
        public LogMessage UntargetedLogMessage { get { return As<LogMessage>("LogMessage{Untargeted}"); } }
        #endregion

        #region Constructor
        public Emote(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}