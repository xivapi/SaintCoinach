using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Regions;

namespace Thaliak.Services {
    public abstract class ViewSourceBase : IViewSource {
        #region Reg
        protected class Registration {
            public Type Type { get; set; }
            public IEnumerable<string> Contexts { get; set; }
            public bool AcceptDerivedTypes { get; set; }
            public string BaseUri { get; set; }
            public UriKind UriKind { get; set; }
        }
        #endregion

        #region Fields
        private List<Registration> _Registrations = new List<Registration>();
        private Dictionary<Tuple<Type, string>, Registration> _DirectMapping = new Dictionary<Tuple<Type, string>, Registration>();
        private Dictionary<Tuple<Type, string>, Registration> _DerivedMapping = new Dictionary<Tuple<Type, string>, Registration>();
        private IObjectStore _TemporaryStorage;
        #endregion

        #region Constructor
        protected ViewSourceBase(IObjectStore temporaryStorage) {
            if (temporaryStorage == null)
                throw new ArgumentNullException("temporaryStorage");
            _TemporaryStorage = temporaryStorage;
        }
        #endregion

        #region Helpers
        protected void Register(Registration reg) {
            _Registrations.Add(reg);
            foreach (var ctx in reg.Contexts) {
                _DirectMapping.Add(Tuple.Create(reg.Type, ctx), reg);
                if (reg.AcceptDerivedTypes) {
                    foreach (var d in _DerivedMapping.Where(_ => _.Key.Item2 == ctx && reg.Type.IsAssignableFrom(_.Key.Item1)).ToArray())
                        _DerivedMapping.Remove(d.Key);
                }
            }
        }
        protected void Register(Type type, string baseUri, bool acceptDerivedTypes, params string[] contexts) {
            Register(type, baseUri, baseUri.Contains("://") ? UriKind.Absolute : UriKind.Relative, acceptDerivedTypes, contexts);
        }
        protected void Register(Type type, string baseUri, UriKind uriKind, bool acceptDerivedTypes, params string[] contexts) {
            Register(new Registration {
                Type = type,
                BaseUri = baseUri,
                UriKind = uriKind,
                AcceptDerivedTypes = acceptDerivedTypes,
                Contexts = contexts.ToArray()
            });
        }
        #endregion

        #region IViewSource Members
        public bool IsHandled(Type modelType, string context) {
            return GetRegistration(modelType, context) != null;
        }

        public Uri GetViewUri(Type modelType, object value, string context) {
            var reg = GetRegistration(modelType, context);
            if (reg == null)
                throw new NotSupportedException();

            var storeId = _TemporaryStorage.Store(value);
            var param = new NavigationParameters();
            param.Add("ID", storeId.ToString());
            return new Uri(reg.BaseUri + param, reg.UriKind);
        }

        private Registration GetRegistration(Type modelType, string context) {
            var key = Tuple.Create(modelType, context);

            if(_DirectMapping.ContainsKey(key))
                return _DirectMapping[key];
            if(_DerivedMapping.ContainsKey(key))
                return _DerivedMapping[key];

            var subs = from reg in _Registrations
                       where reg.Contexts.Contains(context) && reg.AcceptDerivedTypes && reg.Type.IsAssignableFrom(modelType)
                       select new {
                           Registration = reg,
                           Distance = GetTypeInheritanceDistance(reg.Type, modelType)
                       };
            if (subs.Any()) {
                var reg = (from i in subs
                           orderby i.Distance
                           select i.Registration).First();
                _DerivedMapping.Add(key, reg);
                return reg;
            }
            return null;
        }
        private int GetTypeInheritanceDistance(Type baseType, Type derivedType) {
            if (baseType.IsInterface)
                return 0;

            var c = 0;
            Type check = derivedType.BaseType;
            while (check != null) {
                if (check == baseType)
                    break;
                ++c;
                check = check.BaseType;
            }
            if (check == null)
                c = int.MaxValue;
            return c;
        }
        #endregion
    }
}
