using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace SaintCoinach.Ex.Relational.Serialization {
    public class ExRelationSerializer : Serializer {
        #region Constructors

        public ExRelationSerializer(SerializationOptions options = SerializationOptions.None,
                                    INamingConvention namingConvention = null)
            : base(options, namingConvention) { }

        #endregion

        protected override IEventEmitter CreateEventEmitter(IEmitter emitter) {
            var writer = new WriterEventEmitter(emitter);
            return new ExRelationEventEmitter(writer);
        }
    }
}
