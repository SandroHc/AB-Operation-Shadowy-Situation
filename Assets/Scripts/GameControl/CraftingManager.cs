using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		if(Input.GetKeyDown(InputManager.crafting) && !GameController.isPausedOrFocused()) {
			showCraftingPanel();

			GameController.setFocused(true, false);
			panelCrafting.SetActive(true);
		}
		
		// If the Crafting panel is visible and the Cancel button is pressed, close it.
		if(panelCrafting.activeSelf && Input.GetKeyDown(InputManager.cancel)) {
			GameController.setFocused(false);
			panelCrafting.SetActive(false);
		}
	}

	private void buildCraftingPanel() {
		float width = RectTransformExtensions.GetWidth(craftingContents.GetComponent<RectTransform>()) / 3;

		Weapon[] list = WeaponManager.getWeaponList();

		float currY = 0;

		for(int i = 0; i < list.Length; i++) {
			int mod = i % 3;
			generatePanel(list[i].getName(), new Vector2((width * mod) - 416, -currY), width);

			if(mod == 3)
				currY += 200;
		}
	}

	private void showCraftingPanel() {
		foreach(Transform child in craftingContents) {
			CraftingSlot obj = child.GetComponent<CraftingSlot>();
			if(obj != null) obj.show();
		}
	}

	private GameObject generatePanel(string weaponName, Vector2 pos, float width) {
		GameObject panelObject = Object.Instantiate(weaponPanelPrefab);
		panelObject.name = "weapon_" + weaponName;
		panelObject.transform.SetParent(craftingContents);

		RectTransform rt = panelObject.GetComponent<RectTransform>();
		RectTransformExtensions.SetWidth(rt, width);
		RectTransformExtensions.SetPivotAndAnchors(rt, Vector2.zero);
		RectTransformExtensions.SetLeftTopPosition(rt, pos);

		CraftingSlot craftingScript = panelObject.GetComponent<CraftingSlot>();
		craftingScript.weaponName = weaponName;
		craftingScript.setup();

		Debug.Log(weaponName + ": x=" + pos.x + ", y=" + pos.y + ", width=" + width);

		
		// Maybe configure the button on the CraftingSlot script?
		//Button button = panelObject.GetComponent<Button>();
		//button.onClick.AddListener(() => currentDialogue.selected(id));
		
		return panelObject;
	}
}
