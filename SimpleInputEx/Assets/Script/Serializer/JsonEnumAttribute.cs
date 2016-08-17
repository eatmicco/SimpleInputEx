using System;

namespace Simple.Serializer
{
	[AttributeUsage(AttributeTargets.Property)]
	public class JsonEnumAttribute : Attribute
	{
		public Type Type { get; set; }

		public JsonEnumAttribute(Type type)
		{
			Type = type;
		}
	}
}
