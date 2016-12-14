using UnityEngine;
using System.Collections;

public class levelCompleted : BaseObject, IObjectiveComplete
{
    public LevelObjective keysToApartment;
    public GameObject winTextCanvas;

    public void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item)
    {

    }

    public void OnObjectiveComplete(LevelObjective lvlObj)
    {
        if (lvlObj == keysToApartment)
        {
            StartCoroutine("WinLevel");
        }
    }
    IEnumerator WinLevel()
    {
        winTextCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        Application.LoadLevel(Application.loadedLevel);
    }
}
