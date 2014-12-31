using UnityEngine;
using System.Collections;

public class InteractPlayer : MonoBehaviour {
	public float range = 50f;

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		
		// When we left click and our raycast hits something
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range)) {
			Interaction other = (Interaction) hit.transform.gameObject.GetComponent(typeof(Interaction));

			if(other != null) {
				text = other.type + " available. Press E to interact with " + other.name;

				if(Input.GetButtonDown("Interact"))
					other.doAction(this.gameObject);
			} else
				text = "";
		}
	}

	private string text = "";

	void OnGUI() {
		if(!text.Equals(""))
			GUI.Box(new Rect(Screen.width / 6, Screen.height - 50, Screen.width / 1.5f, 50), text);
	}
}
