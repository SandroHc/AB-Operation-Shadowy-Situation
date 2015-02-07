using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.tag != Tags.player)
			return;
		
		// Copy the animations of this Game Object to the Camera
		foreach(AnimationState state in animation)
			CutsceneManager.cutsceneCamera.animation.AddClip(state.clip, state.name);
		
		// Start the cutscene
		GameController.cutsceneManager.setupCamera();
	}
}
