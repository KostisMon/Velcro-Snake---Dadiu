using UnityEngine;
using System.Collections;

public class MovementVisualizer : BaseObject {

    public GameObject particle;
    // Use this for initialization

    protected override void LateAwake()
    {
        if(particle == null)
        {
            Debug.Log("THERE'S NO ASSIGNED PARTICLE FOR THE SNAKE'S MOVEMENT VISUALIZER!!");
        }
    }
        // Update is called once per frame
        void Update () {
	
	}

    public void SuccessfulJumpRequest(RaycastHit hit)
    {
        Instantiate(particle, hit.point, Quaternion.LookRotation(hit.normal));
    }
}
