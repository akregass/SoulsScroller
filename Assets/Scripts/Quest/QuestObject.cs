using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestObject : MonoBehaviour
{

	public QuestManager questManager;
	public List<Quest> outgoingQuests;

	private Quest.Stage qS;

	void Start(){
		questManager = GameObject.FindGameObjectWithTag("God Eye").GetComponent<GodEye>().questManager;

		List<Quest.Stage> s = new List<Quest.Stage>();

		s.Add(new Quest.Stage());
		qS.stageProgress = Quest.StageProgress.NotStarted;

		qS.stageObjectives = new List<QuestObjective>();
		qS.stageObjectives.Add(new QuestObjective());
		qS.stageObjectives.Add(new QuestObjective());

		qS.stageObjectives[0].objectiveType = QuestObjective.Type.COLLECTION;
		qS.stageObjectives[0].desc = "Find evidence in the duplex";
		qS.stageObjectives[0].target = 0;
		qS.stageObjectives[0].targetNum = 1;

		qS.stageObjectives[1].objectiveType = QuestObjective.Type.COLLECTION;
		qS.stageObjectives[1].desc = "Find evidence in the duplex";
		qS.stageObjectives[1].target = 1;
		qS.stageObjectives[1].targetNum = 1;

		s[0] = qS;

		s.Add(new Quest.Stage());
		qS.stageProgress = Quest.StageProgress.NotStarted;
		
		qS.stageObjectives = new List<QuestObjective>();
		qS.stageObjectives.Add(new QuestObjective());
		
		qS.stageObjectives[0].objectiveType = QuestObjective.Type.DIALOG;
		qS.stageObjectives[0].desc = "Show your findings to Amalexia";
		qS.stageObjectives[0].target = 2;
		qS.stageObjectives[0].targetNum = -1;

		s[1] = qS;

		s.Add(new Quest.Stage());
		qS.stageProgress = Quest.StageProgress.NotStarted;
		
		qS.stageObjectives = new List<QuestObjective>();
		qS.stageObjectives.Add(new QuestObjective());
		
		qS.stageObjectives[0].objectiveType = QuestObjective.Type.DIALOG;
		qS.stageObjectives[0].desc = "Talk to Amalexia's contact in City B";
		qS.stageObjectives[0].target = 3;
		qS.stageObjectives[0].targetNum = -1;

		s[2] = qS;

		s.Add(new Quest.Stage());
		qS.stageProgress = Quest.StageProgress.NotStarted;
		
		qS.stageObjectives = new List<QuestObjective>();
		qS.stageObjectives.Add(new QuestObjective());
		
		qS.stageObjectives[0].objectiveType = QuestObjective.Type.TRAVEL;
		qS.stageObjectives[0].desc = "Find a way into the police station";
		qS.stageObjectives[0].target = 0;
		qS.stageObjectives[0].targetNum = -1;

		s[3] = qS;

		s.Add(new Quest.Stage());
		qS.stageProgress = Quest.StageProgress.NotStarted;
		
		qS.stageObjectives = new List<QuestObjective>();
		qS.stageObjectives.Add(new QuestObjective());
		qS.stageObjectives.Add(new QuestObjective());
		
		qS.stageObjectives[0].objectiveType = QuestObjective.Type.TRAVEL;
		qS.stageObjectives[0].desc = "Find Lt. Carmack's office";
		qS.stageObjectives[0].target = 2;
		qS.stageObjectives[0].targetNum = -1;

		qS.stageObjectives[1].objectiveType = QuestObjective.Type.SPEC;
		qS.stageObjectives[1].desc = "Plant the evidence in Lt. Carmack's desk";
		qS.stageObjectives[1].target = -1;
		qS.stageObjectives[1].targetNum = -1;

		s[4] = qS;

		s.Add(new Quest.Stage());
		qS.stageProgress = Quest.StageProgress.NotStarted;
		
		qS.stageObjectives = new List<QuestObjective>();
		qS.stageObjectives.Add(new QuestObjective());
		
		qS.stageObjectives[0].objectiveType = QuestObjective.Type.TRAVEL;
		qS.stageObjectives[0].desc = "Talk to Amalexia";
		qS.stageObjectives[0].target = 2;
		qS.stageObjectives[0].targetNum = -1;

		s[5] = qS;

		Quest q = new Quest("Amalexia's Fall", 0, Quest.QuestProgress.NotEligible, 1, 0, "Help clear Amalexia's name after she was accused of murder", 6, 7, "Help clear Amalexia's name after she was accused of murder", -1, s);

		questManager.questList.Add(q);
		outgoingQuests.Add (q);
	}

	void Update(){

	}
}