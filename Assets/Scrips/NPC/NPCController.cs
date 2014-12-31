using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour {
	private GameController gameController;
	private string name;
	
	public List<Collider> playersNearby;
	
	public float lookSpeed;
	
	public bool toggle = false;
	public float hSliderValue = 0;

	public GameObject textMeshPrefab;
	private TextMesh textMesh;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		name = "NPC Test";

		// Create a clone of the TextMesh prefab
		GameObject obj = GameObject.Instantiate(textMeshPrefab) as GameObject;
		// Set the prefab as a child of this GO
		obj.transform.parent = gameObject.transform;

		// Setup the TextMesh component
		textMesh = obj.GetComponent<TextMesh>(); 
		textMesh.text = name;
		textMesh.characterSize = 0.025f;
		textMesh.alignment = TextAlignment.Center;
		textMesh.anchor = TextAnchor.MiddleCenter;

		// Setup the clone position
		obj.transform.localPosition = new Vector3(0, 1.3f, 0);
	}

	void Update() {
		if(gameController.getInteracting())
			playersNearby.ForEach(updateLookingRotation);


		if(Camera.main != null) // When in cutscenes, the main camera is disabled
			if(Camera.main.transform.hasChanged)
				textMesh.transform.forward = Camera.main.transform.forward; // Keeps the TextMesh facing the player
	}

	private void updateLookingRotation(Collider collider) {
		Quaternion targetRotation = Quaternion.LookRotation(transform.position - collider.transform.position);
		// Smoothly rotate towards the target point
		collider.transform.rotation = Quaternion.Slerp(collider.transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
	}
}