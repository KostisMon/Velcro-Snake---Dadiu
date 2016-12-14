using UnityEngine;
using System.Collections;
using System;

public class CameraFollow : BaseObject, IPauseExit, IPauseEnter, ISnakeTriggerEnter, ISnakeTriggerExit, IEventInvoker{
	public Transform target;
	public float distance = 3;
	public float height = 2.0f;
	public float targetHeightOffset = 1f;
	public float translationSpeed = 2.0f;
	public float rotationSpeed = 3.0f;
	private Transform orgTarget;
	public Transform fridge;
    private CameraTrigger.CameraSettings cameraTriggerSettings;
    bool useCameraTriggerSettings = false;
    bool pausedGame = false;
    bool menuZoomIn = false;
    MenuClicker menu;
    Camera cam;

    public Camera fridgeCamera;
    public Camera fridgeZoomCamera;
    float clickWeight = 0;
    Vector3 movementVector = Vector3.zero;

    public float clickWeightDecreaseTime = 1.3f;
    public float clickWeightOrg = 0.2f;

	protected override void LateAwake() {
        cam = GetComponent<Camera>();
		if(target==null){
			Debug.Log("no camera target.\nShutting off CameraFollow");
			this.enabled = false;
		}
		if(fridgeCamera == null){
            menu = GameObject.FindObjectOfType<MenuClicker>();
            if (menu!=null)
            {
                Camera tempFridgeCamera = menu.transform.GetComponentInChildren<Camera>();
                if (tempFridgeCamera != null)
                    fridgeCamera = tempFridgeCamera;
                else
                {
                    Debug.Log("NO CAMERA FOUND AMONG FRIDGE CHILDREN! TRYING TO MAKE A SUBSTITUTE");
                    GameObject tempObj = new GameObject();
                    tempObj.name = "SUBSTITUTE FRIDGE CAMERA";
                    tempObj.transform.parent = menu.transform;
                    tempObj.transform.position = menu.transform.TransformPoint(-0.5f, 1, 3);
                    tempObj.transform.rotation = menu.transform.rotation;
                    tempObj.transform.Rotate(Vector3.up * 180);
                    Camera tempCam = tempObj.AddComponent<Camera>();
                    tempCam.enabled = false;
                    fridgeCamera = tempCam;
                }

                tempFridgeCamera = null;
                Camera[] tempArrayOfCameras = fridgeCamera.transform.GetComponentsInChildren<Camera>();
                foreach(Camera c in tempArrayOfCameras)
                {
                    if(c.transform != fridgeCamera.transform)
                    {
                        tempFridgeCamera = c;
                        break;
                    }
                }
                if(tempFridgeCamera != null)
                {
                    fridgeZoomCamera = tempFridgeCamera;
                }
                else
                {
                    Debug.Log("NO ZOOM CAMERA FOUND ON FRIDGE! CAN'T ZOOM! CRYING IS ALLOWED! CHECK YOUR FRIDGE PREFAB!");
                    GameObject tempObj = new GameObject();
                    tempObj.name = "SUBSTITUTE ZOOM CAMERA";
                    tempObj.transform.parent = fridgeCamera.transform;
                    tempObj.transform.position = fridgeCamera.transform.TransformPoint(-0.5f, 1, 3);
                    tempObj.transform.rotation = fridgeCamera.transform.rotation;
                    Camera tempCam = tempObj.AddComponent<Camera>();
                    tempCam.fieldOfView = fridgeCamera.fieldOfView;
                    tempCam.enabled = false;
                    fridgeZoomCamera = tempCam;
                }
			}
			else {
				Debug.Log("NO FRIDGE FOUND IN SCENE! CAMERA IS NOW NO WORK FOR YOU...");
				this.enabled = false;
			}
		}
		orgTarget = target;
	}
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
                eventManager.InvokeEvent<IPauseExit>();
            else
                eventManager.InvokeEvent<IPauseEnter>();
        }
        clickWeight = Mathf.Clamp(clickWeight - Time.deltaTime * clickWeightOrg / clickWeightDecreaseTime, 0, 0.5f);
    }

    protected override void EarlyAwake()
    {
        eventManager.InvokeEvent<IPauseEnter>();
    }

	void LateUpdate () {
		Vector3 newPos = KeepDistance(target, distance, height);
		Quaternion runDir = transform.rotation;

        if(pausedGame)
        {
            if (menu.cameraZoomIn)
                CameraLerp(fridgeZoomCamera.transform.position, fridgeZoomCamera.transform.rotation, fridgeZoomCamera.fieldOfView);
            else
                CameraLerp(fridgeCamera.transform.position, fridgeCamera.transform.rotation, fridgeCamera.fieldOfView);
        }
        else if (!useCameraTriggerSettings)
        {
            CameraLerp(newPos, LookAtTarget(target), 60);
        }
        else
        {
            CameraLerp(cameraTriggerSettings.position, (cameraTriggerSettings.followSnake ? LookAtTarget(target) : cameraTriggerSettings.rotation), cameraTriggerSettings.FOV);
        }
	}

    void CameraLerp(Vector3 newPos, Quaternion newRot)
    {
        CameraLerp(newPos, newRot, Camera.main.fieldOfView);
    }

    void CameraLerp(Vector3 newPos, Quaternion newRot, float newFov)
    {
        if (!Physics.Raycast(transform.position, newPos - transform.position, Mathf.Min(Vector3.Distance(transform.position, newPos), 1)))
        {
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * translationSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * rotationSpeed);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newFov, Time.deltaTime * 4);
        }
        else
        {
            transform.position = newPos;
            transform.rotation = newRot;
            cam.fieldOfView = newFov;
        }
    }

	Vector3 KeepDistance(Transform target, float distance, float height)
    {
        //var temp = (transform.position - target.position).normalized * distance + target.position;
        var temp = Vector3.Lerp((transform.position - target.position).normalized*distance, movementVector*distance, clickWeight) + target.position;
		temp.y = height + target.position.y;
		return temp;
	}

	Quaternion LookAtTarget(Transform target){
		var direction = (target.position+Vector3.up*targetHeightOffset) - transform.position;
		var temp = Quaternion.LookRotation(direction);
		return temp;
	}

    public void SnakeMoved(Vector3 movePoint)
    {
        movementVector = (target.position - movePoint).normalized;
        clickWeight = clickWeightOrg;
    }

	public void OnPauseEnter(){
        pausedGame = true;
		//target = fridge;
	}
	public void OnPauseExit(){
        pausedGame = false;
        //target = orgTarget;
	}

    //This stuff really should be TriggerStay...
    public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
    {
    	if(snakeCol.isHead){
	        if (col.transform.parent == null || snakeCol.transform != target)
	            return;
	        CameraTrigger camTrigger = col.transform.parent.GetComponent<CameraTrigger>();
	        if (camTrigger != null)
	        {
	            cameraTriggerSettings = camTrigger.cameraSettings;
	            useCameraTriggerSettings = true;
	        }
	    }
    }

    public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol)
    {
        if(snakeCol.isHead){
	        if (col.transform.parent == null || snakeCol.transform != target)
	            return;

	        CameraTrigger camTrigger = col.transform.parent.GetComponent<CameraTrigger>();
	        if (camTrigger != null)
	        {
	            useCameraTriggerSettings = false;
	        }
	    }
    }
}
