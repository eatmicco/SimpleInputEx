using UnityEngine;
using System.Collections.Generic;

namespace Simple.InputEx
{
	public class InputEx
	{
		private static bool _isReplay;
		private static bool _isHold;
		private static int _replayIndex;
		private static readonly List<InputState> _inputStates = new List<InputState>();

		public static void Replay(bool replay)
		{
			_isReplay = replay;
			_replayIndex = 0;
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
					var lastInputState = _inputStates[_inputStates.Count - 1];
					var notOkToSave = lastInputState.Type == InputType.Keyboard &&
					                 lastInputState.KeyCode == (int)keyCode &&
					                 lastInputState.CurrentState == ButtonState.Down;
					if (!notOkToSave)
					{
						// Save the state
						_inputStates.Add(new InputState
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
				var currentInputState = _inputStates[_replayIndex];
				var isKeyDown = currentInputState.Type == InputType.Keyboard &&
									 currentInputState.KeyCode == (int)keyCode &&
									 currentInputState.CurrentState == ButtonState.Down;
				if (isKeyDown)
				{
					return true;
				}
				return false;
			}
		}

		public static bool GetKeyUp(KeyCode keyCode)
		{
			if (_isHold)
			{
				return false;
			}

			if (!_isReplay)
			{
				if (Input.GetKeyUp(keyCode))
				{
					var lastInputState = _inputStates[_inputStates.Count - 1];
					var notOkToSave = lastInputState.Type == InputType.Keyboard &&
					                  lastInputState.KeyCode == (int)keyCode &&
					                  lastInputState.CurrentState == ButtonState.Up;
					if (!notOkToSave)
					{
						// Save the state
						_inputStates.Add(new InputState
						{
							Type = InputType.Keyboard,
							KeyCode = (int)keyCode,
							SavedState = ButtonState.Down,
							CurrentState = ButtonState.Down
						});
					}
					return true;
				}
			}
			else
			{
				var currentInputState = _inputStates[_replayIndex];
				var isKeyUp = currentInputState.Type == InputType.Keyboard &&
									 currentInputState.KeyCode == (int)keyCode &&
									 currentInputState.CurrentState == ButtonState.Up;
				if (isKeyUp)
				{
					return true;
				}
				return false;
			}

			return false;
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
		public static void Update()
		{
			if (_isReplay)
			{
				_replayIndex++;
			}

			// Reset all current state
			foreach (var inputState in _inputStates)
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
			
		}
	}
}

