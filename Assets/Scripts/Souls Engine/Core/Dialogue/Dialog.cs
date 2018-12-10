using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class Dialog {

	[XmlAttribute("actor")]
	public string actor;

	[XmlAttribute("id")]
	public int id;
	
	[XmlArray("DialogNodes")]
	[XmlArrayItem("DialogNode")]
	public List<DialogNode> nodes = new List<DialogNode>();

}