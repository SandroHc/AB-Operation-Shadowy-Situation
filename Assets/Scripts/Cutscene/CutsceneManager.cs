using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
	public static Camera cutsceneCamera;
	private Animation animation;
	private Camera playerCamera;

	public Cutscene cutscene;
	
	public void Start() {
		cutsceneCamera = GameObject.FindWithTag(Tags.cutsceneCamera).GetComponent<Camera>();
		animation = cutsceneCamera.GetComponent<Animation>();
	}

	public void LateUpdate() {
		if(animation.isPlaying && Input.GetButtonDown("Cancel")) {
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

		if(animation.isPlaying)
			animation.Stop();

		GameController.setFocused(false);
		GameController.playerController.enabled = true;
		
		if(playerCamera != null) playerCamera.enabled = true;
		cutsceneCamera.enabled = false;

		// Remove the reference to the, now finished, cutscene
		cutscene = null;
	}
}
