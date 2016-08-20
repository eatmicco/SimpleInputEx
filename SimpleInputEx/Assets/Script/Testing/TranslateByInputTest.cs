using Simple.InputEx;
using UnityEngine;

public class TranslateByInputTest : MonoBehaviour
{
	public bool InputReplay = false;

	// Use this for initialization
	void Start () 
	{
		InputEx.Replay(InputReplay);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (InputEx.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.Translate(-0.01f, 0, 0);
		}

		if (InputEx.GetKeyDown(KeyCode.RightArrow))
		{
			transform.Translate(0.01f, 0, 0);
		}
	}

	void LateUpdate()
	{
		InputEx.LateUpdate();
	}

	void OnDestroy()
	{
		Debug.Log("OnDestroy");

		InputEx.Save();
	}
}
