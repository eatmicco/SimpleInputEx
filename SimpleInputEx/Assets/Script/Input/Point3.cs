using Simple.Serializer;
using UnityEngine;

namespace Simple.InputEx
{
	[JsonSerializable]
	public class Point3
	{
		[JsonNode("X")]
		public float X { get; set; }

		[JsonNode("Y")]
		public float Y { get; set; }

		[JsonNode("Z")]
		public float Z { get; set; }

		public Point3()
		{
			X = 0;
			Y = 0;
			Z = 0;
		}

		public Point3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Point3(Vector3 vector3)
		{
			X = vector3.x;
			Y = vector3.y;
			Z = vector3.z;
		}

		public Vector3 ToVector3()
		{
			return new Vector3(X, Y, Z);
		}
	}
}
