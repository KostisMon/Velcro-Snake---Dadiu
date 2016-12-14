using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioPlayer))]
public class ObjectiveCompleteEffect : BaseObject, IObjectiveComplete{
	public LevelObjective levelObjective;
	public ParticleEffect particles;
	public GameObject objectToSpawn;
	public AudioPlayer audioPlayer;
	public AudioClip completionSound;
    public bool shutUp;
	//public SoundMaker sound; //TODO: SOUND AT SOME POINT

    public void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item){

    }
    public void OnObjectiveComplete(LevelObjective lvlObj){
    	if(lvlObj == levelObjective){
    		if(particles!=null){
    			particles.PlayOnce();
    		}
    		if(objectToSpawn!=null){
    			GameObject go = Instantiate(objectToSpawn, transform.position, transform.rotation) as GameObject;

    		}
    		if(!shutUp) audioPlayer.Play(completionSound);
    	}
    }
	protected override void LateAwake()
	{
		if(levelObjective==null){
			levelObjective = GetComponent<LevelObjective>();
			if(levelObjective==null){
				Debug.Log("I need an objective to fulfil my purpose :(");
				return;
			}
		}
		audioPlayer = GetComponent<AudioPlayer>();

		if(particles!=null){
			GameObject go = Instantiate(particles.gameObject, transform.position, transform.rotation) as GameObject;
			go.transform.parent = transform;
			particles = go.GetComponent<ParticleEffect>();
		}
	}
}
