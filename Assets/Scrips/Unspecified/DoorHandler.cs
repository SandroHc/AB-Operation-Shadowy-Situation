using UnityEngine;
using System.Collections;

public class DoorHandler : MonoBehaviour {
	public bool isOpen = false;
	public bool working = false;
	private float smooth = 3f;
	private float doorOpenAngle;
	private float doorCloseAngle;

	// Tooltip variables
	private bool showTooltip = false;
	private Rect tooltipPosition;
	
	public float offset = .0005f;

	void Start() {
		doorCloseAngle = transform.rotation.y;
		doorOpenAngle = doorCloseAngle + 90; 
	}

	void Awake() { // The screen size can be changed between screen changes (Options are in Main Menu)
		tooltipPosition.x = 0; // Will force an update on the OnGUI() function
	}

	void OnGUI() {
		if(showTooltip) {
			showTooltip = false; // Re-set to false, so this is only shown when the player can interact with the door

			if(tooltipPosition.x == 0)
				tooltipPosition = new Rect(10, Screen.height - 40, Screen.width - 20, 25);

			GUI.Label(tooltipPosition, "Click E to " + (isOpen ? "close" : "open") + " door.");
		}
	}
	
	void FixedUpdate() {
		if(working) {
			if(isOpen) {
				Quaternion target = Quaternion.Euler(0, doorOpenAngle, 0);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth); // Dampen towards the target rotation

				if(transform.localRotation.y + offset >= target.y) {
					transform.localRotation = target;
					working = false;
				}
			} else {
				Quaternion target = Quaternion.Euler(0, doorCloseAngle, 0);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth); // Dampen towards the target rotation

				if(transform.localRotation.y - offset <= target.y) {
					transform.localRotation = target;
					working = false;
				}
			}
		}
	}
	
	void OnTriggerStay() {
		showTooltip = true;

		if(Input.GetButtonUp("Interact")) {
			working = true;
			isOpen = !isOpen;
		}
	}
}
