using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using FullSerializer;
using System;

public class Serializer{
	private static fsSerializer _serializer = new fsSerializer();
	private static string fileExtenstion =	".snake";

	public static string Serialize<T>(T value){
		Type type = typeof(T);
		fsData data;
		var success = _serializer.TrySerialize(type, value, out data);
		return fsJsonPrinter.PrettyJson(data);
	}

	public static T Deserialize<T>(string serializedState){
		Type type = typeof(T);
		fsData data;
		var success = fsJsonParser.Parse(serializedState, out data);
		object deserialized = null;
		success = _serializer.TryDeserialize(data, type, ref deserialized);
		return (T)(object)deserialized;
	}

	public static T Load<T>(string fileName){
		string finalPath = Application.persistentDataPath+"/"+fileName+fileExtenstion;
		try
		{
			if(File.Exists(finalPath)){
				Type type = typeof(T);
				BinaryFormatter bf = new BinaryFormatter();
				string file = File.ReadAllText(finalPath);
				var temp = (T)(object)Deserialize(type, file);
				return temp;
			}
			else return default (T);
		}
		catch(System.Exception exception){
			Debug.Log("Malformed file brah: " + fileName);
			return default (T);
		}
	}

	public static string GetFileInternal(string fileName){
		string finalPath = "Assets/Resources/"+fileName+fileExtenstion;
		try
		{
			if(File.Exists(finalPath)){
				string file = File.ReadAllText(finalPath);
				return file;
			}
			else return "";
		}
		catch(System.Exception exception){
			Debug.Log("Malformed file brah: " + fileName);
			return "";
		}
	}

	public static T LoadInternal<T>(string fileName){
		string finalPath = "Assets/Resources/"+fileName+fileExtenstion;
		try
		{
			if(File.Exists(finalPath)){
				Type type = typeof(T);
				BinaryFormatter bf = new BinaryFormatter();
				string file = File.ReadAllText(finalPath);
				var temp = (T)(object)Deserialize(type, file);
				return temp;
			}
			else return default (T);
		}
		catch(System.Exception exception){
			Debug.Log("Malformed file brah: " + fileName);
			return default (T);
		}
	}

	public static void Save<T>(T item, string fileName, string comments = "") {
		Type type = typeof(T);
		string finalPath = Application.persistentDataPath+"/"+fileName+fileExtenstion;
		string serializedData = Serialize(type, item);
		Debug.Log(finalPath);
		File.WriteAllText(finalPath, serializedData+comments);
	}

	public static void SaveInternal<T>(T item, string fileName) {
		Type type = typeof(T);
		string finalPath = "Assets/Resources/"+fileName+fileExtenstion;
		string serializedData = Serialize(type, item);
		Debug.Log(finalPath);
		File.WriteAllText(finalPath, serializedData);
	}

	public static string Serialize(Type type, object value) {
		// serialize the data
		fsData data;
		var success = _serializer.TrySerialize(type, value, out data);
		//if (success.Failed) throw new Exception(success.FailureReason);

		// emit the data via JSON
		return fsJsonPrinter.PrettyJson(data);
	}

	public static object Deserialize(Type type, string serializedState) {

		// step 1: parse the JSON data
		fsData data;
		var success = fsJsonParser.Parse(serializedState, out data);
		//if (success.Failed) throw new Exception(success.FailureReason);

		// step 2: deserialize the data
		object deserialized = null;
		success = _serializer.TryDeserialize(data, type, ref deserialized);
	   // if (success.Failed) throw new Exception(success.FailureReason);

		return deserialized;
	}
}
