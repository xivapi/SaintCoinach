using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public class EvaluationParameters {
        #region Fields
        private object _FallbackValue = null;
        private Dictionary<DecodeExpressionType, Parameters.ParameterBase> _Parameters = new Dictionary<DecodeExpressionType, Parameters.ParameterBase>();
        private IEvaluationFunctionProvider _FunctionProvider;
        private Parameters.ParameterBase _TopLevelParameters = new Parameters.ParameterBase();
        #endregion

        #region Properties
        public object FallbackValue { get { return _FallbackValue; } set { _FallbackValue = value; } }
        public IEvaluationFunctionProvider FunctionProvider { get { return _FunctionProvider; } }
        public Parameters.ParameterBase TopLevelParameters { get { return _TopLevelParameters; } }
        public Parameters.ParameterBase InputParameters { get { return _Parameters[DecodeExpressionType.IntegerParameter]; } }
        public Parameters.PlayerParameters PlayerParameters { get { return (Parameters.PlayerParameters)_Parameters[DecodeExpressionType.PlayerParameter]; } }
        public Parameters.ParameterBase StringParameters { get { return _Parameters[DecodeExpressionType.StringParameter]; } }
        public Parameters.ObjectParameters ObjectParameters { get { return (Parameters.ObjectParameters)_Parameters[DecodeExpressionType.ObjectParameter]; } }
        #endregion

        #region Constructor
        public EvaluationParameters(IEvaluationFunctionProvider functionProvider) {
            if (functionProvider == null)
                throw new ArgumentNullException("functionProvider");
            _FunctionProvider = functionProvider;
            _Parameters.Add(DecodeExpressionType.IntegerParameter, new Parameters.ParameterBase() { FallbackValue = 0 });
            _Parameters.Add(DecodeExpressionType.PlayerParameter, new Parameters.PlayerParameters());
            _Parameters.Add(DecodeExpressionType.StringParameter, new Parameters.ParameterBase() { FallbackValue = string.Empty });
            _Parameters.Add(DecodeExpressionType.ObjectParameter, new Parameters.ObjectParameters());
        }
        public EvaluationParameters(EvaluationParameters copyFrom) {
            if (copyFrom == null)
                throw new ArgumentNullException("copyFrom");
            _FunctionProvider = copyFrom.FunctionProvider;
            _TopLevelParameters = new Parameters.ParameterBase(copyFrom.TopLevelParameters);
            _Parameters.Add(DecodeExpressionType.IntegerParameter, new Parameters.ParameterBase(copyFrom._Parameters[DecodeExpressionType.IntegerParameter]));
            _Parameters.Add(DecodeExpressionType.PlayerParameter, new Parameters.PlayerParameters(copyFrom._Parameters[DecodeExpressionType.PlayerParameter]));
            _Parameters.Add(DecodeExpressionType.StringParameter, new Parameters.ParameterBase(copyFrom._Parameters[DecodeExpressionType.StringParameter]));
            _Parameters.Add(DecodeExpressionType.ObjectParameter, new Parameters.ObjectParameters(copyFrom._Parameters[DecodeExpressionType.ObjectParameter]));
        }
        #endregion

        #region Indexer
        public object this[DecodeExpressionType type, int index] {
            get {
                Parameters.ParameterBase pb;
                if (!_Parameters.TryGetValue(type, out pb))
                    return FallbackValue;
                return pb[index];
            }
            set {
                Parameters.ParameterBase pb;
                if (!_Parameters.TryGetValue(type, out pb))
                    throw new NotSupportedException();
                pb[index] = value;
            }
        }
        #endregion
    }
}
