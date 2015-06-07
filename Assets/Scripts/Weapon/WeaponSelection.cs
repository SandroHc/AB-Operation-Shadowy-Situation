using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSelection : MonoBehaviour {
	private static WeaponSelection INSTANCE;

	public RectTransform weaponSelection;
	private Vector3 scale;

	public RectTransform[] pieces;

	public static int index;
	public float target;

	public bool show = false;
	private float timerHide = 1f; // Hide in 1 second
	private float timer;

	private bool changed = false;

	void Awake() {
		INSTANCE = this;

		// Get the initial rotation value for the Z axis
		target = weaponSelection.localRotation.eulerAngles.z;

		// The UI scale when showing
		scale = Vector3.one * .35f;
	}

	void Update() {
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		// Set this every update time in case a key is pressed, instead of only updating when showing the selection
		int old = index;

		// Check for key input
		if(!GameController.isPausedOrFocused()) {
			// Reset the timer each time the scroll button is "pressed" (more like spinned?)
			if(scroll != 0) {
				timer = 0f;
				show = true;
			}

			for(int i=1; i <= 5; i++) {
				if(Input.GetKeyDown(i.ToString())) {
					index = i-1; // Change the index here, so the change is detected below
					updateTarget();

					// Reset the timer
					timer = 0f;
					show = true;
				}
			}

			if(show) {
				if(scroll > 0) { // Up
					index = Mathf.Max(0, index-1);
					updateTarget();
				} else if(scroll < 0) { // Down
					index = Mathf.Min(4, index+1);
					updateTarget();
				}
				
				// Smooth transition to highlight current weapon
				if(old != index) {
					pieces[old].transform.localScale = Vector3.one;
					changed = true;
				}
				pieces[index].transform.localScale = Vector3.Slerp(pieces[index].transform.localScale, Vector3.one * 1.03f, Time.deltaTime * 10);
				
				// Smooth the angle torwards the target
				float targetSmooth = Mathf.Lerp(weaponSelection.localRotation.eulerAngles.z, target, Time.deltaTime * 25);
				
				weaponSelection.localRotation = Quaternion.Euler(0, 0, targetSmooth);
				weaponSelection.localScale = Vector3.Slerp(weaponSelection.localScale, scale, Time.deltaTime * 10);
				
				// Increment the timer
				timer += Time.deltaTime;
				if(timer >= timerHide) {
					show = false;
					
					if(changed) {
						indexChanged();
						changed = false;
					}
				}
			} else {
				weaponSelection.localScale = Vector3.Slerp(weaponSelection.localScale, Vector3.zero, Time.deltaTime * 10);
			}
		} else {
			weaponSelection.localScale = Vector3.Slerp(weaponSelection.localScale, Vector3.zero, Time.deltaTime * 10);
		}
	}

	private void indexChanged() {
		WeaponManager.switchSlot(index);
	}

	private void updateTarget() {
		target = (270 - 43.6f * index) % 360;
	}

	public static void updateIcon(Weapon weapon) {
		INSTANCE.pieces[weapon.getSlot()].Find("img").GetComponent<Image>().sprite = weapon.icon ?? GameController.spriteManager.weaponNoIcon;
    }
}
