using UnityEngine;

public class StaticComponent : MonoBehaviour
{
	// This static method is useful to allow editor script getting access to the game code.
	public static StaticComponent Instance;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
