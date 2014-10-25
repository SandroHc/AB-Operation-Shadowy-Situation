using UnityEngine;
using System.Collections;

public class MainDoorHandlerCopy : MonoBehaviour {
	public GameObject doorLeft;
	public GameObject doorRight;
	
	private Vector3 doorLeftPos;
	private Vector3 doorRightPos;

	public float offset = 1.1f;
	
	private byte playersNearby = 0;

	void Start () {
		doorLeftPos = doorLeft.transform.position;
		doorRightPos = doorRight.transform.position;
	}

	void FixedUpdate() {
		Vector3 temp;

		// Left door
		temp = doorLeft.transform.position;
		temp.x = Mathf.Lerp(temp.x, doorLeftPos.x + (hasPlayersNearby() ? offset/* open */ : 0/* closed */), Time.fixedDeltaTime * 2);
		doorLeft.transform.position = temp;

		// Right door
		temp = doorRight.transform.position;
		temp.x = Mathf.Lerp(temp.x, doorRightPos.x + (hasPlayersNearby() ? -offset/* open */ : 0/* closed */), Time.fixedDeltaTime * 2);
		doorRight.transform.position = temp;
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
