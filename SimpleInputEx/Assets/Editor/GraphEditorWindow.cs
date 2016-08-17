using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Graphs;

public class GraphEditorWindow : EditorWindow {

	[MenuItem("Test/Graph Editor")]
	public static void Init()
	{
		_window = GetWindow(typeof(GraphEditorWindow)) as GraphEditorWindow;
		_window.Show();
	}

	public string GameObjectName = "GameObject";
	private Graph _stateMachineGraph;
	private GraphGuiEx _stateMachineGraphGui;
	private static GraphEditorWindow _window;

	public void OnEnable()
	{
		_stateMachineGraph = ScriptableObject.CreateInstance<Graph>();
		_stateMachineGraph.hideFlags = HideFlags.HideAndDontSave;

		// Create new node
		Node node = ScriptableObject.CreateInstance<Node>();
		node.title = "mile2";
		node.position = new Rect(400, 34, 300, 200);
		node.AddInputSlot("input");
		var start = node.AddOutputSlot("output");
		node.AddProperty(new Property(typeof(System.Int32), "integer"));
		_stateMachineGraph.AddNode(node);

		// Create new node
		node = ScriptableObject.CreateInstance<Node>();
		node.title = "mile1";
		node.position = new Rect(0, 0, 300, 200);

		Slot end = node.AddInputSlot("input");
		node.AddOutputSlot("output");
		node.AddProperty(new Property(typeof(System.Int32), "integer"));
		_stateMachineGraph.AddNode(node);

		// Create edge
		_stateMachineGraph.Connect(start, end);

		_stateMachineGraphGui = ScriptableObject.CreateInstance<GraphGuiEx>();
		_stateMachineGraphGui.graph = _stateMachineGraph;
	}

	public void OnGUI()
	{
		/*
		GUILayout.Label(GameObjectName);
		*/

		if (_window != null && _stateMachineGraphGui != null)
		{
			_stateMachineGraphGui.BeginGraphGUI(_window, new Rect(0, 0, _window.position.width, _window.position.height));
			_stateMachineGraphGui.OnGraphGUI();
			_stateMachineGraphGui.EndGraphGUI();
		}
	}

	// This is called every frame
	public void Update()
	{
		if (Selection.activeGameObject != null && Selection.activeGameObject.name != GameObjectName)
		{
			GameObjectName = Selection.activeGameObject.name;
			Repaint();
		}		
	}

	// This is called around 10 times per second
	public void OnInspectorUpdate()
	{
		
	}
}
