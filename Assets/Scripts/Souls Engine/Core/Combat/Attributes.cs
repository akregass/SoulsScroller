using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attributes : MonoBehaviour {

	public Dictionary<string, float> attributes = new Dictionary<string, float>();
	public Dictionary<string, float> baseAttributes = new Dictionary<string, float>();

	void Start () {
		attributes.Add("Health", 0f); 	 //actor hp
		attributes.Add("Energy", 0f);	 //cyberabilities require energy
		attributes.Add("Speed", 0f);	 //actor speed
		attributes.Add("Cyberlink", 0f); //potency of cyberabilities

		baseAttributes = attributes;
	}
}
