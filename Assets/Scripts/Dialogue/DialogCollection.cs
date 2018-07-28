using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
[XmlRoot("DialogCollection")]
public class DialogCollection {

	[XmlArray("Dialogs")]
	[XmlArrayItem("Dialog")]
	public List<Dialog> dialogList = new List<Dialog>();

	/*
	public static DialogCollection Load(string path)
	{
		var serializer = new XmlSerializer(typeof(DialogCollection));
		using(var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as DialogCollection;
		}
	}
	*/

	public static DialogCollection Load(TextAsset fileC)
	{
		var serializer = new XmlSerializer(typeof(DialogCollection));
		using (var reader = new StringReader (fileC.text))
		{
			return serializer.Deserialize(reader) as DialogCollection;
		}
	}

	public static DialogCollection Load(string path){
		var serializer = new XmlSerializer(typeof(DialogCollection));

		using (var reader = new StringReader (File.ReadAllText (path)))
		{
			return serializer.Deserialize(reader) as DialogCollection;
		}
	}

	public void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(DialogCollection));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
		
}