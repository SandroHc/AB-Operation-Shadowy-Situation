using UnityEngine;
using System.Collections;

public class InteractDistance : MonoBehaviour {
	public float range = 5f;
	
	void Awake() {
		SphereCollider col = gameObject.GetComponent<SphereCollider>();
		if(col == null)
			col = gameObject.AddComponent<SphereCollider>();

		col.radius = range / gameObject.transform.localScale.x;
		col.isTrigger = true;
	}

	void OnTriggerEnter(Collider other) {
		if(Tags.player.Equals(other.tag)) {
			InteractControl obj = gameObject.GetComponent<InteractControl>();
			if(obj != null) {
				// Enable the InputControl script
				obj.enabled = true;
				// Ligthen the UI color
				obj.background.color = Color.white;
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(Tags.player.Equals(other.tag)) {
			InteractControl obj = gameObject.GetComponent<InteractControl>();
			if(obj != null) {
				// Disable the InputControl script
				obj.enabled = false;
				// Darken the UI color
				obj.background.color = Color.grey;
			}
		}
	}
}
