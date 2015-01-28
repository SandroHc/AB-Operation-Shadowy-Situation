using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaterialManager : MonoBehaviour {
	public Mesh stage3;
	public Mesh stage2;
	public Mesh stage1;
	public Mesh stageDepleted;

	private int materialCount; // Total material collected
	public Text uiMaterialCounter;

	private float pickUpTime = 1;
	private bool pickingUp = false;

	// Use this for initialization
	void Awake() {
		// Load total materials collected
		materialCount = PlayerPrefs.GetInt("material_count", 0);
		uiMaterialCounter.text = materialCount.ToString();
	}

	// Update is called once per frame
	void Update () {

	}

	public void increase(int value) {
		if(value == 0) return;

		materialCount += value;

		saveNewValues();
	}

	public void decrease(int value) {
		if(value == 0) return;

		materialCount -= value;
		if(materialCount < 0) materialCount = 0;

		saveNewValues();
	}

	public void pickUp() {
		if(pickingUp) return;

		pickingUp = true;
		GameController.setFocused(true);

		Invoke("finishPickUp", pickUpTime);
	}

	private void finishPickUp() {
		pickingUp = false;
		GameController.setFocused(false);
		
		increase(1);
	}

	private void saveNewValues() {
		uiMaterialCounter.text = materialCount.ToString();

		PlayerPrefs.SetInt("material_count", materialCount);
	}
}
