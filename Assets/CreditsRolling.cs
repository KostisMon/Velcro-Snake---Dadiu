using UnityEngine;
using System.Collections;

public class CreditsRolling : BaseObject {

    public GameObject creditsText;
	public MenuClicker menuClicker;
	private Vector3  startPos;

	void Start() {

		startPos = transform.position;

	}

    void Update()
    {

        creditsText.transform.position += Vector3.up *  Time.deltaTime * 0.08f;
		if (menuClicker.fridgeIsClosed) {
			transform.position = startPos;
		}
	}
}
