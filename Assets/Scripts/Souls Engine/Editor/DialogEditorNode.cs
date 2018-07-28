using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class DialogEditorNode{

	public List<Action> content;
	public Dictionary<string, string> fields;
	public DialogEditorText linkedNode;
	public List<DialogEditorOption> options;
	public Node parentNode;

	public void DrawField(ref string v, string t){
		v = EditorGUILayout.TextField(t, v);
	}

	public void DrawArea(ref string v, float h){
		EditorStyles.textArea.wordWrap = true;
		EditorStyles.textField.wordWrap = true;
		v = EditorGUILayout.TextArea(v, GUILayout.ExpandHeight(true), GUILayout.MinHeight(h));
	}

}
