using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace YamlDotNet
{
#if PORTABLE
	/// <summary>
	/// Mock SerializableAttribute to avoid having to add #if all over the place
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class SerializableAttribute : Attribute { }

	internal static class ReflectionExtensions
	{
		public static bool IsValueType(this Type type)
		{
			return type.GetTypeInfo().IsValueType;
		}

		public static bool IsGenericType(this Type type)
		{
			return type.GetTypeInfo().IsGenericType;
		}

		public static bool IsInterface(this Type type)
		{
			return type.GetTypeInfo().IsInterface;
		}

		public static bool IsEnum(this Type type)
		{
			return type.GetTypeInfo().IsEnum;
		}

		/// <summary>
		/// Determines whether the specified type has a default constructor.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the type has a default constructor; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasDefaultConstructor(this Type type)
		{
			var typeInfo = type.GetTypeInfo();
			return typeInfo.IsValueType || typeInfo.DeclaredConstructors
				.Any(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length == 0);
		}

		public static bool IsAssignableFrom(this Type type, Type source)
		{
			return type.GetTypeInfo().IsAssignableFrom(source.GetTypeInfo());
		}

		public static TypeCode GetTypeCode(this Type type)
		{
			if (type.IsEnum())
			{
				type = Enum.GetUnderlyingType(type);
			}

			if (type == typeof(bool))
			{
				return TypeCode.Boolean;
			}
			else if (type == typeof(char))
			{
				return TypeCode.Char;
			}
			else if (type == typeof(sbyte))
			{
				return TypeCode.SByte;
			}
			else if (type == typeof(byte))
			{
				return TypeCode.Byte;
			}
			else if (type == typeof(short))
			{
				return TypeCode.Int16;
			}
			else if (type == typeof(ushort))
			{
				return TypeCode.UInt16;
			}
			else if (type == typeof(int))
			{
				return TypeCode.Int32;
			}
			else if (type == typeof(uint))
			{
				return TypeCode.UInt32;
			}
			else if (type == typeof(long))
			{
				return TypeCode.Int64;
			}
			else if (type == typeof(ulong))
			{
				return TypeCode.UInt64;
			}
			else if (type == typeof(float))
			{
				return TypeCode.Single;
			}
			else if (type == typeof(double))
			{
				return TypeCode.Double;
			}
			else if (type == typeof(decimal))
			{
				return TypeCode.Decimal;
			}
			else if (type == typeof(DateTime))
			{
				return TypeCode.DateTime;
			}
			else if (type == typeof(String))
			{
				return TypeCode.String;
			}
			else
			{
				return TypeCode.Object;
			}
		}

		public static Type[] GetGenericArguments(this Type type)
		{
			return type.GetTypeInfo().GenericTypeArguments;
		}

		public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
		{
			return type.GetRuntimeProperties()
				.Where(p => p.GetMethod.IsPublic && !p.GetMethod.IsStatic);
		}

		public static IEnumerable<MethodInfo> GetPublicMethods(this Type type)
		{
			return type.GetRuntimeMethods()
				.Where(m => m.IsPublic && !m.IsStatic);
		}

		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes)
		{
			return type.GetRuntimeMethods()
				.FirstOrDefault(m =>
				{
					if (m.IsPublic && m.IsStatic && m.Name.Equals(name))
					{
						var parameters = m.GetParameters();
						return parameters.Length == parameterTypes.Length
							&& parameters.Zip(parameterTypes, (pi, pt) => pi.Equals(pt)).All(r => r);
					}
					return false;
				});
		}

		public static MethodInfo GetGetMethod(this PropertyInfo property)
		{
			return property.GetMethod;
		}

		public static IEnumerable<Type> GetInterfaces(this Type type)
		{
			return type.GetTypeInfo().ImplementedInterfaces;
		}

		public static Exception Unwrap(this TargetInvocationException ex)
		{
			return ex.InnerException;
		}
	}

	internal enum TypeCode
	{
		Empty = 0,
		Object = 1,
		DBNull = 2,
		Boolean = 3,
		Char = 4,
		SByte = 5,
		Byte = 6,
		Int16 = 7,
		UInt16 = 8,
		Int32 = 9,
		UInt32 = 10,
		Int64 = 11,
		UInt64 = 12,
		Single = 13,
		Double = 14,
		Decimal = 15,
		DateTime = 16,
		String = 18,
	}

	internal static class StandardRegexOptions
	{
		public const RegexOptions Compiled = RegexOptions.None;
	}

	internal abstract class DBNull
	{
		private DBNull() {}
	}

	internal sealed class CultureInfoAdapter : CultureInfo
	{
		private readonly IFormatProvider _provider;

		public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider)
			: base(baseCulture.Name)
		{
			_provider = provider;
		}

		public override object GetFormat(Type formatType)
		{
			return _provider.GetFormat(formatType);
		}
	}
#else

    internal static class ReflectionExtensions
    {
        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }

		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        /// Determines whether the specified type has a default constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the type has a default constructor; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasDefaultConstructor(this Type type)
        {
            return type.IsValueType || type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null) != null;
        }

		public static TypeCode GetTypeCode(this Type type)
		{
			return Type.GetTypeCode(type);
		}
 
		public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}

		public static IEnumerable<MethodInfo> GetPublicMethods(this Type type)
		{
			return type.GetMethods(BindingFlags.Static | BindingFlags.Public);
		}

		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes)
		{
			return type.GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);
		}

		private static readonly FieldInfo remoteStackTraceField = typeof(Exception)
				.GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

		public static Exception Unwrap(this TargetInvocationException ex)
		{
			var result = ex.InnerException;
			if (remoteStackTraceField != null)
			{
				remoteStackTraceField.SetValue(ex.InnerException, ex.InnerException.StackTrace + "\r\n");
			}
			return result;
		}
	}
	
	internal static class StandardRegexOptions
	{
		public const RegexOptions Compiled = RegexOptions.Compiled;
	}

	internal sealed class CultureInfoAdapter : CultureInfo
	{
		private readonly IFormatProvider _provider;

		public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider)
			: base(baseCulture.LCID)
		{
			_provider = provider;
		}

		public override object GetFormat(Type formatType)
		{
			return _provider.GetFormat(formatType);
		}
	}

#endif
}
