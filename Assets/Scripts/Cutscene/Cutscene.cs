using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Cutscene : MonoBehaviour {
	protected List<Stage> stageList = new List<Stage>();
	protected Stage currentStage;

	protected new string name;

	public Cutscene(string name) {
		this.name = name;
	}

	public void setupCutscene() {
		// Copy the animations of this Game object to the Camera
		Animation targetAnimation = CutsceneManager.animation;
		foreach(AnimationState state in GetComponent<Animation>()) {
			targetAnimation.AddClip(state.clip, state.name);
		}

		// Set the current stage to the first one
		currentStage = stageList[0]; 
		currentStage.setupStage(this);
	}

	public void startCutscene() {
		if(currentStage != null)
			currentStage.startStage();
	}
	
	public virtual void stopCutscene() {
		// Cear the reference to the current stage.
		// Ensures that there is no way for the stage to interfere
		// with this cutscene, effectively stopping it.
		if(currentStage != null) {
			currentStage.setCutscene(null);
			currentStage = null;
		}
	}

	public void nextStage() {
		// If the current stage is null (WHY?), reset back to the first stage
		if(currentStage == null) {
			currentStage = stageList[0];
			return;
		}

		// Try to get the index of the next stage
		int index = stageList.IndexOf(currentStage);
		if(index != -1 && ++index < stageList.Count) {
			currentStage = stageList[index];
		} else {
			// Most probably, we hit the final stage
			GameController.cutsceneManager.stopCutscene();
			return;
		}

		// Send the events to cofing and start the new stage
		currentStage.setupStage(this);
		currentStage.startStage();
	}

	public void finishAnimation() {
		// Send the event to the stage
		if(currentStage != null)
			currentStage.finishAnimation();
	}

	public string getName() {
		return name;
	}

	public abstract class Stage {
		protected Cutscene cutscene;

		abstract public void startStage();

		public void setupStage(Cutscene cutscene) {
			setCutscene(cutscene);
		}

		public void stopStage() {
			if(cutscene == null) return;

			// Go to the next stage in the cutscene
			cutscene.nextStage();
		}

		public virtual void finishAnimation() { }

		public void setCutscene(Cutscene cutscene) {
			this.cutscene = cutscene;
		}
	}
}
