using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

namespace SaintCoinach.Ex.Relational.Serialization {
    public class ExRelationSerializer : Serializer {
        public ExRelationSerializer(SerializationOptions options = SerializationOptions.None, INamingConvention namingConvention = null)
            : base(options, namingConvention) { }

        protected override IEventEmitter CreateEventEmitter(IEmitter emitter) {
            var writer = new WriterEventEmitter(emitter);
            return new ExRelationEventEmitter(writer);
        }
    }
}
