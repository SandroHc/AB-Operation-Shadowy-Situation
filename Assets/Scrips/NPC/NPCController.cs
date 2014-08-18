using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour {
	private string npcName;
	
	public List<Collider> playersNearby;
	
	public float lookSpeed;
	
	public bool toggle = false;
	public float hSliderValue = 0;
	
	private GameController gameController;
	
	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		npcName = "NPC Test";
	}
	
	void Update() {
		if(playersNearby.Count > 0 && Input.GetButtonDown("Interact"))
			gameController.toggleInteracting();
	}
	
	void FixedUpdate() {
		if(gameController.getInteracting())
			playersNearby.ForEach(updateLookingRotation);
	}
	
	private void updateLookingRotation(Collider collider) {
		Vector3 dir = transform.position - collider.transform.position;
		Quaternion look = Quaternion.LookRotation(dir);
		//collider.transform.LookAt(Quaternion.Quaternion.Lerp(collider.transform.rotation, look, lookSpeed * Time.fixedDeltaTime));
		collider.transform.rotation = Quaternion.Lerp(transform.rotation, look, lookSpeed * Time.fixedDeltaTime);
		
		//collider.transform.LookAt(transform.position);
	}
	
	void OnGUI() {
		if(playersNearby.Count > 0) {
			//toggle = GUI.Toggle(new Rect(20, 40, 100, 20), toggle, "A Toggle text");
			//hSliderValue = GUI.HorizontalSlider(new Rect(20, 60, 100, 20), hSliderValue, 0, 10);
			
			string text = gameController.getInteracting() ? npcName : "Press E to talk.";
			GUI.Box(new Rect(0, Screen.height - 75, Screen.width, 75), text);
		}
	}
	
	void OnTriggerEnter(Collider hit) {
		if(!playersNearby.Contains(hit)) playersNearby.Add(hit);
	}
	
	void OnTriggerExit(Collider hit) {
		playersNearby.RemoveAll(collider => collider == hit);
	}
}