using UnityEngine;
using System.Collections;

public class ChangeMass : BaseObject {

    public GameObject massPoint;
    Vector3 centerOfMass;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        centerOfMass = massPoint.transform.localPosition;
        rb.centerOfMass = centerOfMass;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
