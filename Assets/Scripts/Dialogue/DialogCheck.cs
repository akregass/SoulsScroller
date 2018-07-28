using UnityEngine;
using System.Collections;

public class DialogCheck
{

	public int questID;
	public int actorId;
	public int node;
	public bool completed;

	public DialogCheck(int q, int id, int n){
		questID = q;
		actorId = id;
		node = n;
	}

}

