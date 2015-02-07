using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
	public static Camera cutsceneCamera;
	private Camera playerCamera;

	public void Start() {
		cutsceneCamera = gameObject.GetComponent<Camera>();
	}

	public void LateUpdate() {
		if(gameObject.camera.animation.isPlaying && Input.GetButtonDown("Cancel")) {
			Debug.Log("Cancelling cutscene");

			finishCutscene();
		}
	}

	public void startCutscene() {
		//Debug.Log("Starting cutscene");

		GameController.setFocused(true);
		GameController.playerController.enabled = false;

		playerCamera = Camera.main;
		if(playerCamera != null) playerCamera.enabled = false;
		gameObject.camera.enabled = true;

		gameObject.camera.animation.PlayQueued("CutsceneTest1");
		//gameObject.camera.animation.PlayQueued("CutsceneTest2");
	/*	foreach(AnimationState state in gameObject.camera.animation) {
			Debug.Log("Playing section: " + state.name);
			gameObject.camera.animation.PlayQueued(state.name);
		}*/
	}

	public void finishCutscene() {
		//Debug.Log("Finished cutscene");

		if(cutsceneCamera.animation.isPlaying)
			cutsceneCamera.animation.Stop();

		GameController.setFocused(false);
		GameController.playerController.enabled = true;
		
		if(playerCamera != null) playerCamera.enabled = true;
		cutsceneCamera.enabled = false;
	}
}
