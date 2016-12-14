using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using FullInspector;

public class EndCar : BaseObject, IEventInvoker, IEndGameFadeIn, IEndGameFadeOut{
	#region IEndGameEvents implementation
	public void OnFadeOut(CameraFader cam)
	{
		audio = GetComponent<AudioPlayer>();
		audio.Play(doorSound);
		camFade = cam;

	}
	public void OnFadeIn(CameraFader cam)
	{
		OnObjectiveComplete();
	}
	public void OnEnd()
	{

	}
	#endregion

	public float animationDuration;
	public float waitTime;
	public Ease easing;
	public Transform destination;
	bool moving;
	public float carSoundTime;
	public float driveSoundTime;
	public AudioClip carStart;
	public AudioClip carDrive;
	public AudioClip doorSound;
	private AudioPlayer audio;
	private CameraFader camFade;


	public void OnObjectiveComplete()
	{
		if(!moving){
			moving = true;
			Drive();
		}
	}


	public void Drive(){

		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(transform.DOMove(destination.position, animationDuration).SetEase(easing).OnComplete(()=>DriveComplete())).SetDelay(waitTime).InsertCallback(carSoundTime, StartCarSound).InsertCallback(driveSoundTime, DriveSound);
	}


	public void StartCarSound(){
		audio.Play(carStart);
	}

	public void DriveSound(){
		audio.Play(carDrive);
	}

	void DriveComplete(){

		eventManager.InvokeEvent<IEndGameEnd>();
	}
}
