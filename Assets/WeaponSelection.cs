using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSelection : MonoBehaviour {
	public RectTransform weaponSelection;
	private Vector3 scale;

	public int index;
	public float target;

	public bool show = false;
	private float timerHide = 1f; // Hide in 1 second
	private float timer;

	void Awake() {
		target = weaponSelection.localRotation.eulerAngles.z;

		scale = weaponSelection.localScale;
		weaponSelection.localScale = Vector3.zero;
	}

	void Update() {
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		// Reset the timer each time the scroll button os "pressed" (more like spinned?)
		if(scroll != 0) {
			timer = 0f;
			show = true;
		}

		if(show) {
			timer += Time.deltaTime;
			if(timer >= timerHide)
				show = false;

			if(scroll > 0) { // Up
				index = Mathf.Max(1, index-1);
				updateTarget();
			} else if(scroll < 0) { // Down
				index = Mathf.Min(5, index+1);
				updateTarget();
			}

			float targetSmooth = Mathf.Lerp(weaponSelection.localRotation.eulerAngles.z, target, Time.deltaTime * 25);

			weaponSelection.localRotation = Quaternion.Euler(0, 0, targetSmooth);
			weaponSelection.localScale = Vector3.Slerp(weaponSelection.localScale, scale, Time.deltaTime * 10);
		} else {
			weaponSelection.localScale = Vector3.Slerp(weaponSelection.localScale, Vector3.zero, Time.deltaTime * 10);
		}
	}

	private void updateTarget() {
		target = (270 - 43.6f * (index-1)) % 360;
	}
}
