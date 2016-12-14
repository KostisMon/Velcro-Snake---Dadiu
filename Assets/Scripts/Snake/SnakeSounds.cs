using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SnakeSounds : BaseObject, ISnakeEvents, ISnakeCollisionEnter {

    private AudioSource audioS;
    public HashSet<Collider> colliders = new HashSet<Collider>();
    public AudioClip jump, hit;
    public float maxHitVelocity = 30;

    public void Start()
    {
        audioS = GetComponent<AudioSource>();

        foreach (var x in transform.GetComponentsInChildren<Collider>())
        {
            colliders.Add(x);
        }
    }

    public void OnSnakeEvent(SnakeEvent snakeEvent)
    {
        if (snakeEvent == SnakeEvent.Jump)
        {
            audioS.PlayOneShot(jump);
        }
    }

    public void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol)
    {
        BaseObject obj;
        ColliderCache.cachedObjects.TryGetValue(col.collider, out obj);

        if(!(obj is SnakeCollider))
        {
            //audioS.PlayOneShot(hit, VelosityToVolume(snakeCol.rigid));
        }
    }

    public void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol)
    {

    }

    float VelosityToVolume(Rigidbody rb)
    {
        float tempVelosity = Mathf.Clamp(rb.angularVelocity.magnitude, 0, maxHitVelocity);
        tempVelosity = tempVelosity / maxHitVelocity;
        return tempVelosity;
    }
}
