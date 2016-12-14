using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using FullInspector;

public class GroceryItem : BaseObject, IObjectiveComplete, IObjectivePickup, ISnakeItemDrop{

	public void OnSnakeItemPickup(global::StickyItem obj, global::SnakeInventory snakeInv)
	{
		Debug.Log("asd");
	}
	public void OnSnakeItemDrop(global::StickyItem obj, global::SnakeInventory snakeInv)
	{
		if(obj.UUID == connectedItem.UUID && connectedItem.UUID!= -1){
			if(connectedObjective!=null){
				if(!connectedObjective.hasFinished){
					image.sprite = defaultIcon;
				}
			}

		}
	}

	public Image image;
	public StickyItem connectedItem;
	public Sprite defaultIcon;
	//TODO: Sounds!
	public float animationDuration = 1f;
	public Ease easing;
	public float fullScale = 1f;
	RectTransform rect;
	AudioPlayer audio;
	public float soundTime =  0.8f;
	public AudioClip awardMain, awardSide;
	protected AudioClip completionSound;
	public event System.Action<bool> itemCompleteEvent;
	public LevelObjective connectedObjective;
	protected override void EarlyAwake()
	{
		if(image == null){
			image = GetComponent<Image>();
		}
		image.sprite = defaultIcon;
		audio = GetComponent<AudioPlayer>();
	}

	void UpdateIcon(){
		bool success = true;
	}

	public virtual void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item){
		StickyItem obj = item as StickyItem;
		if(connectedItem != null && obj != null){

			if(obj.UUID == connectedItem.UUID && connectedItem.UUID!= -1){
				connectedObjective = lvlObj;
				if(lvlObj.isMainObjective){
					completionSound = awardMain;
				}
				else {
					completionSound = awardSide;
				}
				image.sprite = connectedItem.icon;
				FadeOpen(false);
			}
		}
	}
	public virtual void OnObjectiveComplete(LevelObjective lvlObj){
		if(lvlObj==connectedObjective){
			image.sprite = connectedItem.icon;
		}
	}

	[InspectorButton]
	public void FadeOpen(bool droppingItem){
		rect.localScale = Vector3.zero;
		DOTween.Kill(rect);
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(rect.DOScale(fullScale, animationDuration).SetEase(easing).OnComplete(()=>TweenDone(droppingItem))).InsertCallback(soundTime, CompletionSound);
	}

	public void TweenDone(bool droppingItem){
		if(itemCompleteEvent!=null){
			itemCompleteEvent(droppingItem);
		}
	}


	public void CompletionSound(){
		if(completionSound!=null)
			audio.Play(completionSound);
	}
	protected override void LateAwake()
	{
		rect = GetComponent<RectTransform>();
	}
}
