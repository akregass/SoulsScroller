using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class NodeEditor : EditorWindow
{	
	private string currFile;
	private DialogCollection dialogCollection;
	private int currNodeGroup;
	private List<NodeGroup> nodeGroups;

	private bool showFile;
	private bool showDialog;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
	private Vector2 drag;

	private bool changesMade;

    [MenuItem("Souls Engine/Dialog Editor")]
    private static void OpenWindow()
    {
        NodeEditor window = GetWindow<NodeEditor>();
        window.titleContent = new GUIContent("Dialog Editor");
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
		outPointStyle.border = new RectOffset(4, 4, 12, 12);

		nodeGroups = new List<NodeGroup> ();

		dialogCollection = new DialogCollection ();
		dialogCollection.dialogList = new List<Dialog>();

		nodeGroups.Add (new NodeGroup (new List<Node> (), new Queue<DialogEditorText> (), new List<DialogEditorOption>(), new List<Connection> (), new List<Vector2>()));
		currNodeGroup = 0;

		currFile = "";
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

		showFile = EditorGUILayout.Foldout(showFile, "File");

        if(showFile){

			EditorGUILayout.TextField("Dialog File", currFile);

			if(GUILayout.Button ("New")){
				if(changesMade){
					int i = EditorUtility.DisplayDialogComplex ("Changes made", "Save changes?", "Yes", "No", "Cancel");
					if(i == 0){
						SaveFile ();
						Clear ();
					}else if(i == 1){
						Clear ();
					}
				}

				Clear ();
			}

	        if(GUILayout.Button("Open")){
				Clear ();
				currFile = EditorUtility.OpenFilePanel("Open Dialog File",@"C:\Users\Insol\Desktop\Dsyti\Dysti\Assets\Resources\Dialog","xml");
				LoadCollection (currFile);
	        }

	        if(GUILayout.Button("Save")){
				SaveFile ();
	        }

        }

		showDialog = EditorGUILayout.Foldout(showDialog, "Dialog");

		if(showDialog){
			EditorGUILayout.LabelField (currNodeGroup.ToString ());

	        if(GUILayout.Button ("New")){
				nodeGroups.Add (new NodeGroup (new List<Node> (), new Queue<DialogEditorText> (), new List<DialogEditorOption>(), new List<Connection> (), new List<Vector2>()));
				currNodeGroup++;
	        }

			if(GUILayout.Button("Previous")){
				if(currNodeGroup > 0){
					currNodeGroup--;
				}
	        }

	        if(GUILayout.Button("Next")){
	        	if(currNodeGroup < nodeGroups.Count - 1){
					currNodeGroup++;
	        	}
	        }
        }

        BeginWindows();

       	DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        EndWindows();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

		if (GUI.changed) {
			changesMade = true;
			Repaint ();
		}

    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
		if (nodeGroups[currNodeGroup].nodes != null)
        {
			for (int i = 0; i < nodeGroups[currNodeGroup].nodes.Count; i++)
            {
				nodeGroups[currNodeGroup].nodes[i].Draw(i);
            }
        }
    }

    private void DrawConnections()
    {
		if (nodeGroups[currNodeGroup].connections != null)
        {
			for (int i = 0; i < nodeGroups[currNodeGroup].connections.Count; i++)
            {
				nodeGroups[currNodeGroup]. connections[i].Draw();
            } 
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
            break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
            break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
		if (nodeGroups[currNodeGroup].nodes != null)
        {
			for (int i = nodeGroups[currNodeGroup].nodes.Count - 1; i >= 0; i--)
            {
				bool guiChanged = nodeGroups[currNodeGroup].nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }

    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition, Node.NodeType.TEXT)); 
        genericMenu.AddItem(new GUIContent("Add option"), false, () => OnClickAddNode(mousePosition, Node.NodeType.OPTION));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

		if (nodeGroups[currNodeGroup].nodes != null)
        {
			for (int i = 0; i < nodeGroups[currNodeGroup].nodes.Count; i++)
            {
				nodeGroups[currNodeGroup].nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddNode(Vector2 mousePosition, Node.NodeType type)
    {
		if (nodeGroups[currNodeGroup].nodes == null)
        {
			nodeGroups[currNodeGroup].nodes = new List<Node>();
        }

		Node n = new Node (mousePosition, 200, 100, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, type, nodeGroups[currNodeGroup].nodesT.Count);

		if(type == Node.NodeType.TEXT){
			nodeGroups[currNodeGroup].nodesT.Enqueue ((DialogEditorText)n.dialogNode);
        }else{
			nodeGroups[currNodeGroup].nodesO.Add ((DialogEditorOption)n.dialogNode);
        }

		nodeGroups[currNodeGroup].nodes.Add (n);

    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection(); 
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node)
    {
		if (nodeGroups[currNodeGroup].connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

			for (int i = 0; i < nodeGroups[currNodeGroup].connections.Count; i++)
            {
				if (nodeGroups[currNodeGroup].connections[i].inPoint == node.inPoint || nodeGroups[currNodeGroup].connections[i].outPoint == node.outPoint)
                {
					connectionsToRemove.Add(nodeGroups[currNodeGroup].connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
            	OnClickRemoveConnection (connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

		if(node.type == Node.NodeType.TEXT){
			nodeGroups[currNodeGroup].nodesT.Dequeue ();

			for(int i = 0; i < nodeGroups[currNodeGroup].nodesT.Count; i++){
				nodeGroups[currNodeGroup].nodesT.ElementAt (i).id = i.ToString ();
	        }
        }

		nodeGroups[currNodeGroup].nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection)
    {
		ClearLink (connection);
		nodeGroups[currNodeGroup].connections.Remove(connection);
    }

    private void CreateConnection ()
	{
		if (nodeGroups[currNodeGroup].connections == null) {
			nodeGroups[currNodeGroup].connections = new List<Connection> ();
		}
		if (selectedInPoint.node.type == selectedOutPoint.node.type)
			ClearConnectionSelection ();
		else {
			nodeGroups[currNodeGroup].connections.Add (new Connection (selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

			if (selectedOutPoint.node.type == Node.NodeType.OPTION) {
				selectedOutPoint.node.dialogNode.linkedNode = (DialogEditorText)selectedInPoint.node.dialogNode;
			} else {
				selectedOutPoint.node.dialogNode.options.Add ((DialogEditorOption)selectedInPoint.node.dialogNode);
			}
			}
			ClearConnectionSelection ();

    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

	private void ClearLink(Connection connection){
    	if(connection.outPoint.node.type == Node.NodeType.OPTION){
			connection.outPoint.node.dialogNode.linkedNode = null;
		}else{
			connection.outPoint.node.dialogNode.options.Remove ((DialogEditorOption)connection.inPoint.node.dialogNode);
    	}
    }

    private void Clear (){
		nodeGroups = new List<NodeGroup> ();
		nodeGroups.Add (new NodeGroup (new List<Node> (), new Queue<DialogEditorText> (), new List<DialogEditorOption>(), new List<Connection> (), new List<Vector2>()));

		dialogCollection = new DialogCollection ();
		dialogCollection.dialogList = new List<Dialog>();

		currNodeGroup = 0;
		currFile = "";
    }

    private void LoadCollection (string path)
	{

		bool duplicateFound = false;
		DialogEditorOption duplicate = new DialogEditorOption ();

		string metaPath = path.Replace (".xml", ".xmlmeta");
		NodeEditorData data = new NodeEditorData ();

		var serializer = new XmlSerializer (typeof(NodeEditorData));

		using (var reader = new StringReader (File.ReadAllText (metaPath))) {
			data = serializer.Deserialize (reader) as NodeEditorData;
		}

		nodeGroups = new List<NodeGroup> ();
		currNodeGroup = 0;

		dialogCollection = DialogCollection.Load (path);

		foreach (Dialog d in dialogCollection.dialogList) {

			//Debug.Log ("Reading dialog: " + nodeGroups.Count);

			nodeGroups.Add (new NodeGroup (new List<Node> (), new Queue<DialogEditorText> (), new List<DialogEditorOption> (), new List<Connection> (), new List<Vector2> ()));

			NodeGroup nG = nodeGroups [nodeGroups.Count - 1];

			nG.nodePositions = data.posList [nodeGroups.Count - 1];
			//Debug.Log (nG.nodePositions.Count);

			foreach (DialogNode dN in d.nodes) {

				//Debug.Log ("Reading node: " + nG.nodesT.Count + " ; Value: " + dN.text);

				Node nT = new Node (nG.nodePositions [nG.nodes.Count], 200, 100, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, Node.NodeType.TEXT, nodeGroups [currNodeGroup].nodesT.Count);

				DialogEditorText t = (DialogEditorText)nT.dialogNode;
				t.id = dN.id.ToString ();
				t.text = dN.text;
				nT.dialogNode = t;

				nG.nodes.Add (nT);
				nG.nodesT.Enqueue ((DialogEditorText)nT.dialogNode);

				foreach (DialogOption o in dN.options) {

					//Debug.Log ("Reading option: " + nG.nodesO.Count + " ; Value: " + o.text);

					foreach (DialogEditorOption cO in nG.nodesO) {
						if (cO.destination == o.destination) {
							if (cO.text == o.text) {
								//Debug.Log ("Duplicate found at node " + (nG.nodesT.Count - 1).ToString ());
								duplicateFound = true;
								duplicate = cO;
							}
						}
					}

					if (!duplicateFound) {

						Node nO = new Node (nG.nodePositions [nG.nodes.Count], 200, 100, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, Node.NodeType.OPTION, nodeGroups [currNodeGroup].nodesT.Count);

						DialogEditorOption v = (DialogEditorOption)nO.dialogNode;
						v.destination = o.destination;
						v.text = o.text;
						v.incItems = o.incItems;
						v.outItems = o.outItems;
						nO.dialogNode = v;

						nG.nodes.Add (nO);
						nT.dialogNode.options.Add ((DialogEditorOption)nO.dialogNode);
						nG.nodesO.Add ((DialogEditorOption)nO.dialogNode);

						nG.connections.Add (new Connection (nT.outPoint, nO.inPoint, OnClickRemoveConnection));
					}else{
						nG.connections.Add (new Connection (nT.outPoint, duplicate.parentNode.inPoint, OnClickRemoveConnection));
						nT.dialogNode.options.Add (duplicate);
						duplicateFound = false;
						duplicate = new DialogEditorOption ();
					}

					nodeGroups [nodeGroups.Count - 1] = nG;
				}
			}

			foreach(DialogEditorOption nO in nG.nodesO){
				if(nO.destination != null && nO.destination != "-1"){
					//Debug.Log ("Connecting option ");
					nG.connections.Add (new Connection (nO.parentNode.outPoint, nG.nodesT.ElementAt (Int32.Parse (nO.destination)).parentNode.inPoint, OnClickRemoveConnection));

					nO.linkedNode = nG.nodesT.ElementAt (Int32.Parse (nO.destination));
				}
			}

			Debug.Log ("Node count: " + nG.nodesT.Count + " ; Option count: " + nG.nodesO.Count);

		}
    }

    private void SaveCollection (string path)
	{

		bool duplicateFound = false;

		dialogCollection.dialogList = new List<Dialog> ();

		for (int i = 0; i < nodeGroups.Count; i++) {

			//Debug.Log ("Saving node group: " + i.ToString ());

			NodeGroup nG = nodeGroups [i];
			nG.nodePositions = new List<Vector2> ();

			dialogCollection.dialogList.Add (new Dialog ());

			Dialog d = dialogCollection.dialogList [dialogCollection.dialogList.Count - 1];

			d.actor = "";
			d.id = dialogCollection.dialogList.Count - 1;
			d.nodes = new List<DialogNode> ();

			foreach (DialogEditorText nT in nG.nodesT) {

				//Debug.Log ("Saving node: " + d.nodes.Count);

				d.nodes.Add (new DialogNode ());
				d.nodes [d.nodes.Count - 1].id = Int32.Parse (nT.id);
				d.nodes [d.nodes.Count - 1].text = nT.text;
				d.nodes [d.nodes.Count - 1].options = new List<DialogOption> ();

				nG.nodePositions.Add (nT.parentNode.rect.position);

				foreach (DialogEditorOption nO in nT.options) {

					//Debug.Log ("Saving option: " + d.nodes[d.nodes.Count - 1].options.Count);

					DialogOption dO = new DialogOption ();

					dO.destination = nO.destination;
					dO.text = nO.text;
					dO.incItems = nO.fields["incItems"];
					dO.outItems = nO.fields["outItems"];

					foreach (DialogNode cN in d.nodes) {
						foreach (DialogOption cO in cN.options) {
							if (cO.destination == dO.destination) {
								if (cO.text == dO.text) {
									duplicateFound = true;
								}
							}
						}
					}

					d.nodes [d.nodes.Count - 1].options.Add (dO);

					if (duplicateFound) {
						duplicateFound = false;
					} else {
						nG.nodePositions.Add (nO.parentNode.rect.position);
					}
				}
    		}

			nodeGroups [i] = nG;
    	}

		string metaPath = path.Replace (".xml", ".xmlmeta");
		NodeEditorData data = new NodeEditorData ();
		data.posList = new List<List<Vector2>> ();

		var serializer = new XmlSerializer(typeof(NodeEditorData));
		using(var stream = new FileStream(metaPath, FileMode.Create)){

			foreach(NodeGroup nG in nodeGroups){
				data.posList.Add (nG.nodePositions);
			}

			serializer.Serialize (stream, data);

		}

    	dialogCollection.Save (path);
	}

	private void SaveFile(){
		if(currFile != ""){
    		if(EditorUtility.DisplayDialog ("Overwrite file", "Are you sure you want to overwrite this dialog file?", "Yes", "Cancel")){
				if(currFile != "" && currFile != null){
					SaveCollection (currFile);
				}
    		}else{
				currFile = EditorUtility.SaveFilePanel ("Save Dialog File", @"C:\Users\Insol\Desktop\Dsyti\Dysti\Assets\Resources\Dialog", "Untitled", "xml");
				if(currFile != "" && currFile != null){
					SaveCollection (currFile);
				}
    		}
    	}else{
			currFile = EditorUtility.SaveFilePanel ("Save Dialog File", @"C:\Users\Insol\Desktop\Dsyti\Dysti\Assets\Resources\Dialog", "Untitled", "xml");
			if(currFile != "" && currFile != null){
					SaveCollection (currFile);
				}
    	}
	}
}