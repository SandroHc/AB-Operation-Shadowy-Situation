using UnityEngine;
using System.Collections;

public class WeaponDrop : MonoBehaviour {

	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			drop();
		}
	}

	void drop() {
		gameObject.AddComponent<Rigidbody>();

		gameObject.transform.SetParent(null);
	}
}
