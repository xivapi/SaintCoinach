// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharpYaml;
using SharpYaml.Serialization;

namespace SaintCoinach.Ex.Relational.Definition.EXDSchema;

public enum FieldType
{
	Scalar,
	Array,
	Icon,
	ModelId,
	Color,
	Link,
}

public class Sheet
{
	[YamlMember(0)]
	public string Name { get; set; }

	[YamlMember(1)]
	public string? DisplayField { get; set; }

	[YamlMember(2)]
	public List<Field> Fields { get; set; }
}

public class Field
{
    [YamlIgnore]
    public int Index;

    [YamlIgnore]
    public int FieldCount;
    
	[YamlMember(0)]
	public string? Name { get; set; }

	[YamlMember(1)]
	public int? Count { get; set; }

	[YamlMember(2)]
	[DefaultValue(FieldType.Scalar)]
	[JsonConverter(typeof(StringEnumConverter), true)]
	public FieldType Type { get; set; }

	[YamlMember(3)]
	public string? Comment { get; set; }

	[YamlMember(4)]
	public List<Field>? Fields { get; set; }

	[YamlMember(5)]
	public Condition? Condition { get; set; }

	[YamlMember(6)]
	[YamlStyle(YamlStyle.Flow)]
	public List<string>? Targets { get; set; }
}

public class Condition
{
	[YamlMember(0)]
	public string? Switch { get; set; }

	[YamlMember(1)]
	public Dictionary<int, List<string>>? Cases { get; set; }
}