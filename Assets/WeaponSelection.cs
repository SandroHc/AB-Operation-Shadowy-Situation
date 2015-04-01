using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSelection : MonoBehaviour {
	public RectTransform weaponSelection;

	public int index;
	public float target;

	void Awake() {
		target = weaponSelection.localRotation.eulerAngles.z;
	}

	void Update() {
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		if(scroll > 0) { // Up
			index = Mathf.Max(1, index-1);
			updateTarget();
		} else if(scroll < 0) { // Down
			index = Mathf.Min(5, index+1);
			updateTarget();
		}



		float targetSmooth = Mathf.Lerp(weaponSelection.localRotation.eulerAngles.z, target, Time.deltaTime * 25);

		weaponSelection.localRotation = Quaternion.Euler(0, 0, targetSmooth);
	}

	private void updateTarget() {
		target = (270 - 43.6f * (index-1)) % 360;
	}
}
