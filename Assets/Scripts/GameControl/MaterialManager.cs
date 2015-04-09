using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaterialManager : MonoBehaviour {
	public Mesh stage3;
	public Mesh stage2;
	public Mesh stage1;
	public Mesh stageDepleted;

	private static int materialCount; // Total material collected
	public Text uiMaterialCounter;

	private float pickUpTime = 1;
	private bool pickingUp = false;

	void Awake() {
		// Load total materials collected
		materialCount = PlayerPrefs.GetInt("material_count", 0);
		uiMaterialCounter.text = materialCount.ToString();
	}

	public static void increase(int value) {
		if(value <= 0) return;

		materialCount += value;

		GameController.materialManager.saveMaterialCount();
	}

	public static void decrease(int value) {
		if(value <= 0) return;

		materialCount -= value;
		if(materialCount < 0) materialCount = 0;

		GameController.materialManager.saveMaterialCount();
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

		// Generate a random number between 3 and 5
		int materialCount = Random.Range(3, 6);
		
		increase(materialCount);

		// Send a quest progress update with the number of materials picked.
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.MATERIAL_PICKUP).setNumber(materialCount));
	}

	private void saveMaterialCount() {
		uiMaterialCounter.text = materialCount.ToString();

		PlayerPrefs.SetInt("material_count", materialCount);
	}

	public static int getMaterials() {
		return materialCount;
	}
}
