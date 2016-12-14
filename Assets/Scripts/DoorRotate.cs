using UnityEngine;
using System.Collections;

public class DoorRotate : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	private bool doorClosed = true;
	private bool doorOpen = false;
	private Quaternion tarAngle;
	public float rotSpeed= 0.5f;


	void FixedUpdate(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.tag == "Door" && doorClosed == true){
					doorOpen = true;
				}else if(hit.collider.tag == "Door" && doorClosed== false){
					doorOpen = false;
				}
			}
		}
		if (doorOpen == true) {

			Debug.Log ("2@");
			tarAngle = Quaternion.Euler (0, 180f, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, tarAngle, Time.deltaTime * rotSpeed);
			doorClosed = false;
		} else if(doorOpen ==false) {
			tarAngle = Quaternion.Euler (0, 0, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, tarAngle, Time.deltaTime * rotSpeed);
			doorClosed = true;
		}
	}
}
