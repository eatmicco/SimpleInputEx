using System;

namespace Simple.Serializer
{
	[AttributeUsage(AttributeTargets.Property)]
	public class JsonArrayAttribute : Attribute
	{
		public Type Type { get; set; }

		public JsonArrayAttribute(Type type)
		{
			Type = type;
		}
	}
}
