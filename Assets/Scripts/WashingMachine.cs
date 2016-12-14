using UnityEngine;
using System.Collections;
using System;

public class WashingMachine : BaseObject, ISnakeTriggerEnter,ISnakeTriggerExit , ICollidable
{

    public bool IsWashingMachine, IsDishWasher;
    public ParticleSystem CircularBubbles, SquareBubbles1, SquareBubbles2, SquareBubbles3, SquareBubbles4;
    public Transform door, shinyCleanSparkles;
    private Collider animationTrigger;
    public int OpeningAngle = 90;
    private Collider doorCollider;
    private SnakeCollider snakeCollider;
    private bool MakeSnakeShine;
    static bool doorMoving;
    bool doorOpen = true;

    public AudioSource audioS;
    public AudioClip WashingSound;
    public Transform openDoorRotator;
    public Transform closedDoorRotator;
    public AnimationCurve openAnim;
    public AnimationCurve closeAnim;
    float openingWeight = 1;
    float washingTimer = 3;

	// Use this for initialization

    protected override void LateAwake()
    {
        animationTrigger = GetComponent<Collider>();
        audioS = GetComponent<AudioSource>();
        door.rotation = openDoorRotator.rotation;
    }

    // Update is called once per frame
    void Update ()
    {
        if (MakeSnakeShine)
        {
            shinyCleanSparkles.position = snakeCollider.transform.position;
        }

	}
    public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
    {
        snakeCollider = snakeCol;
        if (col == animationTrigger && !doorMoving)
        {
            if (doorOpen)
            {
                //door.Rotate(new Vector3(0, -90, 0));
                StartCoroutine(CloseDoor());
                doorOpen = false;
            }
            else
            {
                //door.Rotate(new Vector3(0, 90, 0));
                StartCoroutine(OpenDoor());
                doorOpen = true;
            }
        }
    }

    public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol)
    {

    }

    IEnumerator OpenDoor()
    {
        doorMoving = true;
        openingWeight = 0;
        while (openingWeight < 1)
        {
            door.rotation = Quaternion.Lerp(closedDoorRotator.rotation, openDoorRotator.rotation, openAnim.Evaluate(openingWeight));
            /*
            if (IsWashingMachine)
                door.rotation = Quaternion.Lerp(closedDoorRotator.rotation, doorRotator.rotation, openAnim.Evaluate(openingWeight));
            else if (IsDishWasher)
                door.rotation = Quaternion.Lerp(doorRotator.rotation, closedDoorRotator.rotation, closeAnim.Evaluate(openingWeight));
            */
            openingWeight += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        ThrowUp();
        yield return new WaitForSeconds(1);
        doorOpen = true;
        doorMoving = false;
    }

    IEnumerator CloseDoor()
    {
        doorMoving = true;
        openingWeight = 0;
        while (openingWeight < 1)
        {
            door.rotation = Quaternion.Lerp(openDoorRotator.rotation, closedDoorRotator.rotation, closeAnim.Evaluate(openingWeight));
            /*
            if (IsWashingMachine)
                door.rotation = Quaternion.Lerp(doorRotator.rotation, closedDoorRotator.rotation, closeAnim.Evaluate(openingWeight));
            else if (IsDishWasher)
                door.rotation = Quaternion.Lerp(closedDoorRotator.rotation, doorRotator.rotation, openAnim.Evaluate(openingWeight));
            */
            openingWeight += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        doorOpen = false;
        StartCoroutine(WashingCooldown());
    }

    IEnumerator WashingCooldown()
    {
        float counter = 0;
        Quaternion orgRot = transform.rotation;
        audioS.PlayOneShot(WashingSound);
        if (IsWashingMachine)
        {
            CircularBubbles.Play();
        }
        else if (IsDishWasher)
        {
            SquareBubbles1.Play();
            SquareBubbles2.Play();
            SquareBubbles3.Play();
            SquareBubbles4.Play();
        }
        while (counter < washingTimer)
        {
            transform.rotation = orgRot;
            transform.Rotate(new Vector3(UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5)));
            counter += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        CircularBubbles.Stop();
        SquareBubbles1.Stop();
        SquareBubbles2.Stop();
        SquareBubbles3.Stop();
        SquareBubbles4.Stop();
        transform.rotation = orgRot;
        audioS.Stop();
        StartCoroutine(OpenDoor());
    }

    IEnumerator RememberMyTrigger(Collider col)
    {
        yield return new WaitForSeconds(2f);
        Physics.IgnoreCollision(col, animationTrigger, false);
    }

    void ThrowUp()
    {


        foreach (Transform t in snakeCollider.transform.root.GetComponent<SnakeMovement>().jointHolder)
        {
            foreach (Collider c in t.GetComponents<Collider>())
            {
                Physics.IgnoreCollision(c, animationTrigger);
                StartCoroutine(RememberMyTrigger(c));
            }
            t.GetComponent<Rigidbody>().AddForce(-transform.right * 2.5f + Vector3.up * 3, ForceMode.VelocityChange);
        }
        foreach(StickyItem si in snakeCollider.transform.root.GetComponent<SnakeInventory>().inventory)
        {
            si.StopBeingSticky(3);
            foreach (Collider c in si.transform.GetComponents<Collider>())
            {
                if(c.gameObject.activeSelf && animationTrigger.gameObject.activeSelf){
                    Physics.IgnoreCollision(c, animationTrigger);
                    StartCoroutine(RememberMyTrigger(c));
                }
            }
            foreach(Collider c in si.transform.GetComponentsInChildren<Collider>())
            {
                if(c.gameObject.activeSelf && animationTrigger.gameObject.activeSelf){
                    Physics.IgnoreCollision(c, animationTrigger);
                    StartCoroutine(RememberMyTrigger(c));
                }
            }
            si.rigid.AddForce(-transform.right * 10 + Vector3.up * 3, ForceMode.VelocityChange);

        }
        snakeCollider.transform.root.GetComponent<SnakeInventory>().DropAllItems();
        MakeSnakeShine = true;
        StartCoroutine("SnakeShine");
    }
    IEnumerator SnakeShine()
    {
        shinyCleanSparkles.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        shinyCleanSparkles.gameObject.SetActive(false);
        MakeSnakeShine = false;
    }
}
