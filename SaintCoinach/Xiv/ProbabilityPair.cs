using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ProbabilityPair<T> {
        #region Fields
        private T _Value;
        private double _Probability;
        #endregion

        #region Properties
        public T Value { get { return _Value; } }
        public double Probability { get { return _Probability; } }
        #endregion

        #region Constructor
        public ProbabilityPair(T value, double probability) {
            _Value = value;
            _Probability = probability;
        }
        #endregion
    }
}
