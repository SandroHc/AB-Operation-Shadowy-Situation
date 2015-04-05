using UnityEngine;
using System.Collections;

public class PositionSentinel : MonoBehaviour {

	public float range = 5f;
	
	void Awake() {
		SphereCollider col = GetComponent<SphereCollider>();
		if(col == null)
			col = gameObject.AddComponent<SphereCollider>();

		col.isTrigger = true;
	}
	
	// Update is called once per frame
	public void setup() {
		// Setup the collider radius
		GetComponent<SphereCollider>().radius = range;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == Tags.player) {
			// Send a position event to the quest system 
			GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.POSITION).setPosition(transform.position));
		}
	}
}
