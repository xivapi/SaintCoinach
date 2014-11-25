using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSquish {
    [Flags]
    public enum SquishOptions {
        /// <summary>
        /// Use DXT1 compression.
        /// </summary>
        DXT1 = (1 << 0),
        /// <summary>
        /// Use DXT3 compression.
        /// </summary>
        DXT3 = (1 << 1),
        /// <summary>
        /// Use DXT5 compression.
        /// </summary>
        DXT5 = (1 << 2),
        /// <summary>
        /// Use a very slow but very high quality colour compressor.
        /// </summary>
        ColourIterativeClusterFit = (1 << 3),
        /// <summary>
        /// Use a slow but high quality colour compressor (default).
        /// </summary>
        ColourClusterFit = (1 << 4),
        /// <summary>
        /// Use a fast but low quality colour compressor.
        /// </summary>
        ColourRangeFit = (1 << 5),
        /// <summary>
        /// Use a perceptual metric for colour error (default).
        /// </summary>
        ColourMetricPerceptual = (1 << 6),
        /// <summary>
        /// Use a uniform metric for colour error.
        /// </summary>
        ColourMetricUniform = (1 << 7),
        /// <summary>
        /// Weight the colour by alpha during cluster fit (off by default).
        /// </summary>
        WeightColourByAlpha = (1 << 8),
    }
    public static class SquishOptionsExtensions {
        public static SquishOptions GetMethod(this SquishOptions self) {
            return (self & (SquishOptions.DXT1 | SquishOptions.DXT3 | SquishOptions.DXT5));
        }
        public static SquishOptions GetFit(this SquishOptions self) {
            return (self & (SquishOptions.ColourIterativeClusterFit | SquishOptions.ColourClusterFit | SquishOptions.ColourRangeFit));
        }
        public static SquishOptions GetMetric(this SquishOptions self) {
            return (self & (SquishOptions.ColourMetricPerceptual | SquishOptions.ColourMetricUniform));
        }
        public static SquishOptions GetExtra(this SquishOptions self) {
            return (self & (SquishOptions.WeightColourByAlpha));
        }
    }
}
