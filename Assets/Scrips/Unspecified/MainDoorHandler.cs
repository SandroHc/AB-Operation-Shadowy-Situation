using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {
	public GameObject doorLeft;
	public GameObject doorRight;

	private Vector3 doorLeftPos;
	private Vector3 doorRightPos;

	private byte playersNearby = 0;

	void Start () {
		doorLeftPos = doorLeft.transform.position;
		doorRightPos = doorRight.transform.position;
	}

	void FixedUpdate() {
		float doorLeftNewPos = Mathf.Lerp(doorLeft.transform.position.x, doorLeftPos.x + (hasPlayersNearby() ? 1.5f/* open */ : 0/* closed */), Time.fixedDeltaTime * 2);
		float doorRightNewPos = Mathf.Lerp(doorRight.transform.position.x, doorRightPos.x + (hasPlayersNearby() ? -1.5f/* open */ : 0/* closed */), Time.fixedDeltaTime * 2);

		doorLeft.transform.position = new Vector3(doorLeftNewPos, doorLeft.transform.position.y, doorLeft.transform.position.z);
		doorRight.transform.position = new Vector3(doorRightNewPos, doorRight.transform.position.y, doorRight.transform.position.z);
	}

	void OnTriggerEnter(Collider hit) {
		playersNearby++;
	}

	void OnTriggerExit(Collider hit) {
		playersNearby--;
	}

	private bool hasPlayersNearby() {
		return playersNearby > 0;
	}
}
