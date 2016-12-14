using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using FullInspector;
public class EndFrame : BaseObject, IEndGameEnd {
	//ublic RawImage image;
	public RawImage flashImage;
		public float animationDuration;
	public Ease easing;
	public AudioClip takePicSound, screenFlashSound;
	public AudioPlayer audio;
	public RawImage danish, english;
	public GameObject button1,button2;
	public Text text;

	#region IEndGameEvents implementation

	public void OnFadeOut(global::CameraFader cam)
	{

	}
	public void OnFadeIn(global::CameraFader cam)
	{

	}
	public void OnEnd()
	{
		gameObject.SetActive(true);
		text.enabled = true;
		if(!SettingsControl.settings.english){
			danish.enabled = true;
		}
		else {
			english.enabled = true;
		}

		//image.enabled = true;
		FlashWhite();
		audio.Play(screenFlashSound);
	}

	void Start(){
		gameObject.SetActive(false);
	}
	#endregion

	public void FlashWhite(){
		flashImage.color = Color.clear;
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(flashImage.DOColor(Color.white, animationDuration).SetEase(easing)).Append(flashImage.DOColor(Color.clear, animationDuration).SetEase(easing));
	}

	public void Restart(){
		Time.timeScale = 1f;
		Application.LoadLevel(0);
	}

	public void TakePic(){
		TakeScreenshot();
	}

	public IEnumerator ActualScreenshot(){
		System.DateTime dateTime = System.DateTime.Now;
		string format = "MMM_d HH-mm tt";
		string filename = "VelcroSnake2_"+dateTime.ToString(format, new System.Globalization.CultureInfo("en-US"));
		Debug.Log(filename);
		button1.SetActive(false);
		button2.SetActive(false);
		yield return new WaitForEndOfFrame();
		//yield return new WaitForEndOfFrame();
        string path = Application.persistentDataPath;

        //#if UNITY_ANDROID
        //path = "../../../../DCIM/Camera";
        //#endif

        Debug.Log(path);
        Application.CaptureScreenshot(path +"/"+ filename +".png");
			button1.SetActive(false);
			button2.SetActive(true);
		Time.timeScale = 0f;
	}
	[InspectorButton]
	public void TakeScreenshot(){
		Time.timeScale = 0.3f;
		StartCoroutine(ActualScreenshot());
		audio.Play(takePicSound);
	}
}
