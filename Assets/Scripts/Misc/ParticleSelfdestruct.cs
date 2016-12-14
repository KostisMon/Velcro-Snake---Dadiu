using UnityEngine;
using System.Collections;

public class ParticleSelfdestruct : BaseObject {

    // Use this for initialization
    protected override void LateAwake()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().duration);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
