using UnityEngine;
using System.Collections;

public class GrenadeThrow : MonoBehaviour {
	public GameObject grenadePrefab;

	public float throwForce = 10f;
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.F)) {
			throwGrenade();
		}
	}

	void throwGrenade() {
		GameObject prefab = GameObject.Instantiate(grenadePrefab);
		prefab.transform.position = Camera.main.transform.position;

		prefab.transform.localPosition += Vector3.forward * 3;

		prefab.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * throwForce);

		prefab.GetComponent<TimerGrenade>().activate();
	}
}
