using UnityEngine;
using Simple.Serializer;

namespace Simple.InputEx
{
	[JsonSerializable]
	public class InputState
	{
		[JsonNode("Type")]
		[JsonEnum(typeof(InputType))]
		public InputType Type { get; set; }

		[JsonNode("SavedState")]
		[JsonEnum(typeof(ButtonState))]
		public ButtonState SavedState { get; set; }

		[JsonNode("CurrentState")]
		[JsonEnum(typeof(ButtonState))]
		public ButtonState CurrentState { get; set; }

		[JsonNode("KeyCode")]
		public int KeyCode { get; set; }

		[JsonNode("MouseButton")]
		public int MouseButton { get; set; }

		[JsonNode("MousePosition")]
		public Point3 MousePosition { get; set; }
	}
}
