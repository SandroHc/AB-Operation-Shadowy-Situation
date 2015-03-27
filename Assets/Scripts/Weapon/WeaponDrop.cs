using UnityEngine;
using System.Collections;

public class WeaponDrop : MonoBehaviour {

	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			drop();
		}
	}

	public void drop() {
		//Add a rigidbody (for the weapon to fall on the ground)
		gameObject.AddComponent<Rigidbody>();

		// Set the parent to null (so the Camera can't change its local position)
		gameObject.transform.SetParent(null);


		InteractDistance interactScript = GetComponent<InteractDistance>();
		if(interactScript != null)
			interactScript.setActive(true);
	}

	public void pick() {
		// Remove the rigidbody (to prevent the weapon falling to the ground)
		Destroy(GetComponent<Rigidbody>());

		// Reset the parent to the player Camera
		gameObject.transform.SetParent(Camera.main.gameObject.transform);

		// Reset position and rotation
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);


		InteractDistance interactScript = GetComponent<InteractDistance>();
		if(interactScript != null)
			interactScript.setActive(false);
	}
}
