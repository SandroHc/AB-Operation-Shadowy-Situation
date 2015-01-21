using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaterialManager : MonoBehaviour {
	public MeshRenderer rubbishStage1;
	public MeshRenderer rubbishStage2;

	private GameController gameController;

	private int materialCount; // Total material collected
	public Text uiMaterialCounter;

	// Use this for initialization
	void Awake() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();

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

	private void saveNewValues() {
		uiMaterialCounter.text = materialCount.ToString();

		PlayerPrefs.SetInt("material_count", materialCount);
	}
}
