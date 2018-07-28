using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {

	public DialogCollection dialogCollection;
	protected string path;

	protected Dictionary<string, bool> flags;
	protected Dictionary<bool, string> flagR;
	protected Dictionary<string, int> flaggedNode;

	protected int overrideDialog;
	protected int defaultDialog;

	protected GodEye godEye;
	protected Actor actor;
	protected string zoneLastCheck;

	protected void Awake(){
		flags = new Dictionary<string, bool> ();
		flagR = new Dictionary<bool, string> ();
		flaggedNode = new Dictionary<string, int> ();

		overrideDialog = -1;
		defaultDialog = 0;

		godEye = GameObject.FindGameObjectWithTag("God Eye").GetComponent<GodEye>();
		zoneLastCheck = null;

		actor = GetComponent<Actor> ();
	}

	protected void OnMouseOver ()
	{

		if (zoneLastCheck == null || godEye.worldState ["Zone"] != zoneLastCheck) {
			zoneLastCheck = godEye.worldState ["Zone"];

			dialogCollection = DialogCollection.Load (Resources.Load (path + zoneLastCheck) as TextAsset);
		}

		foreach (string key in flags.Keys) {
			flagR [flags [key]] = key;
		}

		if (flags.ContainsValue (true)) {
			overrideDialog = flaggedNode [flagR [true]];
		}

		if (Vector3.Distance (gameObject.transform.position, godEye.player.transform.position) < 5) {
			godEye.dialogManager.dialogList = dialogCollection;

			if (Input.GetKeyDown (KeyCode.E) && !godEye.dialogManager.isActive) {
				if (flags.ContainsKey ("first time")) {
					if (flags ["first time"] == true) {
						flags ["first time"] = false;
						Debug.Log ("first time");
						godEye.dialogManager.StartDialog (actor ,flaggedNode["first time"],0);
					}else{
						godEye.dialogManager.StartDialog (actor, (overrideDialog > 0) ? overrideDialog : defaultDialog, 0);
					}
				} else {
					godEye.dialogManager.StartDialog (actor, (overrideDialog > 0) ? overrideDialog : defaultDialog, 0);
				}
			}
		}

		overrideDialog = -1;
	}

	protected void StoreKeys(){
		foreach (string key in flags.Keys){
			flagR.Add (flags[key], key);
		}
	}

}
