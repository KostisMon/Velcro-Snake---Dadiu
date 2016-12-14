using UnityEngine;
using System.Collections;

public class BackgroundMusic : BaseObject, IEndGameFadeOut {
	AudioSource source;
	#region IEndGameEvents implementation
	public void OnFadeOut(global::CameraFader cam)
	{
		source = GetComponent<AudioSource>();
		source.Stop();
	}
	#endregion
}
