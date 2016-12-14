using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using FullInspector;

public class GroceryCrossOut : BaseObject, IObjectiveComplete{
	public AudioClip crossOutSound;
	public float animationDuration = 1f;
	public Ease easing;
	public Vector2 Position;
	public Image image;
	Sequence mySequence;
	public float moveAmount;
	AudioPlayer audioPlayer;
	public GroceryItem connectedGroceries;

	#region IObjectiveEvents implementation
	public void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item)
	{

	}
	public void OnObjectiveComplete(LevelObjective lvlObj)
	{
		if(connectedGroceries.connectedObjective == lvlObj){
			MoveMe();
		}
	}
	#endregion
	[InspectorButton]
	public void MoveMe(){
		mySequence = DOTween.Sequence();
		//audioPlayer.Play(crossOutSound);
		image.color = Color.white;
		//mySequence.Append(image.DOFade(Color.white, animationDuration, false).SetEase(easing));
	}
	protected override void LateAwake()
	{
		audioPlayer = GetComponent<AudioPlayer>();
		image = GetComponent<Image>();
	}
}
