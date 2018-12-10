using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class DialogOption{

	[XmlAttribute("goto")]
	public string destination;

	[XmlAttribute("quest")]
	public string questRef;

	[XmlAttribute("incItems")]
	public string incItems;

	[XmlAttribute("outItems")]
	public string outItems;

	[XmlElement("DialogOptionText")]
	public string text;

	[XmlIgnore]
	public Dictionary<int, int> incomingItems;

	[XmlIgnore]
	public Dictionary<int, int> outgoingItems;


	public void ParseElements ()
	{

		incomingItems = new Dictionary<int, int> ();
		outgoingItems = new Dictionary<int, int> ();

		int i = -1;
		int j = -1;

		if (incItems != null) {
			foreach (String ss in incItems.Split(';')) {
				foreach (String s in ss.Split(',')) {
					if (i != -1) {
						if (j == -1) {
							j = Int32.Parse (s);
							incomingItems.Add (i, j);
							i = -1;
							j = -1;
						}
					} else {
						i = Int32.Parse (s);
					}
				}
			}
		}

		if (outItems != null) {
			foreach (String ss in outItems.Split(';')) {
				foreach (String s in ss.Split(',')) {
					if (i != -1) {
						if (j == -1) {
							j = Int32.Parse (s);
							outgoingItems.Add (i, j);
							i = -1;
							j = -1;
						}
					} else {
						i = Int32.Parse (s);
					}
				}
			}
		}
	}
}
