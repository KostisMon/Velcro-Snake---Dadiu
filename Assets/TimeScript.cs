using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScript : MonoBehaviour {
	public Text text;
	// Use this for initialization
	void Start () {
		text.text = System.DateTime.Now.ToString("dd.MM.yyyy");
	}

	// Update is called once per frame
	void Update () {

	}
}
