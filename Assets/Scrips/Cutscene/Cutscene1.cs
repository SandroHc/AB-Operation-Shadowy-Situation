using UnityEngine;
using System.Collections;

public class Cutscene1 : MonoBehaviour {
	public Camera targetCamera;

	void OnTriggerEnter(Collider other) {
		// Copy the animations of this Game Object to the Camera
		foreach(AnimationState state in animation)
			targetCamera.animation.AddClip(state.clip, state.name);

		// Start the cutscene
		targetCamera.SendMessage("startCutscene", SendMessageOptions.DontRequireReceiver);
	}
}
