using UnityEngine;
using System.Collections;


public class SettingsControl : BaseObject{
	public static Settings settings;

	protected override void EarlyAwake()
	{
        settings = Serializer.Load<Settings>("settings");
        if(settings==null){
        	settings = new Settings();
        	Serializer.Save<Settings>(settings, "settings");
        }
	}

	public static void Save(){
		Serializer.Save<Settings>(settings, "settings");
	}
}

[System.Serializable]
public class Settings{
	public bool english = true;
	public bool muted = false;
}
