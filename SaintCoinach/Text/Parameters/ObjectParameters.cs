using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Parameters {
    public class ObjectParameters : ParameterBase {
        const int LocalObjectIndex = 1;
        const int SourceObjectIndex = 2;
        const int TargetObjectIndex = 3;

        #region Constructors
        public ObjectParameters() { }
        public ObjectParameters(ParameterBase copyFrom) : base(copyFrom) { }
        #endregion

        #region Properties
        public object LocalObject { get { return this[LocalObjectIndex]; } }
        public object SourceObject { get { return this[SourceObjectIndex]; } }
        public object TargetObject { get { return this[TargetObjectIndex]; } }
        #endregion
    }
}
