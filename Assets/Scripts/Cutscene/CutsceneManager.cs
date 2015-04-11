using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
	public static Camera cutsceneCamera;
	public static new Animation animation;
	private Camera playerCamera;

	public Cutscene cutscene;
	
	public void Start() {
		cutsceneCamera = GameObject.FindWithTag(Tags.cutsceneCamera).GetComponent<Camera>();
		animation = cutsceneCamera.GetComponent<Animation>();
	}

	public void cancelBtnClicked() {
		if(animation.isPlaying) {
			Debug.Log("Cancelling cutscene");
			
			//stopCutscene();
			if(cutscene != null)
				cutscene.nextStage();
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

		// Send a quest progress update with the cutscene ID
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.CUTSCENE).setStr(cutscene.getName()));
		

		cutscene.stopCutscene();

		if(animation.isPlaying)
			animation.Stop();

		/*foreach(AnimationClip state in animation)
			animation.RemoveClip(state);*/

		GameController.setFocused(false);
		GameController.playerController.enabled = true;
		
		if(playerCamera != null) playerCamera.enabled = true;
		cutsceneCamera.enabled = false;

		// Remove the reference to the, now finished, cutscene
		cutscene = null;
	}

	public static void playAnimation(string name, bool playImmediate = false) {
		if(playImmediate)
			animation.Play(name);
		else
			animation.PlayQueued(name);
	}
}
