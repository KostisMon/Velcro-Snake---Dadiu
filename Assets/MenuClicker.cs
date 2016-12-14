using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuClicker : BaseObject, IPauseEnter, IPauseExit, IEventInvoker
{

    private Ray ray;
    private RaycastHit hit;
    public Rotator rotator;
    private bool startButtonPressed;
    public bool resumeButtonPressed;
    private bool restartButtonPressed;
    private bool howToPlayButtonPressed;
    private bool creditsButtonPressed;
    private bool muteButtonPressed;
    private bool languageEng;
    private bool paused;
    private bool languageButtonPressed;
    private bool goBackButtonPressed;
    public bool fridgeIsClosed = true;
	private bool isStart;
    public Collider startButton;
    public Collider restartButton;
    public Collider howToPlayButton;
    public Collider creditsButton;
    public Collider muteButton;
    public Collider languageButton;
    public Collider goBackButton;
    public GameObject pizzaBox;
	public GameObject muteTapes;
    public GameObject howToPlayImage;
    public GameObject creditsImage;
    public Transform fridgeDoor;
    private Vector3 fridgeInitial;
    private Vector3 fridgeCurrent;
    private AudioSource audioS;
    private Canvas menuCanvas;
	public Sprite startButtonDK;
	public Sprite startButtonENG;
	public Sprite howToPlayENG;
	public Sprite howToPlayDK;
	public Sprite resumeButtonENG;
    public Sprite resumeButtonDK;
	public Sprite creditsBoxENG;
    public Sprite creditsBoxDK;
    public Sprite creditsDK;
    public Sprite creditsENG;
    public Sprite restartButtonDK;
    public Sprite restartButtonENG;
    public AudioClip OpenSound;
    public AudioClip CloseSound;
    public Text textENG;
    public Text textDK;

    [HideInInspector]
    public bool cameraZoomIn = false;

    void Start()
    {

        fridgeInitial = fridgeDoor.forward;
        audioS = GetComponent<AudioSource>();
        restartButton.gameObject.SetActive(false);
		languageEng = true;
		isStart = true;

        ChangeLanguage(SettingsControl.settings.english);
        Mute(SettingsControl.settings.muted);
    }

    void ChangeLanguage(bool english){
            if (!english)
            {
                if (isStart){
                    startButton.GetComponent<SpriteRenderer>().sprite = startButtonDK;

                }else{
                    startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonDK;
                }
                howToPlayButton.GetComponent<SpriteRenderer>().sprite = howToPlayDK;
                creditsButton.GetComponent<SpriteRenderer>().sprite = creditsDK;
                restartButton.GetComponent<SpriteRenderer>().sprite = restartButtonDK;
                textENG.gameObject.SetActive(false);
                textDK.gameObject.SetActive(true);
                languageEng = false;
                languageButtonPressed = false;
            }
            else
            {
                if (isStart){
                    startButton.GetComponent<SpriteRenderer>().sprite = startButtonENG;

                }else{
                    startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonENG;
                }
                howToPlayButton.GetComponent<SpriteRenderer>().sprite = howToPlayENG;
                creditsButton.GetComponent<SpriteRenderer>().sprite = creditsENG;
                restartButton.GetComponent<SpriteRenderer>().sprite = restartButtonENG;
                textENG.gameObject.SetActive(true);
                textDK.gameObject.SetActive(false);
                languageEng = true;
                languageButtonPressed = false;
            }
    }

    public void Mute(bool state){
        if (state){
            muteTapes.SetActive(true);
            AudioListener.volume = 0;
        }
        else{
            muteTapes.SetActive(false);
            AudioListener.volume = 1;
        }
    }

    void Update()
    {
        fridgeCurrent = fridgeDoor.forward;
        float angleOfDoor = Vector3.Angle(fridgeInitial, fridgeCurrent);
        if (angleOfDoor < 2)
        {
            goBackButtonPressed = false;
            fridgeIsClosed = true;
            creditsImage.SetActive(false);
            howToPlayImage.SetActive(false);
        }
        else if (angleOfDoor > 178)
        {
            howToPlayButtonPressed = false;
            creditsButtonPressed = false;
            fridgeIsClosed = false;
            cameraZoomIn = true;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == startButton)
                {
					eventManager.InvokeEvent<IPauseExit>();
                    isStart = false;
                    if (languageEng)
                    {
                        startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonENG;
                    }
                    else if (!languageEng)
                    {
                        startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonDK;
                    }
                    startButtonPressed = true;
                }
                if (hit.collider == restartButton)
                {
                    restartButtonPressed = true;

                }
                else if (hit.collider == howToPlayButton)
                {
                    howToPlayButtonPressed = true;
                    audioS.PlayOneShot(OpenSound);

                }
                else if (hit.collider == creditsButton)
                {
                    creditsButtonPressed = true;
                    audioS.PlayOneShot(OpenSound);
                }
                else if (hit.collider == muteButton)
                {
                    muteButtonPressed = true;

                }
                else if (hit.collider == languageButton)
                {
					languageButtonPressed = true;
				}
                else if (hit.collider == goBackButton)
                {
                    goBackButtonPressed = true;
                    audioS.PlayOneShot(CloseSound);
                }
            }
        }

        if (!paused)
        {
            rotator.RotateThings(0f, 5f);
            cameraZoomIn = false;

        }

        if (startButtonPressed) {

			startButtonPressed = false;
		}
		else if (restartButtonPressed) {
			Application.LoadLevel (0);
			restartButtonPressed = false;
		}
		else if (howToPlayButtonPressed && fridgeIsClosed) {
			rotator.RotateThings (180f, 5f);

			howToPlayImage.SetActive (true);
		}
		else if (creditsButtonPressed && fridgeIsClosed) {
			rotator.RotateThings (180f, 5f);

			creditsImage.SetActive (true);


		} else if (languageButtonPressed) {
			if (languageEng)
			{
				if (isStart){
					startButton.GetComponent<SpriteRenderer>().sprite = startButtonDK;

				}else{
					startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonDK;
				}
				howToPlayButton.GetComponent<SpriteRenderer>().sprite = howToPlayDK;
				creditsButton.GetComponent<SpriteRenderer>().sprite = creditsDK;
				restartButton.GetComponent<SpriteRenderer>().sprite = restartButtonDK;
				textENG.gameObject.SetActive(false);
				textDK.gameObject.SetActive(true);
				languageEng = false;
				languageButtonPressed = false;
                SettingsControl.settings.english = false;
                SettingsControl.Save();
			}
			else if (!languageEng)
			{
				if (isStart){
					startButton.GetComponent<SpriteRenderer>().sprite = startButtonENG;

				}else{
					startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonENG;
				}
				howToPlayButton.GetComponent<SpriteRenderer>().sprite = howToPlayENG;
				creditsButton.GetComponent<SpriteRenderer>().sprite = creditsENG;
				restartButton.GetComponent<SpriteRenderer>().sprite = restartButtonENG;
				textENG.gameObject.SetActive(true);
				textDK.gameObject.SetActive(false);
				languageEng = true;
				languageButtonPressed = false;
                SettingsControl.settings.english = true;
                SettingsControl.Save();
			}



		}
        else if (muteButtonPressed)
        {
            muteButtonPressed = false;
            if (AudioListener.volume == 1)
            {
				muteTapes.SetActive(true);
                AudioListener.volume = 0;
                SettingsControl.settings.muted = true;
                SettingsControl.Save();
            }
			else if(AudioListener.volume ==0){
				muteTapes.SetActive(false);
				AudioListener.volume =1;
                SettingsControl.settings.muted = false;
                SettingsControl.Save();
			}
        }
        else if (goBackButtonPressed && !fridgeIsClosed)
        {
            rotator.RotateThings(0f, 5f);
            cameraZoomIn = false;
        }
    }

    public void OnPauseEnter()
    {
        paused = true;
        startButton.gameObject.SetActive(true);
        startButton.enabled = true;
        if (isStart)
        {
            if (languageEng)
            {

                startButton.GetComponent<SpriteRenderer>().sprite = startButtonENG;
                restartButton.gameObject.SetActive(false);
            }
            else if (!languageEng)
            {
                startButton.GetComponent<SpriteRenderer>().sprite = startButtonDK;
            }

        }

        restartButton.gameObject.SetActive(true);
        languageButton.enabled = true;
        muteButton.enabled = true;
        creditsButton.enabled = true;
        howToPlayButton.enabled = true;
        goBackButton.enabled = true;
    }

    public void OnPauseExit()
    {

        if (languageEng)
        {
          startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonENG;
          restartButton.gameObject.SetActive(false);
        }
        else if (!languageEng)
        {
          startButton.GetComponent<SpriteRenderer>().sprite = resumeButtonDK;
        }

        paused = false;
        startButton.enabled = false;
        restartButton.gameObject.SetActive(false);
        howToPlayButton.enabled = false;
        creditsButton.enabled = false;
        muteButton.enabled = false;
        languageButton.enabled = false;
        goBackButton.enabled = false;
        isStart = false;
    }
}
