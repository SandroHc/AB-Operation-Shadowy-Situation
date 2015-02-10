using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Cutscene : MonoBehaviour {
	protected List<Stage> stageList = new List<Stage>();
	protected Stage currentStage;

	public void setupCutscene() {
		// Copy the animations of this Game object to the Camera
		foreach(AnimationState state in animation) {
			CutsceneManager.cutsceneCamera.animation.AddClip(state.clip, state.name);
		}

		// Set the current stage to the first one
		currentStage = stageList[0]; 
		currentStage.setupStage();
	}

	abstract public void startCutscene();
	abstract public void stopCutscene();

	protected void nextStage() {
		// If the current stage is null (WHY?), reset back to the first stage
		if(currentStage == null) {
			currentStage = stageList[0];
			return;
		}

		currentStage.stopStage();

		int index = stageList.IndexOf(currentStage);
		if(index != -1 && ++index < stageList.Count) {
			currentStage = stageList[index];
		} else {
			// Most probably, we hit the final stage
			GameController.cutsceneManager.stopCutscene();
			return;
		}

		currentStage.setupStage();
		currentStage.startStage();
	}

	public void finishAnimation() {
		int index = stageList.IndexOf(currentStage);
		if(index >= 0 && index < stageList.Count)
			nextStage();
	}

	public abstract class Stage {
		abstract public void setupStage();
		abstract public void startStage();
		abstract public void stopStage();
	}
}
