using UnityEngine;
using System.Collections;

public class InteractDistance : MonoBehaviour {
	public bool active = true;
	public bool cooldown = false;

	public float range = 5f;
	private bool inRange = false;
	
	void Awake() {
		SphereCollider col = gameObject.GetComponent<SphereCollider>();
		if(col == null)
			col = gameObject.AddComponent<SphereCollider>();

		col.radius = 10 / gameObject.transform.localScale.x;
		col.isTrigger = true;


		control = gameObject.GetComponent<InteractControl>();
	}

	void Update() {
		if(inRange && !cooldown)
			enable();
		else
			disable();
	}

	void OnTriggerStay(Collider other) {
		if(!active) return;

		if(Tags.player.Equals(other.tag)) {
			inRange = Vector3.Distance(other.transform.position, transform.position) <= range;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(Tags.player.Equals(other.tag)) {
			// Enable the InputControl script
			control.active = true;

			// Show the canvas
			control.background.GetComponentInParent<Canvas>().enabled = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(Tags.player.Equals(other.tag)) {
			// Disable the InputControl script
			control.active = false;

			// Hide the canvas
			control.background.GetComponentInParent<Canvas>().enabled = false;
		}
	}

	InteractControl control;

	private void enable() {
		if(control != null) {
			// Enable the InputControl script
			control.active = true;
			// Ligthen the UI color
			control.background.color = Color.white;
			control.fill.color = Color.white;
		}
	}

	private void disable() {
		if(control != null) {
			// Disable the InputControl script
			control.active = false;
			// Darken the UI color
			control.background.color = Color.grey;
			control.fill.color = Color.grey;
		}
	}
}
