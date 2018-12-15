using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsEngine;

public class DialogController : MonoBehaviour {

	public DialogCollection dialogCollection;
	protected string path;

	protected Dictionary<string, bool> flags;
	protected Dictionary<bool, string> flagR;
	protected Dictionary<string, int> flaggedNode;

	protected int overrideDialog;
	protected int defaultDialog;
    
	protected Actor actor;
	protected int zoneLastCheck;

	protected void Awake(){
		flags = new Dictionary<string, bool> ();
		flagR = new Dictionary<bool, string> ();
		flaggedNode = new Dictionary<string, int> ();

		overrideDialog = -1;
		defaultDialog = 0;

		actor = GetComponent<Actor> ();

        print(zoneLastCheck);
	}

	protected void OnMouseOver ()
	{

		if (GameState.Level != zoneLastCheck) {
			zoneLastCheck = GameState.Level;

			dialogCollection = DialogCollection.Load (Resources.Load (path + zoneLastCheck) as TextAsset);
		}

		foreach (string key in flags.Keys) {
			flagR [flags [key]] = key;
		}

		if (flags.ContainsValue (true)) {
			overrideDialog = flaggedNode [flagR [true]];
		}

		if (Vector3.Distance (gameObject.transform.position, GodManager.Player.transform.position) < 5) {
			GodManager.DialogManager.dialogList = dialogCollection;

			if (Input.GetKeyDown (KeyCode.E) && !GodManager.DialogManager.isActive) {
				if (flags.ContainsKey ("first time")) {
					if (flags ["first time"] == true) {
						flags ["first time"] = false;
						Debug.Log ("first time");
						GodManager.DialogManager.StartDialog (actor ,flaggedNode["first time"],0);
					}else{
						GodManager.DialogManager.StartDialog (actor, (overrideDialog > 0) ? overrideDialog : defaultDialog, 0);
					}
				} else {
					GodManager.DialogManager.StartDialog (actor, (overrideDialog > 0) ? overrideDialog : defaultDialog, 0);
				}
			}
		}

		overrideDialog = -1;
	}

	protected void StoreKeys()
    {
		foreach (string key in flags.Keys)
			flagR.Add (flags[key], key);
	}

}
