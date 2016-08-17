using System.Collections.Generic;
using Simple.Serializer;

namespace Simple.InputEx
{
	[JsonSerializable]
	public class InputDataList
	{
		[JsonNode("List")]
		[JsonArray(typeof(InputState))]
		public List<InputState> List { get; set; } 
	}
}
