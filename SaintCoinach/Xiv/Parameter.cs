using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a collection of bonuses for a specific <see cref="BaseParam" />.
    /// </summary>
    public class Parameter : IEnumerable<ParameterValue> {
        #region Fields

        /// <summary>
        ///     List of bonuses for the current parameter.
        /// </summary>
        private readonly List<ParameterValue> _Values = new List<ParameterValue>();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="BaseParam" /> associated with the current object.
        /// </summary>
        /// <value>The <see cref="BaseParam" /> associated with the current object.</value>
        public BaseParam BaseParam { get; private set; }

        /// <summary>
        ///     Gets the bonuses of the current object.
        /// </summary>
        /// <value>The bonuses of the current object.</value>
        public IEnumerable<ParameterValue> Values { get { return _Values; } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Parameter" /> class.
        /// </summary>
        /// <param name="baseParam"><see cref="BaseParam" /> to associate with the <see cref="Parameter" />.</param>
        public Parameter(BaseParam baseParam) {
            BaseParam = baseParam;
        }

        #endregion

        #region IEnumerable<ParameterValue> Members

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="Parameter"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="Parameter"/>.</returns>
        public IEnumerator<ParameterValue> GetEnumerator() {
            return _Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="Parameter"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="Parameter"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Add

        /// <summary>
        ///     Add a value to the current <see cref="Parameter" />.
        /// </summary>
        /// <param name="value"><see cref="ParameterValue" /> to add.</param>
        public void AddValue(ParameterValue value) {
            _Values.Add(value);
        }

        #endregion
    }
}
