using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogEditorOption : DialogEditorNode{

	public string destination;
	public string text;
	public string incItems;
	public string outItems;

	public bool showActions;
	public bool lastFrame;

	public DialogEditorOption(){

		fields = new Dictionary<string, string> ();

		fields.Add ("destination", destination);
		fields.Add ("text", text);
		fields.Add ("incItems", incItems);
		fields.Add ("outItems", outItems);

		content = new List<Action>();

		content.Add(() => UpdateFields());
		content.Add(() => UpdateRect());
		content.Add(() => EditorGUILayout.TextField(new GUIContent ("Goto"), destination));
		content.Add(() => DrawInfo());
		content.Add(() => EditorGUILayout.PrefixLabel("Text"));
		content.Add(() => DrawArea(ref text, 20f));

		fields["destination"] = "-1";

		showActions = false;
	}

	public void UpdateRect(){
		if(showActions){
			parentNode.rect.height = 174;
		}else{
			parentNode.rect.height = 120;
		}
	}

	public void UpdateFields(){
		if(linkedNode != null){
			destination = linkedNode.fields ["id"];
			fields ["destination"] = destination;
		}else{
			destination = "-1";
		}

		if(incItems == "-1"){
			incItems = null;
		}
		if(outItems == "-1"){
			outItems = null;
		}

		fields ["incItmes"] = incItems;
		fields ["outItems"] = outItems;
		fields ["text"] = text;
	}

	public void DrawInfo (){
		showActions = EditorGUILayout.Foldout (showActions, "Actions");

		if (showActions != lastFrame) {
			GUI.changed = true;
		}

		lastFrame = showActions;

		if(showActions){
			DrawField (ref incItems, "Incoming Items");
			DrawField (ref outItems, "Outgoing Items");
		}
	}

}