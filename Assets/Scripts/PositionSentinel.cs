using UnityEngine;
using System.Collections;

public class PositionSentinel : MonoBehaviour {

	public float range = 5f;

	public bool singleUse = true; // If enabled, this marker will auto-destroyafter the first trigger
	
	public void setup() {
		SphereCollider col = GetComponent<SphereCollider>();
		if(col == null) col = gameObject.AddComponent<SphereCollider>();

		col.isTrigger = true;
		col.radius = range;

		GameController.playerPathfind.target = transform.position;
	}

	public void trigger() {
		GameController.playerPathfind.complete();

		// Send a position event to the quest system 
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.POSITION).setPosition(transform.position));

		if(singleUse)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == Tags.player)
			trigger();
	}
}
