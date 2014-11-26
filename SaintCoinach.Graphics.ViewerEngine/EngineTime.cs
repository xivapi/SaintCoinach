using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public struct EngineTime {
        public readonly TimeSpan TotalTime;
        public readonly TimeSpan ElapsedTime;

        public EngineTime(TimeSpan totalTime, TimeSpan elapsedTime) {
            this.TotalTime = totalTime;
            this.ElapsedTime = elapsedTime;
        }
    }
}
