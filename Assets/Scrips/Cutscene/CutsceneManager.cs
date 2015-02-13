using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
	public static Camera cutsceneCamera;
	private Camera playerCamera;

	public Cutscene cutscene;
	
	public void Start() {
		cutsceneCamera = (GameObject.FindWithTag(Tags.cutsceneCamera) as GameObject).GetComponent<Camera>();
	}

	public void LateUpdate() {
		if(cutsceneCamera.animation.isPlaying && Input.GetButtonDown("Cancel")) {
			Debug.Log("Cancelling cutscene");

			stopCutscene();
		}
	}

	public void startCutscene(Cutscene cutscene) {
		if(cutscene == null) return;

		//Debug.Log("Starting cutscene");

		this.cutscene = cutscene;
		cutscene.setupCutscene();

		GameController.setFocused(true);
		GameController.playerController.enabled = false;

		playerCamera = Camera.main;
		if(playerCamera != null) playerCamera.enabled = false;
		cutsceneCamera.enabled = true;

		cutscene.startCutscene();
	}

	public void stopCutscene() {
		//Debug.Log("Stopping cutscene");

		cutscene.stopCutscene();

		if(cutsceneCamera.animation.isPlaying)
			cutsceneCamera.animation.Stop();

		GameController.setFocused(false);
		GameController.playerController.enabled = true;
		
		if(playerCamera != null) playerCamera.enabled = true;
		cutsceneCamera.enabled = false;

		// Remove the reference to the, now finished, cutscene
		cutscene = null;
	}
}
