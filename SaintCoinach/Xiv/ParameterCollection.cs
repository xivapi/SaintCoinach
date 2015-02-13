using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a collection of parameter bonuses.
    /// </summary>
    public class ParameterCollection : IEnumerable<Parameter> {
        #region Fields

        /// <summary>
        ///     Dictionary mapping <see cref="BaseParam" /> to their respective <see cref="Parameter" />.
        /// </summary>
        private readonly Dictionary<BaseParam, Parameter> _Parameters = new Dictionary<BaseParam, Parameter>();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Parameter" /> bonuses for the current collection.
        /// </summary>
        /// <value>The <see cref="Parameter" /> bonuses for the current collection.</value>
        public IEnumerable<Parameter> Parameters { get { return _Parameters.Values; } }

        #endregion

        #region Add

        /// <summary>
        ///     Add a value to a specific <see cref="BaseParam" />.
        /// </summary>
        /// <param name="baseParam"><see cref="BaseParam" /> to which to add a value.</param>
        /// <param name="value"><see cref="ParameterValue" /> to add to <c>baseParam</c>.</param>
        public void AddParameterValue(BaseParam baseParam, ParameterValue value) {
            Parameter param;
            if (!_Parameters.TryGetValue(baseParam, out param))
                _Parameters.Add(baseParam, param = new Parameter(baseParam));

            param.AddValue(value);
        }

        /// <summary>
        ///     Copy the values from given <see cref="Parameter" />s to the current collection.
        /// </summary>
        /// <param name="other"><see cref="Parameter" />s from which to add the values.</param>
        public void AddRange(IEnumerable<Parameter> other) {
            foreach (var p in other) {
                foreach (var v in p)
                    AddParameterValue(p.BaseParam, v);
            }
        }

        #endregion

        #region IEnumerable<Parameter> Members

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="ParameterCollection" />.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="ParameterCollection" />.</returns>
        public IEnumerator<Parameter> GetEnumerator() {
            return _Parameters.Values.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="ParameterCollection" />.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="ParameterCollection" />.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
