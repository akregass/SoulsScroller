using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogEditorText : DialogEditorNode{

	public string id;
	public string text;

	public DialogEditorText(int i){

		id = i.ToString ();

		fields = new Dictionary<string, string> ();
		options = new List<DialogEditorOption> ();

		fields.Add ("id", id);
		fields.Add ("text", text);

		content = new List<Action>();

		content.Add(() => UpdateFields());
		content.Add(() => EditorGUILayout.TextField("ID", id));
		content.Add(() => EditorGUILayout.PrefixLabel("Text"));
		content.Add(() => DrawArea(ref text, 20f));
	}

	public void UpdateFields(){
		fields["id"] = id;
		fields ["text"] = text;
	}

}