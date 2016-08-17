using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.Serializer;

namespace Simple.Test.UnitTest
{
	[TestClass]
	public class SerializerTest
	{
		public enum FooEnum
		{
			Item1,
			Item2,
			Item3
		}

		[JsonSerializable]
		public class Foo
		{
			[JsonNode("Data1")]
			public int Data1 { get; set; }

			[JsonNode("Data2")]
			public float Data2 { get; set; }
		}

		[JsonSerializable]
		public class Bar
		{
			[JsonNode("Data1")]
			public string Data1 { get; set; }

			[JsonNode("Foo1")]
			public Foo Foo1 { get; set; }
		}

		[JsonSerializable]
		public class FooBar
		{
			[JsonNode("Data1")]
			public int Data1 { get; set; }

			[JsonNode("Enum1")]
			[JsonEnum(typeof(FooEnum))]
			public FooEnum Enum1 { get; set; }
		}

		[JsonSerializable]
		public class FooBarFoo
		{
			[JsonNode("Data1")]
			public int Data1 { get; set; }

			[JsonNode("List1")]
			[JsonArray(typeof(int))]
			public List<int> List1 { get; set; } 
		}

		[JsonSerializable]
		public class FooBarBar
		{
			[JsonNode("Data1")]
			public int Data1 { get; set; }

			[JsonNode("List1")]
			[JsonArray(typeof(Foo))]
			public List<Foo> List1 { get; set; } 
		}

		[TestMethod]
		public void Serializer_InputObjectOnlyPrimitives_ResultInJson()
		{
			var foo = new Foo()
			{
				Data1 = 1,
				Data2 = 2.5f
			};

			var expectedJson = "{ \n  \"Foo\": { \n    \"Data1\": 1, \n    \"Data2\": 2.5\n    }\n  }";

			var node = JsonSerializer.Serialize(foo);

			var json = node.ToJSON(0);

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public void Serializer_InputObjectComplex_ResultInJson()
		{
			var bar = new Bar()
			{
				Data1 = "FooBar",
				Foo1 = new Foo()
				{
					Data1 = 15,
					Data2 = 3.14f
				}
			};

			var expectedJson = "{ \n  \"Bar\": { \n    \"Data1\": \"FooBar\", \n    \"Foo1\": { \n      \"Data1\": 15, \n      \"Data2\": 3.14\n      }\n    }\n  }";

			var node = JsonSerializer.Serialize(bar);

			var json = node.ToJSON(0);

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public void Serializer_InputObjectWithEnum_ResultInJson()
		{
			var fooBar = new FooBar()
			{
				Data1 = 13,
				Enum1 = FooEnum.Item2
			};

			var expectedJson = "{ \n  \"FooBar\": { \n    \"Data1\": 13, \n    \"Enum1\": \"Item2\"\n    }\n  }";

			var node = JsonSerializer.Serialize(fooBar);

			var json = node.ToJSON(0);

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public void Serializer_InputObjectWithArray_ResultInJson()
		{
			var fooBarFoo = new FooBarFoo()
			{
				Data1 = 10,
				List1 = new List<int>()
				{
					2, 3, 76
				}
			};

			var expectedJson = "{ \n  \"FooBarFoo\": { \n    \"Data1\": 10, \n    \"List1\": [ \n      2, \n      3, \n      76\n      ]\n    }\n  }";

			var node = JsonSerializer.Serialize(fooBarFoo);

			var json = node.ToJSON(0);

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public void Serializer_InputObjectWithArrayOfObjects_ResultInJson()
		{
			var fooBarBar = new FooBarBar()
			{
				Data1 = 24,
				List1 = new List<Foo>()
				{
					new Foo
					{
						Data1 = 33,
						Data2 = 4.5f
					},
					new Foo
					{
						Data1 = 35,
						Data2 = 9.5f
					}
				}
			};

			var expectedJson = "{ \n  \"FooBarBar\": { \n    \"Data1\": 24, \n    \"List1\": [ \n      { \n        \"Data1\": 33, \n        \"Data2\": 4.5\n        }, \n      { \n        \"Data1\": 35, \n        \"Data2\": 9.5\n        }\n      ]\n    }\n  }";

			var node = JsonSerializer.Serialize(fooBarBar);

			var json = node.ToJSON(0);

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public void Deserializer_JsonInput_ResultInObject()
		{
			var jsonString = "{ \n  \"Foo\": { \n    \"Data1\": 1, \n    \"Data2\": 2.5\n    }\n  }";

			var foo = JsonSerializer.Deserialize(typeof(Foo), jsonString) as Foo;

			Assert.AreEqual(1, foo.Data1);
			Assert.AreEqual(2.5f, foo.Data2);
		}

		[TestMethod]
		public void Deserializer_JsonInputWithObject_ResultInObject()
		{
			var jsonString = "{ \n  \"Bar\": { \n    \"Data1\": \"FooBar\", \n    \"Foo1\": { \n      \"Data1\": 15, \n      \"Data2\": 3.14\n      }\n    }\n  }";

			var bar = JsonSerializer.Deserialize(typeof(Bar), jsonString) as Bar;

			Assert.AreEqual("FooBar", bar.Data1);
			Assert.AreEqual(15, bar.Foo1.Data1);
			Assert.AreEqual(3.14f, bar.Foo1.Data2);
		}

		[TestMethod]
		public void Deserializer_JsonInputWithEnum_ResultInObject()
		{
			var jsonString = "{ \n  \"FooBar\": { \n    \"Data1\": 1, \n    \"Enum1\": \"Item1\"\n    }\n  }";

			var fooBar = JsonSerializer.Deserialize(typeof(FooBar), jsonString) as FooBar;

			Assert.AreEqual(1, fooBar.Data1);
			Assert.AreEqual(FooEnum.Item1, fooBar.Enum1);
		}

		[TestMethod]
		public void Deserializer_JsonInputWithArray_ResultInObject()
		{
			var jsonString = "{ \n  \"FooBarFoo\": { \n    \"Data1\": 10, \n    \"List1\": [ \n      2, \n      3, \n      76\n      ]\n    }\n  }";

			var fooBarFoo = JsonSerializer.Deserialize(typeof (FooBarFoo), jsonString) as FooBarFoo;

			Assert.AreEqual(3, fooBarFoo.List1[1]);
			Assert.AreEqual(76, fooBarFoo.List1[2]);
		}

		[TestMethod]
		public void Deserializer_JsonInputWithArrayObject_ResultInObject()
		{
			var jsonString = "{ \n  \"FooBarBar\": { \n    \"Data1\": 24, \n    \"List1\": [ \n      { \n        \"Data1\": 33, \n        \"Data2\": 4.5\n        }, \n      { \n        \"Data1\": 35, \n        \"Data2\": 9.5\n        }\n      ]\n    }\n  }";

			var fooBarBar = JsonSerializer.Deserialize(typeof (FooBarBar), jsonString) as FooBarBar;

			Assert.AreEqual(33, fooBarBar.List1[0].Data1);
			Assert.AreEqual(35, fooBarBar.List1[1].Data1);
		}
	}
}
