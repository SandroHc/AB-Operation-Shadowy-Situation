using UnityEngine;
using System.Collections;

public class CutsceneUtil : MonoBehaviour {
	private Camera old;

	private GameController gameController;
	private PlayerController playerController;	

	public void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		playerController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();
	}

	public void Update() {
		if(gameObject.camera.animation.isPlaying && Input.GetButtonDown("Cancel")) {
			Debug.Log("Cancelling cutscene");

			finishCutscene();
		}
	}

	public void startCutscene() {
		Debug.Log ("Starting cutscene");

		gameController.stopPlayerControls = true;
		playerController.enabled = false;

		old = Camera.main;
		old.enabled = false;
		gameObject.camera.enabled = true;

		gameObject.camera.animation.PlayQueued("CutsceneTest1");
		gameObject.camera.animation.PlayQueued("CutsceneTest2");
	/*	foreach(AnimationState state in gameObject.camera.animation) {
			Debug.Log("Playing section: " + state.name);
			gameObject.camera.animation.PlayQueued(state.name);
		}*/
	}

	public void finishCutscene() {
		Debug.Log ("Finished cutscene");

		if(gameObject.camera.animation.isPlaying)
			gameObject.camera.animation.Stop();

		gameController.stopPlayerControls = false;
		playerController.enabled = true;
		
		old.enabled = true;
		gameObject.camera.enabled = false;
	}
}
