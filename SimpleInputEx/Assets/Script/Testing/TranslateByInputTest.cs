using Simple.InputEx;
using UnityEngine;

public class TranslateByInputTest : MonoBehaviour
{
	public bool InputReplay = false;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputEx.Replay(InputReplay);

		if (InputEx.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.Translate(-0.01f, 0, 0);
		}

		if (InputEx.GetKeyDown(KeyCode.RightArrow))
		{
			transform.Translate(0.01f, 0, 0);
		}

		InputEx.Update();
	}

	void OnDestroy()
	{
		Debug.Log("OnDestroy");
	}
}
