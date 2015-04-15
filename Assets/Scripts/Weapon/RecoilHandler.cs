using UnityEngine;
using System.Collections;

public class RecoilHandler : MonoBehaviour {
	float upSpeed = 9; // controls smoothing speed
	float dnSpeed = 20; // how fast the weapon returns to original position
	
	Vector3 angleInitial; // initial angle private
	Vector3 angleCurrent = Vector3.zero; // smoothed angle
	
	private float targetX; // unfiltered recoil angle private
	private float targetY;
	
	void Awake() {
		// Save original angles
		angleInitial = transform.localEulerAngles;
	}

	void Update () {
		handleAimAngles();
	}

	private void handleAimAngles() {
		if(targetX - .005f > 0) {
			angleCurrent.x = Mathf.Lerp(angleCurrent.x, targetX, upSpeed * Time.deltaTime); // smooth movement a little
			angleCurrent.y = Mathf.Lerp(angleCurrent.y, targetY, upSpeed * Time.deltaTime);
			transform.localEulerAngles = angleInitial - angleCurrent; // move the camera or weapon
			targetX = Mathf.Lerp(targetX, 0, dnSpeed * Time.deltaTime); // returns to rest
		}
	}

	public void recoil(float value) {
		targetX += value; // Add recoil force
		targetY = Random.Range(-(value / 5), value / 5);
	}
}
