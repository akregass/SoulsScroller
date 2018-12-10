using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SoulsEngine;

public class DialogManager : MonoBehaviour {

	public GUISkin skin;
	public DialogCollection dialogList;
	public bool isActive;
	public bool showOptions;
	public Texture2D tex;
	public int optH;

	public TextAsset dialogFile;

	private int boxW;
	private int boxH;

	public float t;

	public int currentDialog;
	public int currentNode;
	public int currentTextId;
	public Actor currentActor;
	public string[] currText;
	public List<DialogOption> currOptions;
	
	public bool scrollingText;
	public string[] buttonTextures;

	private GodManager godManager;

	void Start () {
		godManager = GetComponent<GodManager> ();

		boxW = Screen.width;
		boxH = 60;
		optH = skin.GetStyle ("Middle Button").normal.background.height;

		buttonTextures = new string[] {"Top Button", "Middle Button", "Bottom Button"};
	}

	void OnGUI (){
		if (isActive) {
			foreach (DialogCheck dC in godManager.QuestManager.dialogChecks) {
				if (!dC.completed) {
					if (dC.actorId == currentActor.Id) {
						if (dC.node == currentNode) {
							dC.completed = true;
						}
					}
				}
			}

			if(t > 6f){
				t = 0f;
				SetText();
			}else{
				t += Time.deltaTime;
			}

			if(Input.GetKeyDown(KeyCode.Escape)){
				t = 3.6f;
			}

			//GUI.Label(new Rect(0, 0, boxW, boxH), currText[currTextId], skin.GetStyle("Thumb Panel"));
			GUI.TextArea (new Rect(0, 0, boxW, boxH), currText[currentTextId]);

			if(showOptions){
				int optY = Screen.height - 3 * optH;

				GUI.DrawTexture (new Rect (0, Screen.height - skin.GetStyle ("Thumb Panel").normal.background.height, skin.GetStyle ("Thumb Panel").normal.background.width, skin.GetStyle ("Thumb Panel").normal.background.height), skin.GetStyle ("Thumb Panel").normal.background);

				int counter = 0;
				for(int i=0; i<currOptions.Count; i++){

					if(GUI.Button(new Rect(0, optY + i*optH, skin.GetStyle (buttonTextures[counter]).normal.background.width, skin.GetStyle(buttonTextures[counter]).normal.background.height), currOptions[i].text, skin.GetStyle (buttonTextures[counter]))){
						CheckActions (currOptions[i]);
						SetNode (i);
					}

					counter++;
				}
				counter = 0;
			}

		}
	}

	void SetDialog (int id, int node){
		currentDialog = id;
		currentNode = node;
		currentTextId = 0;
		t = 0f;
		currText = dialogList.dialogList[id].nodes[node].text.Split('#');
		currOptions = dialogList.dialogList[id].nodes[currentNode].options;
	}

	void SetNode (int id){
		if (int.Parse(currOptions[id].destination) != -1){
			currentNode = int.Parse(currOptions[id].destination);
			currText = dialogList.dialogList[currentDialog].nodes[currentNode].text.Split ('#');
			currentTextId = 0;
			t = 0f;
			currOptions = dialogList.dialogList[currentDialog].nodes[currentNode].options;
			showOptions = false;
		}else{
			isActive = false;
			showOptions = false;
			godManager.Player.hasControl = true;
		}
	}

	void SetText (){
		if (currentTextId+1 == currText.Length){
			showOptions = true;
		}else{
			currentTextId++;
		}
	}

	public void StartDialog (Actor actor, int id, int node){
		godManager.Player.hasControl = false;
		currentActor = actor;
		SetDialog (id, node);
		isActive = true;
	}

	public void CheckActions(DialogOption option){
		option.ParseElements ();

		if(option.questRef != null && option.questRef != "-1"){
			Debug.Log (option.questRef);
			QuestManager.StartQuest(godManager.QuestManager.questList[Int32.Parse (option.questRef)]);
		}

		if(option.incomingItems.Count > 0){
			foreach(KeyValuePair<int, int> item in option.incomingItems){
				godManager.Player.Inventory.AddItem (item.Key, item.Value);
			}
		}

		if(option.outgoingItems.Count > 0){
			foreach(KeyValuePair<int, int> item in option.outgoingItems){
				godManager.Player.Inventory.RemoveItem (item.Key, item.Value);
			}
		}
	}

}