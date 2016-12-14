using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class GroceryList : BaseObject, IEventInvoker, IObjectivePickup, IPointerDownHandler, IPauseEnter, IPauseExit, IEndGameFadeIn{
	#region IEndGameEvents implementation
	public void OnFadeOut(CameraFader cam)
	{

	}
	public void OnFadeIn(CameraFader cam)
	{
		gameObject.SetActive(false);
	}
	public void OnEnd()
	{

	}
	#endregion

	public Vector2 closedPos;
	public Vector2 openPos;
	private RectTransform rect;
	public float animationDuration = 1f;
	public Ease easing;
	public Ease peekEasing;
	private float peekTime = 1.45f;
	public float normalPeekTime = 1.45f;
	public float allCollectedPeekTime = 2.5f;

	bool isOpen;

	[InspectorButton]
	public void FadeOpen(){
		//rect.anchoredPosition = closedPos;
		isOpen = true;
		//DOTween.Kill(rect);
		rect.DOAnchorPos(openPos, animationDuration, false).SetEase(easing);
	}

	[InspectorButton]
	public void FadeClosed(){
		//rect.anchoredPosition = openPos;
		isOpen = false;
		//DOTween.Kill(rect);
		rect.DOAnchorPos(closedPos, animationDuration, false).SetEase(easing);
	}
	[InspectorButton]
	public void Peek(){
		rect.DOAnchorPos(openPos, animationDuration/2f, false).SetEase(peekEasing).OnComplete(PeekBack);
	}
	public void PeekBack(){
		rect.DOAnchorPos(closedPos, animationDuration, false).SetEase(peekEasing).SetDelay(peekTime);
	}
	protected override void LateAwake()
	{
		rect = GetComponent<RectTransform>();
		rect.anchoredPosition = closedPos;
	}
	public void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item){
		if(lvlObj.AllItemsCollected){
			peekTime = allCollectedPeekTime;
		}
		else {
			peekTime = normalPeekTime;
		}
		Peek();
	}
	public void OnObjectiveComplete(LevelObjective lvlObj){

	}

	public void ToggleOpen(){
		DOTween.Kill(rect);
		isOpen = !isOpen;
		if(isOpen)
			FadeOpen();
		else
			FadeClosed();
	}

	public void OnPointerDown(PointerEventData data){
		if(data.selectedObject != gameObject){
			if(isOpen)
				FadeClosed();
		}
	}
	bool paused;
	public void Pause(){
		FadeClosed();
		if(!paused)
			eventManager.InvokeEvent<IPauseEnter>();
		else
			eventManager.InvokeEvent<IPauseExit>();

        //print(paused);
		//paused = !paused;
	}

    public void OnPauseEnter()
    {
        paused = true;
    }

    public void OnPauseExit()
    {
        paused = false;
    }
}
