using UnityEngine;
using System.Collections;

public class QuestMarker : MonoBehaviour
{

	public int questId;
	public int id;
	public bool triggered;

	void OnCollisionEnter2D(Collision2D c){
		if(c.gameObject.tag == "Player"){
			triggered = true;
		}
	}

}

