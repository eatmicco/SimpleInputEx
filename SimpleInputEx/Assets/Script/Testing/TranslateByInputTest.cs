using Simple.InputEx;
using UnityEngine;
using UnityEngine.Networking.Match;

public class TranslateByInputTest : MonoBehaviour
{
	public bool InputReplay = false;
	public Transform DestinationTransform;

	public Collider Collider;

	private Vector3 _initialPosition;

	// Use this for initialization
	void Start ()
	{
		_initialPosition = transform.position;
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
					InputEx.Hold();
					if (transform.position == _initialPosition)
					{
						iTween.MoveTo(gameObject, iTween.Hash("position", DestinationTransform.position, "time", 2.0f, "oncomplete", "OnMoveComplete"));
					}
					else
					{
						iTween.MoveTo(gameObject, iTween.Hash("position", _initialPosition, "time", 2.0f, "oncomplete", "OnMoveComplete"));
					}
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

	private void OnMoveComplete()
	{
		InputEx.Release();
	}
}
