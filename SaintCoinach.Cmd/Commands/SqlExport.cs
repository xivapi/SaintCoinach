using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaintCoinach.Xiv;
using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands
{
    public class SqlExport : AsyncActionCommandBase
    {
        private ARealmReversed _Realm;
        
        public SqlExport(ARealmReversed realm) : 
            base("sql", "Exports the EXD data as SQL schema and associated imports.")
        {
            _Realm = realm;
        }

        private string GetSqlType(Type type)
        {
            switch (type.Name)
            {
                case "UInt32":
                    return "INT UNSIGNED";
                case "Int32":
                    return "INT";
                case "SByte":
                    return "TINYINT";
                case "Byte":
                    return "TINYINT UNSIGNED";
                case "XivString":
                    return "TEXT";
                case "Boolean":
                    return "BIT";
                case "UInt16":
                    return "SMALLINT UNSIGNED";
                case "Int16":
                    return "SMALLINT";
                case "Single":
                    return "FLOAT";
                case "Double":
                    return "DOUBLE";
                case "Quad":
                    return "BIGINT UNSIGNED";
                
                default:
                    throw new NotImplementedException( $"The type {type.Name} doesn't have an SQL type mapping");
            }
        }
        
        static bool IsUnescaped(object self) {
            return (self is Boolean
                    || self is Byte
                    || self is SByte
                    || self is Int16
                    || self is Int32
                    || self is Int64
                    || self is UInt16
                    || self is UInt32
                    || self is UInt64
                    || self is Single
                    || self is Double);
        }

        private string GetTableName(ISheet sheet)
        {
            return $"`{sheet.Name.Replace("/", "_")}`";
        }

        private void DoRowData(ISheet sheet, XivRow row, List<string> data, StringBuilder sb)
        {
            for (int i = 0; i < sheet.Header.ColumnCount; i++)
            {
                var o = row.GetRaw(i);

                if (o is Quad)
                {
                    var q = (Quad)o;
                    data.Add(q.ToInt64().ToString());
                    continue;
                }

                if (IsUnescaped(o))
                {
                    data.Add(o.ToString());
                    continue;
                }
                    
                string d = o.ToString();
                if (string.IsNullOrEmpty(d))
                    d = "NULL";
                else
                {
                    d = $"'{d.Replace("'", "\\'")}'";
                }
                data.Add(d);
            }
            
            sb.AppendLine($"  ( {string.Join(", ", data)} ),");
        }

        private void WriteVariant1Rows(ISheet sheet, StringBuilder sb)
        {
            var rows = sheet.Cast<XivRow>();
            var cols = new List<string>();
            
            cols.Add("`_Key`");

            foreach (var col in sheet.Header.Columns.Cast<RelationalColumn>())
            {
                string name = string.IsNullOrEmpty(col.Name) ? $"unk{col.Index}" : col.Name;
                
                cols.Add($"`{name}`");
            }

            sb.AppendLine($"INSERT INTO {GetTableName(sheet)} ({string.Join(", ", cols)}) VALUES ");
            
            foreach (var row in rows)
            {
                var data = new List<string>();
                data.Add(row.Key.ToString());

                DoRowData(sheet, row, data, sb);
            }

            sb.Remove(sb.Length - 3, 3);
            sb.AppendLine(";");
        }

        private void WriteVairant2Rows(ISheet sheet, StringBuilder sb)
        {
            var rows = sheet.Cast<XivSubRow>();
            var cols = new List<string>();
            
            cols.Add("`_Key`");
            cols.Add("`_SubKey`");

            foreach (var col in sheet.Header.Columns.Cast<RelationalColumn>())
            {
                string name = string.IsNullOrEmpty(col.Name) ? $"unk{col.Index}" : col.Name;
                
                cols.Add($"`{name}`");
            }

            sb.AppendLine($"INSERT INTO {GetTableName(sheet)} ({string.Join(", ", cols)}) VALUES ");

            foreach (var row in rows)
            {
                var data = new List<string>();
                data.Add(row.ParentRow.Key.ToString());
                data.Add(row.Key.ToString());

                DoRowData(sheet, row, data, sb);
            }

            sb.Remove(sb.Length - 3, 3);
            sb.AppendLine(";");
        }

        private void WriteRows(ISheet sheet, StringBuilder sb)
        {
            if (sheet.Header.Variant == 1)
                WriteVariant1Rows(sheet, sb);
            else
                WriteVairant2Rows(sheet, sb);
        }

        public override async Task InvokeAsync(string[] paramList)
        {
            var imports = new List<string>();
            
            // .Where(n => !n.Contains("quest/") && !n.Contains("custom/"))
            foreach (var name in _Realm.GameData.AvailableSheets)
            {
                var sheet = _Realm.GameData.GetSheet(name);
                var variant = sheet.Header.Variant;
                var sheet2 = sheet as XivSheet2<XivSubRow>;
                
                Console.WriteLine($"Sheet: {name}, variant: {variant}");

                if (sheet.Count == 0)
                    continue;

                var sb = new StringBuilder();

                sb.AppendLine($"CREATE TABLE {GetTableName(sheet)} (");
                
                // key meme
                if (sheet.Header.Variant == 1)
                {
                    sb.AppendLine($"  `_Key` INT NOT NULL,");
                }
                else
                {
                    sb.AppendLine($"  `_Key` INT NOT NULL,");
                    sb.AppendLine($"  `_SubKey` INT NOT NULL,");
                }
                
                // add cols
                foreach (var column in sheet.Header.Columns)
                {
                    var colName = column.Name;
                    if (string.IsNullOrEmpty(colName))
                        colName = $"unk{column.Index}";
                    
                    sb.AppendLine($"  `{colName}` {GetSqlType(column.Reader.Type)},");
                }
                
                // primary key
                if (sheet.Header.Variant == 1)
                {
                    sb.AppendLine($"  PRIMARY KEY (`_Key`)");
                }
                else
                {
                    sb.AppendLine($"  PRIMARY KEY (`_Key`, `_SubKey`)");
                }
            
                sb.AppendLine(") COLLATE='utf8mb4_unicode_ci' ENGINE=MyISAM;");
                sb.AppendLine();
                
                WriteRows(sheet, sb);
                
                imports.Add(sb.ToString());
            }
            
            File.WriteAllText("schema.sql", string.Join(Environment.NewLine, imports));
        }
    }
}