using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SoundMaker : BaseObject, ISnakeTriggerEnter,ISnakeTriggerExit  , ISnakeCollisionEnter, ISnakeCollisionExit , ISnakeItemPickup, ISnakeItemDrop{

    AudioSource audioS;
    private bool justPickedup;
    public bool useColliderOnly;
    public HashSet<Collider> colliders = new HashSet<Collider>();
    public AudioClip triggerAudio;
    public AudioClip colliderAudio;

    // Use this for initialization
    void Start ()
    {
        audioS = GetComponent<AudioSource>();
        if(colliders.Count == 0)
        {
            foreach (var x in GetComponents<Collider>())
            {
                colliders.Add(x);
            }

            foreach (var x in transform.GetComponentsInChildren<Collider>())
            {
                colliders.Add(x);
            }
        }
	}

    public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
    {
        if(colliders.Contains(col) && !useColliderOnly)
        {
            audioS.PlayOneShot(triggerAudio);
        }
    }

    public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol)
    {

    }

    public void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol)
    {
        if(colliders.Contains(col.collider) && justPickedup)
        {
            audioS.PlayOneShot(colliderAudio);
            justPickedup = false;
        }

    }

    public void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol)
    {

    }

    public void OnSnakeItemPickup(StickyItem obj, SnakeInventory snakeInv)
    {
        if(obj.name == gameObject.name)
        {
            justPickedup = true;
        }
    }

    public void OnSnakeItemDrop(StickyItem obj, SnakeInventory snakeInv)
    {

    }
}
