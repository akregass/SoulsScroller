using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class DialogNode{

	[XmlAttribute("Id")]
	public int id;
	
	[XmlElement("DialogText")]
	public string text;
	
	[XmlArray("DialogOptions")]
	[XmlArrayItem("DialogOption")]
	public List<DialogOption> options = new List<DialogOption>();

}
