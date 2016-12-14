using UnityEngine;
using System.Collections;

public class EyeJittering : BaseObject {

    public Transform leftEye;
    public Transform rightEye;
    public Transform target;
    public float lookSpeed = 4;

	// Use this for initialization
	void Start ()
    {
	    
	}

    protected override void LateAwake()
    {
        StartCoroutine(RandomEyeMovement());
    }

    // Update is called once per frame
    void Update ()
    {
        leftEye.rotation = Quaternion.Lerp(leftEye.rotation, Quaternion.LookRotation(target.position - leftEye.position), Time.deltaTime * lookSpeed);
        rightEye.rotation = Quaternion.Lerp(rightEye.rotation, Quaternion.LookRotation(target.position - rightEye.position), Time.deltaTime * lookSpeed);
    }

    IEnumerator RandomEyeMovement()
    {
        while (true)
        {
            target.localPosition = Random.insideUnitSphere * Random.Range(1, 5);
            target.localPosition = new Vector3(target.localPosition.x, Mathf.Abs(target.localPosition.y), Mathf.Abs(target.localPosition.z));
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }
}
