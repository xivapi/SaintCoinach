using System;
using System.Collections.Generic;
using SharpYaml;
using SharpYaml.Events;
using SharpYaml.Serialization;
using SharpYaml.Serialization.Serializers;

namespace SaintCoinach.Ex.Relational.Definition.EXDSchema;

public static class SerializeUtil
{
	private static readonly Serializer _serializer;

	static SerializeUtil()
	{
		var settings = new SerializerSettings
		{
			EmitAlias = false,
			EmitDefaultValues = false,
			NamingConvention = new CamelCaseNamingConvention(),
			IgnoreNulls = true,
		};
		settings.RegisterSerializer(typeof(Dictionary<int, List<string>>), new CustomDictionarySerializer());
		settings.RegisterSerializer(typeof(FieldType), new CustomFieldTypeSerializer());

		_serializer = new Serializer(settings);
	}

	public static string Serialize(object o)
	{
		return _serializer.Serialize(o);
	}
	
	public static T? Deserialize<T>(string s)
	{
		return _serializer.Deserialize<T>(s);
	}

	public static object? Deserialize(string s)
	{
		return _serializer.Deserialize(s);
	}
}

internal class CustomDictionarySerializer : DictionarySerializer
{
	protected override void WriteDictionaryItem(ref ObjectContext objectContext, KeyValuePair<object, object?> keyValue, KeyValuePair<Type, Type> types)
	{
		objectContext.SerializerContext.WriteYaml(keyValue.Key, types.Key);
		objectContext.SerializerContext.WriteYaml(keyValue.Value, types.Value, YamlStyle.Flow);
	}
}

internal class CustomFieldTypeSerializer : ScalarSerializerBase
{
	public override object? ConvertFrom(ref ObjectContext context, Scalar fromScalar)
	{
		return Enum.Parse<FieldType>(new PascalNamingConvention().Convert(fromScalar.Value));
	}
	
	public override string ConvertTo(ref ObjectContext objectContext)
	{
		return objectContext.Settings.NamingConvention.Convert(objectContext.Instance.ToString());
	}
}