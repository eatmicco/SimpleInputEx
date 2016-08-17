using System;

namespace Simple.Serializer
{
	[AttributeUsage(AttributeTargets.Property)]
	public class JsonNodeAttribute : Attribute
	{
		public string Name { get; set; }

		public JsonNodeAttribute(string name)
		{
			Name = name;
		}
	}
}