using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// The script teleports the snake from A to B.
/// </summary>
public class AirDuct : BaseObject, ISnakeTriggerEnter, ISnakeTriggerExit
{
    public float timer;
    public Transform
        destination,
        teleporter;
    public AudioClip enterSound;
    public GameObject dustBurstHolder;

    AudioSource audioS;
    private Transform _originalVelcroSnake;
    private Rigidbody _rigid;
    private Vector3 _startPosition;
    private bool
        _initialParentSet,
        snakeMoved;

    void Start()
    {
        _startPosition = transform.position;
        audioS = GetComponent<AudioSource>();
    }

    public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
    {
        if (col == transform.GetComponent<Collider>())
        {
            _rigid = snakeCol.GetComponent<Rigidbody>();
            if (!_initialParentSet)
            {
                _initialParentSet = true;
                _originalVelcroSnake = snakeCol.transform.parent.parent;
                teleporter.position = snakeCol.transform.position;
                //Debug.Log("initialParent= " + _originalVelcroSnake.name);
            }
            //Debug.Log("The rigidbody's 2nd parent= " + _rigid.transform.parent.parent.name);
            foreach (var x in snakeCol.rootOfSnake.GetComponent<SnakeInventory>().inventory)
            {
                x.transform.parent = teleporter;
            }
            _originalVelcroSnake.parent = teleporter;
            audioS.PlayOneShot(enterSound);
            //Debug.Log("The teleporter first child= " + teleporter.GetChild(0).name);
            //Debug.Log("Teleporter's number of children= " + teleporter.childCount);
            /*if (teleporter.childCount > 0)
            {
                for (int i = 0; i < teleporter.childCount; i++)
                {
                    Debug.Log("child name= " + teleporter.GetChild(i).name);
                }
            }*/
            dustBurstHolder.SetActive(false);
            if (!snakeMoved)
            {
                snakeMoved = !snakeMoved;
                StartCoroutine(MoveSnake(timer));
            }
        }
    }

    public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol)
    {

    }
    IEnumerator MoveSnake(float waitTime)
    {
        var rigids = _originalVelcroSnake.GetComponentsInChildren<Rigidbody>();
        foreach (var x in rigids) {
            x.isKinematic = true;
        }
        //_rigid.transform.GetChild(0).GetChild(2).GetComponent<Rigidbody>().isKinematic = true;
        // Objects stuck to the snake.
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.rigid.isKinematic = true;
            //x.rigid.Sleep();
        }
        //_rigid.Sleep();

        teleporter.position = destination.transform.position;

        yield return new WaitForSeconds(waitTime);

        _originalVelcroSnake.parent = null;

        // Objects stuck to the snake.
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.transform.parent = null;
        }
        teleporter.position = _startPosition;

        //rigids = _originalVelcroSnake.GetComponentsInChildren<Rigidbody>();
        foreach (var x in rigids) {
            x.isKinematic = false;
        }
        // Jaw joint
        /*if (_rigid.transform.GetChild(2).GetComponent<Rigidbody>() != null)
        {
            _rigid.transform.GetChild(2).GetComponent<Rigidbody>().isKinematic = false;
        }*/
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.rigid.isKinematic = false;
            /*x.rigid.angularVelocity = new Vector3(0f, 0f, 0f);
            x.rigid.velocity = new Vector3(0f, 0f, 0f);*/
            //x.rigid.WakeUp();
        }
        //_rigid.WakeUp();
        //_rigid.velocity = new Vector3(0f, 0f, 0f);


        //Debug.Log("The rigidbody's 2nd parent= " + _rigid.transform.parent.parent.name);
        //Debug.Log("The rigidbody= " + _rigid.transform.parent.name);
        //Debug.Log("Teleporter's number of children= " + teleporter.childCount);
        dustBurstHolder.SetActive(true);
        snakeMoved = false;
    }
}
