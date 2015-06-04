using UnityEngine;

public class CraftingManager : MonoBehaviour {
	public GameObject panelCrafting;
	public Transform craftingContents;

	public GameObject weaponPanelPrefab;

	/**
	 * To craft a weapon, one need material & blueprints.
	 * The blueprints are recovered as quest rewards (better weapons), or in stores (cheaper weapons).
	 * 
	 * There will be tabs according to the weapon type.
	 * 	- Pistols
	 * 	- Assault Rifles
	 *  - Shotguns
	 * 	- Sniper Rifles
	 *  - Equipment
	 * 
	 * Each weapon will have the following stats shown: (each displayed as a bar)
	 * 	- Damage
	 * 	- Range
	 *  - Rate of fire
	 */

	void Start() {
		buildCraftingPanel();
	}

	void LateUpdate() {
		// If the Crafting button is pressed, show the panel!
		if(InputManager.getKeyDown("crafting") && !GameController.isPausedOrFocused()) {
			// Fire event to all sub-panels
			showCraftingPanel();

			GameController.setFocused(true, false);
			panelCrafting.SetActive(true);
		}
		
		// If the Crafting panel is visible and the Cancel button is pressed, close it.
		if(panelCrafting.activeSelf && InputManager.getKeyDown("cancel")) {
			GameController.setFocused(false);
			panelCrafting.SetActive(false);
		}
	}

	private void buildCraftingPanel() {
		Weapon[] list = WeaponManager.getAllWeapons();

		int slotsPerRow = 3;

		float width = RectTransformExtensions.GetWidth(craftingContents.GetComponent<RectTransform>());
		float panelWidth = width / slotsPerRow;

		float posY = 0;

		for(int i = 0; i < list.Length; i++) {
			int mod = i % slotsPerRow;
			generatePanel(list[i].name, new Vector2(-(width/2) + panelWidth * mod, -posY), panelWidth);

			// Every 3 slots, create a new row
			if(mod == slotsPerRow) posY += 200;
		}
	}

	private void showCraftingPanel() {
		foreach(Transform child in craftingContents) {
			CraftingSlot obj = child.GetComponent<CraftingSlot>();
			if(obj != null) obj.show();
		}
	}

	private GameObject generatePanel(string weaponName, Vector2 pos, float width) {
		GameObject panelObject = Instantiate(weaponPanelPrefab);
		panelObject.name = "weapon_" + weaponName;
		panelObject.transform.SetParent(craftingContents);

		RectTransform rt = panelObject.GetComponent<RectTransform>();
		RectTransformExtensions.SetWidth(rt, width);
		RectTransformExtensions.SetPivotAndAnchors(rt, new Vector2(0, 1));
		RectTransformExtensions.SetPositionOfPivot(rt, pos);

		CraftingSlot craftingScript = panelObject.GetComponent<CraftingSlot>();
		craftingScript.weaponName = weaponName;
		craftingScript.setup();

		//Debug.Log(weaponName + ": x=" + pos.x + ", y=" + pos.y + ", width=" + width);
		
		return panelObject;
	}
}
