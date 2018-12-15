using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SoulsEngine;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;

	public static List<Quest> questList;
    public static List<Quest> activeQuestList;
    public static List<DialogCheck> dialogChecks;

    public static List<QuestMarker> questMarkers;

    public static bool isActive;

    public static Quest.Stage qS;

	private static Quest activeJournalQuest;
    public static GUISkin skin;

	public void Start()
    {
		dialogChecks = new List<DialogCheck> ();
		questMarkers = new List<QuestMarker> ();
	}

	public void Update()
    {
		UpdateDialogChecks ();
		ListenQuestMarkers ();

		if(Input.GetButtonDown ("Journal"))
        {
			activeJournalQuest = null;
			isActive = !isActive;
            //GodManager.Player.Inventory.ToggleInventory();
		}
	}

	public void OnGUI()
    {
		if(isActive)
        {

		}
	}

	public void LateUpdate()
    {

		UpdateQuests ();

	}

	public static void StartQuest(Quest quest)
    {
		quest.questProgress = Quest.QuestProgress.Accepted;
        qS = quest.questStages[0];
        qS.stageProgress = Quest.StageProgress.Started;
		quest.questStages[0] = qS;

        activeQuestList.Add(quest);
	}

	public void CompleteObjective(Quest quest, int objectiveId)
    {
		quest.questStages [quest.currStage].stageObjectives [objectiveId].completed = true;

		CheckStageProgress (quest);
	}

	private void CheckStageProgress(Quest quest)
    {
		Dictionary<QuestObjective,bool> d = new Dictionary<QuestObjective, bool> ();

		foreach(QuestObjective qO in quest.questStages[quest.currStage].stageObjectives)
        {
			d.Add (qO, qO.completed);
		}

		if(!d.ContainsValue (false))
        {
			NextStage (quest);
		}

	}

	public void NextStage(Quest quest)
    {
		if(quest.currStage+1 != quest.questStages.Count)
        {
			qS = quest.questStages[quest.currStage];
			qS.stageProgress = Quest.StageProgress.Completed;
			quest.questStages[quest.currStage] = qS;
			quest.currStage++;
			qS = quest.questStages[quest.currStage];
			qS.stageProgress = Quest.StageProgress.Started;
			quest.questStages[quest.currStage] = qS;
		}
        else
        {
			qS = quest.questStages[quest.currStage];
			qS.stageProgress = Quest.StageProgress.Completed;
			quest.questStages[quest.currStage] = qS;
			quest.questProgress = Quest.QuestProgress.Complete;

			if(quest.questNext != -1)
            {
				StartQuest (questList[quest.questNext]);
			}
		}
	}

	public bool CheckQuest(Quest quest)
    {
		if(activeQuestList.Contains(quest))
        {
			return true;
		}
		return false;
	}

	public bool CheckQuest(int questId)
    {
		foreach(Quest quest in activeQuestList)
        {
			if(quest.questId == questId)
            {
				return true;
			}
		}
		return false;
	}

	public void UpdateQuests ()
	{
		foreach (Quest q in activeQuestList)
        {
			for (int i = 0; i < q.questStages [q.currStage].stageObjectives.Count; i++)
            {

				QuestObjective qO = q.questStages [q.currStage].stageObjectives [i];

				switch (qO.objectiveType)
                {

				case QuestObjective.Type.COLLECTION:

					if (GodManager.Player.GetComponent<Inventory> ().CountOf ((int)qO.target) >= qO.targetNum)
						CompleteObjective (q, i);

					break;

				case QuestObjective.Type.DIALOG:

					if (GodManager.DialogManager.isActive)
                        {
						foreach (DialogCheck dC in dialogChecks)
                            {
							if (dC.questID == q.questId)
                                {
								if (dC.completed)
                                    {
									if (!qO.completed)
                                        {
										CompleteObjective (q, i);
									}
								}
							}
						}
					}

					break;

				case QuestObjective.Type.TRAVEL:

					foreach(QuestMarker qM in questMarkers)
                        {
						if(qM.questId == q.questId)
                            {
							if(qM.id == (int)qO.target)
                                {
								if(qM.triggered)
                                    {
									if(!qO.completed)
                                        {
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

	void UpdateDialogChecks()
    {
		dialogChecks.Clear ();
		foreach(Quest q in activeQuestList)
        {
			foreach(Quest.Stage qS in q.questStages)
            {
				foreach(QuestObjective qO in qS.stageObjectives)
                {
					if(qO.objectiveType == QuestObjective.Type.DIALOG)
                    {
						dialogChecks.Add (new DialogCheck(q.questId, (int)qO.target, qO.targetNum));
					}
				}
			}
		}
	}

	void ListenQuestMarkers()
    {
		questMarkers.Clear ();
        var markerList = GameObject.FindGameObjectsWithTag("Quest Marker");

		foreach(GameObject gO in markerList)
        {
			questMarkers.Add (gO.GetComponent<QuestMarker>());
		}
	}

	void DrawJournal()
    {

	}
}