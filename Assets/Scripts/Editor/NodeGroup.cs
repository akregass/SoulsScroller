using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeGroup{

	public List<Node> nodes;
	public Queue<DialogEditorText> nodesT;
	public List<DialogEditorOption> nodesO;

	public List<Connection> connections;

	[SerializeField]
	public List<Vector2> nodePositions;

	public NodeGroup(List<Node> n, Queue<DialogEditorText> nT, List<DialogEditorOption> nO, List<Connection> c, List<Vector2> nP){
		nodes = n;
		nodesT = nT;
		nodesO = nO;
		connections = c;
		nodePositions = nP;
	}

}