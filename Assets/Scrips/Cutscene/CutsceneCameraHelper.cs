using UnityEngine;
using System.Collections;

public class CutsceneCameraHelper : MonoBehaviour {
	public void finishAnimation() {
		if(GameController.cutsceneManager.cutscene != null)
			GameController.cutsceneManager.cutscene.finishAnimation();
	}
}
