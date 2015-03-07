using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscene1 : Cutscene {

	public Cutscene1() {
		stageList.Add(new Stage1());
		stageList.Add(new Stage2());
	}

	void OnTriggerEnter(Collider other) {
		// If the collider is not the player, cancel any interactions
		if(other.tag != Tags.player) return;
		
		// Start this cutscene
		GameController.cutsceneManager.startCutscene(this);
	}

	public class Stage1 : Stage {

		public override void startStage() {
			Debug.Log("Starting animation CutsceneTest1");

			CutsceneManager.cutsceneCamera.GetComponent<Animation>().PlayQueued("CutsceneTest1");
		}

		// Advace to the next stage when the animation reaches its end
		public override void finishAnimation() {
			stopStage();
		}
	}

	public class Stage2 : Stage {
		
		public override void startStage() {
			Debug.Log ("Starting animation CutsceneTest2");
			
			CutsceneManager.cutsceneCamera.GetComponent<Animation>().PlayQueued("CutsceneTest2");
		}

		// Advace to the next stage when the animation reaches its end
		public override void finishAnimation() {
			stopStage();
		}
	}
}
