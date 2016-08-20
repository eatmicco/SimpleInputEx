using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.InputEx;
using Simple.Serializer;
using Simple.Test.UnitTest;

namespace Simple.Test.IntegrationTest
{
	[TestClass]
	public class SaveJsonTest
	{
		[TestMethod]
		public void Save_InputData()
		{
			var inputData = new InputDataList()
			{
				List = new List<InputState>()
				{
					new InputState()
					{
						Type = InputType.Keyboard,
						KeyCode = 32,
						SavedState = ButtonState.Down,
						CurrentState = ButtonState.Down
					},
					new InputState()
					{
						Type = InputType.Mouse,
						MouseButton = 0,
						MousePosition = new Point3(10, 10, 10),
						SavedState = ButtonState.Down,
						CurrentState = ButtonState.Down
					},
					new InputState()
					{
						Type = InputType.Keyboard,
						KeyCode = 10,
						SavedState = ButtonState.Down,
						CurrentState = ButtonState.Down
					}
				}
			};

			var expectedJson = "{ \n  \"InputDataList\": { \n    \"List\": [ \n      { \n        \"Type\": \"Keyboard\", \n        \"SavedState\": \"Down\", \n        \"CurrentState\": \"Down\", \n        \"KeyCode\": 32, \n        \"MouseButton\": 0\n        }, \n      { \n        \"Type\": \"Mouse\", \n        \"SavedState\": \"Down\", \n        \"CurrentState\": \"Down\", \n        \"KeyCode\": 0, \n        \"MouseButton\": 0, \n        \"MousePosition\": { \n          \"X\": 10, \n          \"Y\": 10, \n          \"Z\": 10\n          }\n        }, \n      { \n        \"Type\": \"Keyboard\", \n        \"SavedState\": \"Down\", \n        \"CurrentState\": \"Down\", \n        \"KeyCode\": 10, \n        \"MouseButton\": 0\n        }\n      ]\n    }\n  }";

			var node = JsonSerializer.Serialize(inputData);

			var json = node.ToJSON(0);

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public void Load_InputData()
		{
			var jsonString = "{ \n  \"InputDataList\": { \n    \"List\": [ \n      { \n        \"Type\": \"Keyboard\", \n        \"SavedState\": \"Down\", \n        \"CurrentState\": \"Down\", \n        \"KeyCode\": 32, \n        \"MouseButton\": 0\n        }, \n      { \n        \"Type\": \"Mouse\", \n        \"SavedState\": \"Down\", \n        \"CurrentState\": \"Down\", \n        \"KeyCode\": 0, \n        \"MouseButton\": 0, \n        \"MousePosition\": { \n          \"X\": 10, \n          \"Y\": 10, \n          \"Z\": 10\n          }\n        }, \n      { \n        \"Type\": \"Keyboard\", \n        \"SavedState\": \"Down\", \n        \"CurrentState\": \"Down\", \n        \"KeyCode\": 10, \n        \"MouseButton\": 0\n        }\n      ]\n    }\n  }";

			var inputData = JsonSerializer.Deserialize(typeof(InputDataList), jsonString) as InputDataList;

			Assert.AreEqual(32, inputData.List[0].KeyCode);
		}
	}
}
