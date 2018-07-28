using UnityEngine;
using System.Collections;

public class QuestObjective
{

	public enum Type {COLLECTION, TRAVEL, DIALOG, SPEC}

	public Type objectiveType;
	public object target;
	public int targetNum;
	public string desc;
	public bool completed = false;

}

