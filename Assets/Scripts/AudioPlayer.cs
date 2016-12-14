using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : BaseObject {
	public List<AudioSource> audioSources = new List<AudioSource>();
	const int minimumSources = 2;

	protected override void EarlyAwake(){
		audioSources.AddRange(GetComponents<AudioSource>());
		for (int i = audioSources.Count; i<minimumSources; i++) {
			AudioSource source = gameObject.AddComponent<AudioSource>();
			audioSources.Add(source);
		}
	}

	public void PlayLooping(AudioClip clip){
		Play(clip, true);
	}

	public void Play(AudioClip clip, bool looping = false){
        
		if(clip==null){
			clip = Resources.Load("PlaceholderSound") as AudioClip;
		}

		foreach (var x in audioSources) {
			if(x.isPlaying){

			}
			else {
				x.loop = looping;
				x.clip = clip;
				x.Play();
				return;
			}
		}
	}

	public void Stop(AudioClip toStop){
		foreach (var x in audioSources) {
			if(x==toStop || toStop == null){
				x.Stop();
			}
		}
	}

	public void StopAll(){
		Stop(null);
	}
}
