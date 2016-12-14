using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class ParticleEffect : BaseObject {
	public List<ParticleSystem> psys = new List<ParticleSystem>();

	public void PlayOnce(){
		StartCoroutine(Play(0));
	}

	public IEnumerator Play(int loops){
		foreach(var x in psys){
			x.Play();
		}
		yield return null;
	}

	protected override void LateAwake()
	{
		foreach (var x in psys) {
			if(x==null){
				Debug.Log("Don't do this to me");
				return;
			}
		}
	}
}
