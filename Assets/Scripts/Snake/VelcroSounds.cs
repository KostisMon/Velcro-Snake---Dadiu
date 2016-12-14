using UnityEngine;
using System.Collections;
using System;

public class VelcroSounds : BaseObject, ISnakeItemPickup {

    AudioSource audioS;
    public AudioClip velcroSound;

    public void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    public void OnSnakeItemDrop(StickyItem obj, SnakeInventory snakeInv)
    {

    }

    public void OnSnakeItemPickup(StickyItem obj, SnakeInventory snakeInv)
    {
        if(obj.icon == null)
        audioS.PlayOneShot(velcroSound);
    }
}
