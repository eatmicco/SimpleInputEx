using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Simple.Serializer;

namespace Simple.InputEx
{
	public class InputEx
	{
		private static bool _isReplay;
		private static bool _isHold;
		private static int _replayIndex;

		private static InputDataList _inputData = new InputDataList()
		{
			List = new List<InputState>()
		};

		public static void Replay(bool replay)
		{
			_isReplay = replay;
			_replayIndex = 0;

			if (_isReplay)
			{
				Load();
			}
		}

		public static bool GetKeyDown(KeyCode keyCode)
		{
			if (_isHold)
			{
				return false;
			}

			if (!_isReplay)
			{
				if (Input.GetKeyDown(keyCode))
				{
					var notOkToSave = false;
					if (_inputData.List.Count > 0)
					{
						var lastInputState = _inputData.List[_inputData.List.Count - 1];
						notOkToSave = lastInputState.Type == InputType.Keyboard &&
										 lastInputState.KeyCode == (int)keyCode &&
										 lastInputState.CurrentState == ButtonState.Down;						
					}
					if (!notOkToSave)
					{
						// Save the state
						_inputData.List.Add(new InputState
						{
							Type = InputType.Keyboard,
							KeyCode = (int)keyCode,
							SavedState = ButtonState.Down,
							CurrentState = ButtonState.Down
						});
					}
					return true;
				}

				return false;
			}
			else
			{
				if (_inputData.List.Count > 0 && _replayIndex < _inputData.List.Count)
				{
					var keyIsUp = true;
					if (_replayIndex > 1)
					{
						var lastInputState = _inputData.List[_replayIndex - 1];
						if (lastInputState.CurrentState != ButtonState.Up)
						{
							keyIsUp = false;
						}
					}
					var currentInputState = _inputData.List[_replayIndex];
					var isKeyDown = currentInputState.Type == InputType.Keyboard &&
					                currentInputState.KeyCode == (int) keyCode &&
					                currentInputState.SavedState == ButtonState.Down;
					if (isKeyDown && keyIsUp)
					{
						_replayIndex++;
						return true;
					}
				}
				return false;
			}
		}

		// We won't be able to replay KeyUp if there's no KeyDown, so for now we disregard any KeyUp recording
		public static bool GetKeyUp(KeyCode keyCode)
		{
			if (_isHold)
			{
				return false;
			}

			return Input.GetKeyUp(keyCode);
		}

		public static bool GetMouseButtonDown(int button)
		{
			if (_isHold)
			{
				return false;
			}

			if (!_isReplay)
			{

			}
			else
			{
				return false;
			}

			return false;
		}

		// Put the Update method in the end of Update in MonoBehaviour
		public static void LateUpdate()
		{
			// Reset all current state
			foreach (var inputState in _inputData.List)
			{
				inputState.CurrentState = ButtonState.Up;
			}
		}

		public static void Hold()
		{
			_isHold = true;
		}

		public static void Release()
		{
			_isHold = false;
		}

		public static void Save()
		{
			if (!_isReplay)
			{
				var json = JsonSerializer.Serialize(_inputData);

				var path = Application.dataPath + "/Data/inputdata.json";
				// Delete the file if exists
				if (File.Exists(path))
				{
					File.Delete(path);
				}

				using (var fileStream = File.OpenWrite(path))
				{
					var streamWriter = new StreamWriter(fileStream);
					streamWriter.Write(json.ToJSON(0));

					streamWriter.Flush();
					streamWriter.Close();
				}
			}
		}

		public static void Load()
		{
			using (var streamReader = File.OpenText(Application.dataPath + "/Data/inputdata.json"))
			{
				var jsonString = streamReader.ReadToEnd();
				_inputData = JsonSerializer.Deserialize(typeof (InputDataList), jsonString) as InputDataList;
			}
		}
	}
}

