using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using FullInspector;

public class CameraFader : BaseObject, IObjectiveComplete, IEventInvoker, IEndGameEnd{
	#region IEndGameEvents implementation
	public void OnEnd()
	{
		source.Stop();
	}
	#endregion
	public Camera camera;
	public Transform cover;
	private Material mat;
	public float animationDuration;
	public float fullScale;
	public float waitTime;
	public float carShakeDuration;
	public float carShakeStrength;
	public float carShakeRandomness;
	public AudioSource source;

	public event System.Action CameraFadeCallBack;
	public Ease easing;

	protected override void LateAwake()
	{
		source = GetComponent<AudioSource>();
		if(camera==null){
			camera = GetComponent<Camera>();
		}
		mat = cover.GetComponent<Renderer>().material;
	}

	public virtual void OnObjectiveComplete(LevelObjective lvlObj){
		if(lvlObj.isMainObjective){
			FadeBlack(CameraFadeCallBack);
		}
	}

	[InspectorButton]
	public void FadeBlack(){
		FadeBlack(null);
	}

	public void NoCallBack(){}

	public void FadeBlack(System.Action callback){
		eventManager.InvokeEventFast<IEndGameFadeOut, CameraFader>(this);
		cover.localScale = Vector3.zero;

		if(callback==null){
			callback = NoCallBack;
		}

		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(
			cover.DOScale(fullScale, animationDuration)
				.SetEase(easing)
				.OnComplete(()=>FadeClear()))
				.OnComplete(()=>callback())
				.SetDelay(waitTime)
				.InsertCallback(0f, FadeBlackSound);
	}

	[InspectorButton]
	public void FadeClear(){
		eventManager.InvokeEventFast<IEndGameFadeIn, CameraFader>(this);
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(cover.DOScale(0f, animationDuration).SetEase(easing).OnComplete(()=>FadeClear())).SetDelay(waitTime).InsertCallback(0f, FadeClearSound);
	}

	public void FadeBlackSound(){
		//camera.DOShakePosition(carShakeDuration, carShakeStrength, 3, carShakeRandomness);
	}

	public void FadeClearSound(){

	}
}
