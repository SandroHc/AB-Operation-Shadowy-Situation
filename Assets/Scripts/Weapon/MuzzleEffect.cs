using UnityEngine;
using System.Collections;

public class MuzzleEffect : MonoBehaviour {

	private new MeshRenderer renderer;

	public bool timer = false;
	public float timerDelta = 0;
	public float cooldown = .03f;

	void Awake() {
		renderer = GetComponent<MeshRenderer>();
	}

	void Update() {
		if(timer) {
			timerDelta += Time.deltaTime;
			if(timerDelta >= cooldown) {
				timer = false;
				renderer.enabled = false;
			}
		}
	}

	public void triggerEffect() {
		renderer.material = GameController.spriteManager.getMuzzle();
		renderer.enabled = true;

		timer = true;
		timerDelta = 0;
	}
}
