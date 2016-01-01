using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	internal class StringConverter : IStringConverter
	{
		public string ConvertToString<T>(T value)
		{
			if (typeof(T) == typeof(string))
			{
				return value as string;
			}
			if (typeof(T).GetTypeInfo().IsPrimitive)
			{
				return (string)Convert.ChangeType(value, typeof(string));
			}

			using (var ms = new MemoryStream())
			{
				var serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(ms, value);

				ArraySegment<byte> buff;
				ms.TryGetBuffer(out buff); // This result should be always true.

				return Convert.ToBase64String(buff.Array, buff.Offset, buff.Count);
			}
		}

		public T ConvertFromString<T>(string value)
		{
			if (typeof(T) == typeof(string))
			{
				return (T)(object)value;
			}
			if (typeof(T).GetTypeInfo().IsPrimitive)
			{
				return (T)Convert.ChangeType(value, typeof(T));
			}

			using (var ms = new MemoryStream())
			{
				var buff = Convert.FromBase64String(value);

				ms.Write(buff, 0, buff.Length);
				ms.Seek(0, SeekOrigin.Begin);

				var serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(ms);
			}
		}
	}
}