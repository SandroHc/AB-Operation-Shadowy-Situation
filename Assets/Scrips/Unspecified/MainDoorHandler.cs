using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {
	public GameObject doorLeft;
	public GameObject doorRight;

	private Vector3 doorLeftPos;
	private Vector3 doorRightPos;

	private bool state = false; // true=open; false=closed

	void Start () {
		doorLeftPos = doorLeft.transform.position;
		doorRightPos = doorRight.transform.position;
	}

	void FixedUpdate() {
		float left = Mathf.Lerp(doorLeft.transform.position.x, doorLeftPos.x + (state ? 1.5f/* open */ : 0/* closed */), Time.fixedDeltaTime * 2);
		float right = Mathf.Lerp(doorRight.transform.position.x, doorRightPos.x + (state ? -1.5f/* open */ : 0/* closed */), Time.fixedDeltaTime * 2);

		doorLeft.transform.position = new Vector3(left, doorLeft.transform.position.y, doorLeft.transform.position.z);
		doorRight.transform.position = new Vector3(right, doorRight.transform.position.y, doorRight.transform.position.z);
	}

	void OnTriggerEnter(Collider hit) {
		state = true;
	}

	void OnTriggerExit(Collider hit) {
		state = false;
	}
}
