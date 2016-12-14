using UnityEngine;
using System.Collections;
using FullInspector;

public class PathFollower : BaseObject {

	protected Animator animator;
	public string animationBoolName = "flying";
	protected bool isPlaying;

	[InspectorButton]
	public void StartPath(){
		if(animator==null){
			animator = GetComponent<Animator>();
		}
		animator.applyRootMotion = false;
		animator.SetBool(animationBoolName.ToLower(), true);
		animator.speed = 1f;
		isPlaying = true;
	}

	[InspectorButton]
	public void StopPath(){
		isPlaying = false;
		animator.speed = 0f;
		animator.SetBool(animationBoolName.ToLower(), false);
	}
}
