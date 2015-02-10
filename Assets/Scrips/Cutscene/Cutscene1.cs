using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscene1 : Cutscene {

	public Cutscene1() {
		stageList.Add(new Stage1());
		stageList.Add(new Stage2());
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag != Tags.player) return;
		
		// Start the cutscene
		Debug.Log (GameController.cutsceneManager);
		GameController.cutsceneManager.startCutscene(this);
	}

	public override void startCutscene() {
		currentStage.startStage();
	}

	public override void stopCutscene() {
		currentStage.stopStage();
	}

	public class Stage1 : Stage {
		public override void setupStage() {
		}

		public override void startStage() {
			Debug.Log ("Starting animation CutsceneTest1");

			CutsceneManager.cutsceneCamera.animation.PlayQueued("CutsceneTest1");
		}

		public override void stopStage() {

		}
	}

	public class Stage2 : Stage {
		public override void setupStage() {
		}
		
		public override void startStage() {
			Debug.Log ("Starting animation CutsceneTest2");
			
			CutsceneManager.cutsceneCamera.animation.PlayQueued("CutsceneTest2");
		}
		
		public override void stopStage() {
			
		}
	}
}
