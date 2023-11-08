using System.Collections.Generic;

namespace SaintCoinach.Ex.Relational.Definition.EXDSchema;

/// <summary>
/// Useful methods for working with the EXDSchema object model.
/// </summary>
public static class SchemaUtil
{
	public static int GetColumnCount(Sheet sheet)
	{
		var total = 0;
		foreach (var field in sheet.Fields)
			total += GetFieldCount(field);
		return total;
	}

    // public static Sheet PostProcess(Sheet sheet)
    // {
    //     int index = 0;
    //     for (int i = 0; i < sheet.Fields.Count; i++)
    //     {
    //         sheet.Fields[i] = PostProcess(sheet.Fields[i], index);
    //         index += sheet.Fields[i].FieldCount;
    //     }
    //     return sheet;
    // }
    //
    // public static Field PostProcess(Field field, int index)
    // {
    //     field.Index = index;
    //     field.FieldCount = GetFieldCount(field);
    //     return field;
    // }
    
    public static Sheet Flatten(Sheet sheet)
    {
        var fields = new List<Field>();
        foreach (var field in sheet.Fields)
            Emit(fields, field);

        sheet.Fields = fields;
        for (int i = 0; i < sheet.Fields.Count; i++)
        {
            sheet.Fields[i].Index = i;
        }
        return sheet;
    }

    private static string GetFieldName()
    {
        return "";
    }

    private static void Emit(List<Field> list, Field field, List<string> hierarchy = null)
	{
		if (field.Type != FieldType.Array)
		{
			// Single field
			list.Add(CreateField(field, 0, hierarchy));
		}
		else if (field.Type == FieldType.Array)
		{
			// We can have an array without fields, it's just scalars
			if (field.Fields == null)
			{
				for (int i = 0; i < field.Count.Value; i++)
				{
					list.Add(CreateField(field, i, hierarchy));	
				}
			}
			else
			{
				for (int i = 0; i < field.Count.Value; i++)
                {
                    var usableHierarchy = hierarchy == null ? new List<string>() : new List<string>(hierarchy);
					foreach (var nestedField in field.Fields)
                    {
                        usableHierarchy.Add($"{field.Name}[{i}]");
						Emit(list, nestedField, usableHierarchy);
					}	
				}
			}
		}
	}

	private static Field CreateField(Field baseField, int index, List<string> hierarchy)
	{
		var addedField = new Field
		{
			Comment = baseField.Comment,
			Count = null,
			Type = baseField.Type == FieldType.Array ? FieldType.Scalar : baseField.Type,
			Fields = null,
			Condition = baseField.Condition,
			Targets = baseField.Targets,
		};

		var name = $"{baseField.Name}";
		
		if (index != 0)
		{
			name = $"{name}[{index}]";
		}

		if (hierarchy != null)
		{
			if (!string.IsNullOrEmpty(name))
				hierarchy.Add(name);
			addedField.Name = string.Join(".", hierarchy);	
		}
		else
		{
			addedField.Name = name;
		}
		return addedField;
	}

	private static int GetFieldCount(Field field)
	{
		if (field.Type == FieldType.Array)
		{
			var total = 0;
			if (field.Fields != null)
			{
				foreach (var nestedField in field.Fields)
					total += GetFieldCount(nestedField);
			}
			else
			{
				total = 1;
			}
			return total * field.Count.Value;
		}
		return 1;
	}
}