using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleJSON;

namespace Simple.Serializer
{
	public class JsonSerializer
	{
		public static JSONNode Serialize(object obj)
		{
			var type = obj.GetType();
			if (type.GetCustomAttributes(typeof (JsonSerializableAttribute), false).Any())
			{
				// Create JSONClass
				var rootClass = new JSONClass();
				rootClass.Add(type.Name, new JSONClass());
				CreateClass(rootClass[type.Name], obj);

				return rootClass;
			}
			else
			{
				throw new Exception("The object is not serializable!");
			}

			return null;
		}

		public static object Deserialize(Type type, string json)
		{
			var jsonNode = JSON.Parse(json);

			var obj = jsonNode.AsObject;
			var root = obj.Children.ToArray()[0];

			// Access root
			return Deserialize(type, root);
		}

		private static object Deserialize(Type type, JSONNode node)
		{
			if (!type.GetCustomAttributes(typeof (JsonSerializableAttribute), false).Any())
			{
				throw new Exception("The object target is not serializable!");
			}

			var instance = Activator.CreateInstance(type);

			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				var nodeAttribute = property.GetCustomAttributes(typeof(JsonNodeAttribute), false).FirstOrDefault();
				if (nodeAttribute != null)
				{
					var jsonNodeAttribute = nodeAttribute as JsonNodeAttribute;
					if (IsTypePrimitiveOrString(property.PropertyType))
					{
						object value = null;
						if (property.PropertyType == typeof(bool))
						{
							value = node[jsonNodeAttribute.Name].AsBool;
						}
						else if (property.PropertyType == typeof(int))
						{
							value = node[jsonNodeAttribute.Name].AsInt;
						}
						else if (property.PropertyType == typeof(float))
						{
							value = node[jsonNodeAttribute.Name].AsFloat;
						}
						else if (property.PropertyType == typeof(double))
						{
							value = node[jsonNodeAttribute.Name].AsDouble;
						}
						else if (property.PropertyType == typeof(string))
						{
							value = node[jsonNodeAttribute.Name].Value;
						}
						property.SetValue(instance, value, null);
					}
					else
					{
						// Check enum attribute
						var enumAttribute = property.GetCustomAttributes(typeof (JsonEnumAttribute), false).FirstOrDefault();
						if (enumAttribute != null)
						{
							var jsonEnumAttribute = enumAttribute as JsonEnumAttribute;
							var memberInfos = jsonEnumAttribute.Type.GetMembers(BindingFlags.Public | BindingFlags.Static);
							var value = -1;
							for (int i = 0; i < memberInfos.Length; ++i)
							{
								if (memberInfos[i].Name == node[jsonNodeAttribute.Name].Value)
								{
									value = i;
									break;
								}
							}
							var enumValue = Convert.ChangeType(value, Enum.GetUnderlyingType(jsonEnumAttribute.Type));
							property.SetValue(instance, enumValue, null);
						}
						else
						{
							// Check array attribute
							var arrayAttribute = property.GetCustomAttributes(typeof (JsonArrayAttribute), false).FirstOrDefault();
							if (arrayAttribute != null && IsTypeIList(property.PropertyType))
							{
								var jsonArray = node[jsonNodeAttribute.Name].AsArray;
								var jsonArrayAttribute = arrayAttribute as JsonArrayAttribute;
								if (IsTypePrimitiveOrString(jsonArrayAttribute.Type))
								{
									if (jsonArrayAttribute.Type == typeof(bool))
									{
										var valueList = jsonArray.Children.Select(c => c.AsBool).ToList();
										property.SetValue(instance, valueList, null);
									}
									else if (jsonArrayAttribute.Type == typeof(int))
									{
										var valueList = jsonArray.Children.Select(c => c.AsInt).ToList();
										property.SetValue(instance, valueList, null);
									}
									else if (jsonArrayAttribute.Type == typeof(float))
									{
										var valueList = jsonArray.Children.Select(c => c.AsFloat).ToList();
										property.SetValue(instance, valueList, null);
									}
									else if (jsonArrayAttribute.Type == typeof(double))
									{
										var valueList = jsonArray.Children.Select(c => c.AsDouble).ToList();
										property.SetValue(instance, valueList, null);
									}
									else if (jsonArrayAttribute.Type == typeof(string))
									{
										var valueList = jsonArray.Children.Select(c => c.Value).ToList();
										property.SetValue(instance, valueList, null);
									}
								}
								else
								{
									var objects = jsonArray.Children.Select(c => c.AsObject);
									Type listType = typeof(List<>).MakeGenericType(jsonArrayAttribute.Type);
									IList objectList = Activator.CreateInstance(listType) as IList;
									foreach (var obj in objects)
									{
										var val = Deserialize(jsonArrayAttribute.Type, (JSONNode)obj);
										objectList.Add(val);
									}
									property.SetValue(instance, objectList, null);
								}
							}
							else
							{
								if (node[jsonNodeAttribute.Name] != null)
								{
									var propertyNode = node[jsonNodeAttribute.Name].AsObject as JSONNode;
									var value = Deserialize(property.PropertyType, propertyNode);
									property.SetValue(instance, value, null);
								}
							}
						}
					}
				}
			}

