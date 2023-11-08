using SharpYaml.Serialization;

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
		_serializer = new Serializer(settings);
	}
	public static T? Deserialize<T>(string s)
	{
		return _serializer.Deserialize<T>(s);
	}
}