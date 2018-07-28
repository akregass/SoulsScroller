using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest {
	public string questTitle;
	public int questId;             
	public QuestProgress questProgress;
	public int questStartDialog;
	public int questStartNode;
	public string questDescription;
	public int questHint;                                
	public int questCongratulations;                       
	public string questSummary;     
	public List<Stage> questStages;
	public int questNext;
	public int currStage;
	public int currObjective;

	public List<DialogCheck> dialogChecks;

	public enum QuestProgress {NotEligible, Eligible, Accepted, Complete, Done, Failed}
	public enum StageProgress {NotStarted, Started, Completed}

	public struct Stage{
		public StageProgress stageProgress;
		public List<QuestObjective> stageObjectives;
	}

	public Quest(){}

	private Stage qS;

	public Quest(string title, int id, QuestProgress progress, int startDialog, int startNode, string desc, int hint, int congrats, string summary, int nextQuest, List<Stage> stages){
		questTitle = title;
		questId = id;
		questProgress = progress;
		questStartDialog = startDialog;
		questStartNode = startNode;
		questDescription = desc;
		questHint = hint;
		questCongratulations = congrats;
		questSummary = summary;
		questNext = nextQuest;
		questStages = stages;
	}

	public string GetQuestDesc(){
		return questDescription;
	}
}