using Simple.InputEx;
using UnityEngine;
using UnityEngine.Networking.Match;

public class TranslateByInputTest : MonoBehaviour
{
	public bool InputReplay = false;
	public Collider Collider;

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

		if (InputEx.GetMouseButtonDown(0))
		{
			var mousePos = InputEx.GetMousePosition();
			if (Collider != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(mousePos);
				RaycastHit hit;
				if (Collider.Raycast(ray, out hit, 1000))
				{
					Debug.Log("MouseClicked in Sphere");
				}
			}
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