			return instance;
		}

		private static bool IsTypeIList(Type type)
		{
			return type.GetInterfaces().Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof (IList<>));
		}

		private static bool IsTypePrimitiveOrString(Type type)
		{
			return type == typeof (int) || type == typeof (bool) || type == typeof (float) ||
			       type == typeof (double) || type == typeof (string);
		}

		private static void CreateClass(JSONNode objectClass, object obj)
		{
			var type = obj.GetType();

			var properties = type.GetProperties().Where(p => p.GetCustomAttributes(typeof(JsonNodeAttribute), false).Any());
			foreach (var property in properties)
			{
				var nodeAttribute = property.GetCustomAttributes(typeof (JsonNodeAttribute), false).FirstOrDefault();
				if (nodeAttribute != null)
				{
					var jsonNodeAttribute = nodeAttribute as JsonNodeAttribute;
					if (IsTypePrimitiveOrString(property.PropertyType))
					{
						if (property.PropertyType == typeof(bool))
						{
							var value = Convert.ToBoolean(property.GetValue(obj, null));
							objectClass.Add(jsonNodeAttribute.Name, new JSONData(value));
						}
						else if (property.PropertyType == typeof(int))
						{
							var value = Convert.ToInt32(property.GetValue(obj, null));
							objectClass.Add(jsonNodeAttribute.Name, new JSONData(value));
						}
						else if (property.PropertyType == typeof(float))
						{
							var value = Convert.ToSingle(property.GetValue(obj, null));
							objectClass.Add(jsonNodeAttribute.Name, new JSONData(value));
						}
						else if (property.PropertyType == typeof(double))
						{
							var value = Convert.ToSingle(property.GetValue(obj, null));
							objectClass.Add(jsonNodeAttribute.Name, new JSONData(value));
						}
						else if (property.PropertyType == typeof(string))
						{
							var value = property.GetValue(obj, null).ToString();
							objectClass.Add(jsonNodeAttribute.Name, new JSONData(value));
						}
						
					}
					else
					{
						// Check enum attribute
						var enumAttribute = property.GetCustomAttributes(typeof (JsonEnumAttribute), false).FirstOrDefault();
						if (enumAttribute != null)
						{
							var jsonEnumAttribute = enumAttribute as JsonEnumAttribute;
							var memberInfos = jsonEnumAttribute.Type.GetMembers(BindingFlags.Public | BindingFlags.Static);
							var enumValue = property.GetValue(obj, null);
							var stringValue = memberInfos[(int) enumValue].Name;
							objectClass.Add(jsonNodeAttribute.Name, new JSONData(stringValue));
						}
						else
						{
							// Check list attribute
							var arrayAttribute = property.GetCustomAttributes(typeof(JsonArrayAttribute), false).FirstOrDefault();
							if (arrayAttribute != null && IsTypeIList(property.PropertyType))
							{
								var jsonArrayAttribute = arrayAttribute as JsonArrayAttribute;
								if (IsTypePrimitiveOrString(jsonArrayAttribute.Type))
								{
									var jsonArray = new JSONArray();
									if (jsonArrayAttribute.Type == typeof(bool))
									{
										var valueList = (List<bool>)property.GetValue(obj, null);
										foreach (var val in valueList)
										{
											jsonArray.Add(new JSONData((bool)val));
										}
									}
									else if (jsonArrayAttribute.Type == typeof(int))
									{
										var valueList = (List<int>)property.GetValue(obj, null);
										foreach (var val in valueList)
										{
											jsonArray.Add(new JSONData(val));
										}
									}
									else if (jsonArrayAttribute.Type == typeof(float))
									{
										var valueList = (List<float>)property.GetValue(obj, null);
										foreach (var val in valueList)
										{
											jsonArray.Add(new JSONData(val));
										}
									}
									else if (jsonArrayAttribute.Type == typeof(double))
									{
										var valueList = (List<double>)property.GetValue(obj, null);
										foreach (var val in valueList)
										{
											jsonArray.Add(new JSONData(val));
										}
									}
									else if (jsonArrayAttribute.Type == typeof(string))
									{
										var valueList = (List<string>)property.GetValue(obj, null);
										foreach (var val in valueList)
										{
											jsonArray.Add(new JSONData(val));
										}
									}
									objectClass.Add(jsonNodeAttribute.Name, jsonArray);
								}
								else
								{
									IList valueList = (IList)property.GetValue(obj, null);
									var jsonArray = new JSONArray();
									foreach (var val in valueList)
									{
										var jsonClass = new JSONClass();
										CreateClass(jsonClass, val);
										jsonArray.Add(jsonClass);
									}
									objectClass.Add(jsonNodeAttribute.Name, jsonArray);
								}
							}
							else
							{
								if (property.GetValue(obj, null) != null)
								{
									objectClass.Add(jsonNodeAttribute.Name, new JSONClass());
									CreateClass(objectClass[jsonNodeAttribute.Name], property.GetValue(obj, null));
								}
							}
						}
					}
				}
			}
		}
	}
}