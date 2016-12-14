using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioPlayer))]
public class PlanePathFollower : PathFollower, ISnakeCollisionEnter, ICollidable {
	public Collider collider;
	public AudioClip takeOffSound, flyingSound, crashSound;
	public AudioPlayer audioPlayer;
	public ParticleEffect particles;
	public GameObject planeMesh;
    public SnakeTransportInc snakeTransporter;
    public SnakeStickToItem sticker;
		protected override void EarlyAwake() {
			collider = GetComponent<Collider>();
			audioPlayer = GetComponent<AudioPlayer>();
		}

		public void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol){
			BaseObject obj;
			ColliderCache.cachedObjects.TryGetValue(col.collider, out obj);
			if(obj == this && !isPlaying){
                //snakeTransporter.OnSnakeCollisionEnter(col, snakeCol);
                sticker.OnSnakeCollisionEnter(col,snakeCol);
				StartPath();
			}
		}

		public void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol){

		}

		public void TakeOff(){
			audioPlayer.Play(takeOffSound);
		}
		public void Flying(){
			audioPlayer.PlayLooping(flyingSound);
		}
		public void Crash(){
			audioPlayer.StopAll();
			audioPlayer.Play(crashSound); //silence all sources and play
			if(particles!=null){
				var go = Instantiate(particles.gameObject, transform.position, transform.rotation) as GameObject;
				particles = go.GetComponent<ParticleEffect>();
				particles.PlayOnce();
				planeMesh.SetActive(false);
			}
            sticker.DisconnectFromItem();
			collider.enabled = false;
		}
}
