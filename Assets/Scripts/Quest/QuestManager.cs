using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
	public List<Quest> questList;
	public List<Quest> activeQuestList;
	public List<DialogCheck> dialogChecks;

	public List<QuestMarker> questMarkers;

	public bool isActive;

	private static GodEye godEye;

	public Quest.Stage qS;

	float journalX;
	float journalY;
	float journalWidth;
	float journalHeight;
	float journalQuestX;
	float journalQuestY;
	float journalQuestWidth;
	float journalQuestHeight;
	public float skinWidthX;
	public float skinWidthY;

	private Quest activeJournalQuest;
	public GUISkin skin;

	public void Start(){
		godEye = GetComponent<GodEye> ();
		dialogChecks = new List<DialogCheck> ();
		questMarkers = new List<QuestMarker> ();

		journalX = (Screen.width - skin.GetStyle ("background_journal").normal.background.width) / 2 + skinWidthX;
		journalY = 0;

		journalQuestX = journalX;
		journalQuestY = journalY + skinWidthY;

		journalWidth = skin.GetStyle ("background_journal").normal.background.width;
		journalHeight = skin.GetStyle ("background_journal").normal.background.height;

		journalQuestWidth = skin.GetStyle ("entry").normal.background.width;
		journalQuestHeight = skin.GetStyle ("entry").normal.background.height;
	}

	public void Update(){
		UpdateDialogChecks ();

		ListenQuestMarkers ();

		if(Input.GetButtonDown ("Journal")){
			activeJournalQuest = null;
			isActive = !isActive;
			if(godEye.player.inventory.showInventory){
				godEye.player.inventory.showInventory = false;
				Vector2.up;
			}
		}
	}

	public void OnGUI(){
		if(isActive){
			DrawJournal ();
		}
	}

	public void LateUpdate(){

		UpdateQuests ();

	}

	public static void StartQuest(Quest quest){
		quest.questProgress = Quest.QuestProgress.Accepted;
		godEye.questManager.qS = quest.questStages[0];
		godEye.questManager.qS.stageProgress = Quest.StageProgress.Started;
		quest.questStages[0] = godEye.questManager.qS;

		godEye.questManager.activeQuestList.Add(quest);
	}

	public void CompleteObjective(Quest quest, int objectiveId){

		quest.questStages [quest.currStage].stageObjectives [objectiveId].completed = true;

		CheckStageProgress (quest);
	}

	private void CheckStageProgress(Quest quest){

		Dictionary<QuestObjective,bool> d = new Dictionary<QuestObjective, bool> ();

		foreach(QuestObjective qO in quest.questStages[quest.currStage].stageObjectives){
			d.Add (qO, qO.completed);
		}

		if(!d.ContainsValue (false)){
			NextStage (quest);
		}

	}

	public void NextStage(Quest quest){
		if(quest.currStage+1 != quest.questStages.Count){
			qS = quest.questStages[quest.currStage];
			qS.stageProgress = Quest.StageProgress.Completed;
			quest.questStages[quest.currStage] = qS;
			quest.currStage++;
			qS = quest.questStages[quest.currStage];
			qS.stageProgress = Quest.StageProgress.Started;
			quest.questStages[quest.currStage] = qS;
		}else{
			qS = quest.questStages[quest.currStage];
			qS.stageProgress = Quest.StageProgress.Completed;
			quest.questStages[quest.currStage] = qS;
			quest.questProgress = Quest.QuestProgress.Complete;

			if(quest.questNext != -1){
				StartQuest (questList[quest.questNext]);
			}

		}
	}

	public bool CheckQuest(Quest quest){
		if(activeQuestList.Contains(quest)){
			return true;
		}
		return false;
	}

	public bool CheckQuest(int questId){
		foreach(Quest quest in activeQuestList){
			if(quest.questId == questId){
				return true;
			}
		}
		return false;
	}

	public void UpdateQuests ()
	{
		foreach (Quest q in activeQuestList) {
			for (int i = 0; i < q.questStages [q.currStage].stageObjectives.Count; i++) {

				QuestObjective qO = q.questStages [q.currStage].stageObjectives [i];

				switch (qO.objectiveType) {

				case QuestObjective.Type.COLLECTION:

					if (godEye.player.GetComponent<Inventory> ().CountOf ((int)qO.target) >= qO.targetNum)
						CompleteObjective (q, i);

					break;

				case QuestObjective.Type.DIALOG:

					if (godEye.dialogManager.isActive) {
						foreach (DialogCheck dC in dialogChecks) {
							if (dC.questID == q.questId) {
								if (dC.completed) {
									if (!qO.completed) {
										CompleteObjective (q, i);
									}
								}
							}
						}
					}

					break;

				case QuestObjective.Type.TRAVEL:

					foreach(QuestMarker qM in questMarkers){
						if(qM.questId == q.questId){
							if(qM.id == (int)qO.target){
								if(qM.triggered){
									if(!qO.completed){
										CompleteObjective (q, i);
									}
								}
							}
						}
					}

					break;

				}
			}
		}
	}

	void UpdateDialogChecks(){
		dialogChecks.Clear ();
		foreach(Quest q in godEye.questManager.activeQuestList){
			foreach(Quest.Stage qS in q.questStages){
				foreach(QuestObjective qO in qS.stageObjectives){
					if(qO.objectiveType == QuestObjective.Type.DIALOG){
						dialogChecks.Add (new DialogCheck(q.questId, (int)qO.target, qO.targetNum));
					}
				}
			}
		}
	}

	void ListenQuestMarkers(){
		questMarkers.Clear ();
		foreach(GameObject gO in GameObject.FindGameObjectsWithTag ("Quest Marker")){
			questMarkers.Add (gO.GetComponent<QuestMarker>());
		}
	}

	void DrawJournal(){

		GUI.DrawTexture (new Rect(0,0,Screen.width, Screen.height), skin.GetStyle ("background_main").normal.background);

		GUI.DrawTexture (new Rect(journalX, journalY, skin.GetStyle ("panel_left").normal.background.width, Screen.height), skin.GetStyle ("panel_left").normal.background);
		GUI.DrawTexture (new Rect(journalX - 8f + skin.GetStyle ("panel_left").normal.background.width, journalY, skin.GetStyle ("panel_right").normal.background.width, Screen.height), skin.GetStyle ("panel_right").normal.background);

		int i = 0;
		foreach(Quest q in activeQuestList){
			if(GUI.Button (new Rect (journalQuestX, (i * journalQuestHeight) + journalQuestY, journalQuestWidth, journalQuestHeight), q.questTitle, skin.GetStyle ("entry"))){
				activeJournalQuest = q;
			}
			i++;
		}

		if(activeJournalQuest != null){
			DrawActiveQuest ();
		}
	}

	void DrawActiveQuest(){

		Rect area = new Rect (journalX + skin.GetStyle ("panel_left").normal.background.width, journalY, skin.GetStyle ("header").normal.background.width, skin.GetStyle ("header").normal.background.height);

		GUILayout.BeginArea (area, skin.GetStyle ("header").normal.background);

		GUILayout.Space (96f);
		GUILayout.Label (activeJournalQuest.questTitle, skin.GetStyle ("headerTitle"));
		GUILayout.Space (32f);
		GUILayout.Label (activeJournalQuest.questDescription, skin.GetStyle ("headerDesc"));

		GUILayout.EndArea ();

		area.y = area.y + area.height;
		area.height = Screen.height - area.height;


		GUILayout.BeginArea (area);

		GUILayout.Space (128f);

		for(int i = 0; i < activeJournalQuest.currStage + 1; i++){
			GUILayout.BeginVertical ();

			foreach(QuestObjective qO in activeJournalQuest.questStages[i].stageObjectives){

				GUILayout.BeginHorizontal ();

				if(qO.completed){
					GUILayout.Box (skin.GetStyle ("objectiveComplete").normal.background);
				}else{
					GUILayout.Box (skin.GetStyle ("objectiveIncomplete").normal.background);
				}

				GUILayout.Label (qO.desc, skin.GetStyle ("headerDesc"));

				GUILayout.EndHorizontal ();
			}

			GUILayout.EndVertical ();
		}

		GUILayout.EndArea ();

	}
}