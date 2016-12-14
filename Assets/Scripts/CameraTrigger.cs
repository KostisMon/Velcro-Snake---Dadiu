using UnityEngine;
using System.Collections;

public class CameraTrigger : BaseObject {

    public class CameraSettings
    {
        [HideInInspector]
        public Quaternion rotation;
        [HideInInspector]
        public Vector3 position;
        public bool followSnake = false;
        public float FOV;
        public float distance = -1f;
        public float height = -1f;
        public float targetHeightOffset = -1f;
        public float translationSpeed = -1f;
        public float rotationSpeed = -1f;
        public bool hasCamera;
    }

    public CameraSettings cameraSettings;
    public Transform targetCamera;
    public Camera camRef;

    protected override void LateAwake()
    {
        if(targetCamera!=null){
            camRef =  targetCamera.GetComponent<Camera>();
            cameraSettings.hasCamera = true;

            cameraSettings.FOV = camRef.fieldOfView;
            cameraSettings.rotation = targetCamera.rotation;
            cameraSettings.position = targetCamera.position;

            Destroy(targetCamera.gameObject);
        }
    }

    void OnDrawGizmos() {
            Gizmos.color = Color.red;
            if(targetCamera!=null)
                Gizmos.DrawSphere(targetCamera.position,0.5f);
    }
}
