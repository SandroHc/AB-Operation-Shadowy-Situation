using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
	public GameObject panelCrafting;
	public Transform craftingContents;

	public RectTransform panelPistols;
	public RectTransform panelAssaultRifles;
	public RectTransform panelShotguns;
	public RectTransform panelSniperRifles;
	public RectTransform panelOthers;

	private RectTransform currentPanel;

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
		currentPanel = panelPistols;

		buildCraftingPanel();
	}

	void LateUpdate() {
		// If the Crafting button is pressed, show the panel!
		if(InputManager.getKeyDown("crafting") && !GameController.isPausedOrFocused()) {
			// Fire event to all sub-panels
			triggerShowEvent();

			GameController.setFocused(true, false);
			panelCrafting.SetActive(true);
		}
		
		// If the Crafting panel is visible and the Cancel button is pressed, close it.
		if(panelCrafting.activeSelf && InputManager.getKeyDown("cancel")) {
			GameController.setFocused(false);
			panelCrafting.SetActive(false);
		}
	}

	private void triggerShowEvent() {
		foreach(Transform child in currentPanel) {
			CraftingSlot obj = child.GetComponent<CraftingSlot>();
			if(obj != null) obj.show();
		}
	}

	private void buildCraftingPanel() {
		Weapon[] list = WeaponManager.getAllWeapons();

		List<Weapon> pistols = new List<Weapon>();
		List<Weapon> assault = new List<Weapon>();
		List<Weapon> shotguns = new List<Weapon>();
		List<Weapon> snipers = new List<Weapon>();
		List<Weapon> others = new List<Weapon>();


		for(int i = 0; i < list.Length; i++) {
			switch(list[i].type) {
				case Weapon.Type.Pistol:		pistols.Add(list[i]); break;
				case Weapon.Type.AssaultRifle:	assault.Add(list[i]); break;
				case Weapon.Type.Shotgun:		shotguns.Add(list[i]); break;
				case Weapon.Type.SniperRifle:	snipers.Add(list[i]); break;
				default:						others.Add(list[i]); break;
			}
			
		}

		int slotsPerRow = (int) panelPistols.rect.width / 300;

		buildPanel(panelPistols, pistols, slotsPerRow);
		buildPanel(panelAssaultRifles, assault, slotsPerRow);
		buildPanel(panelShotguns, shotguns, slotsPerRow);
		buildPanel(panelSniperRifles, snipers, slotsPerRow);
		buildPanel(panelOthers, others, slotsPerRow);
    }

	private void buildPanel(RectTransform rt, List<Weapon> list, int row) {
		float width = rt.rect.width;
		float slotWidth = width / row;

		float y = 0;

		for(int i = 0; i < list.Count; i++) {
			buildSlot(list[i], new Vector2(-(width / 2) + slotWidth * (i % row), -y), slotWidth, rt);

			// Every 3 slots, go to the next row
			if((i + 1) % row == 0) y += 200;
		}

		y += 200;
		//RectTransformExtensions.SetHeight(rt, y);
    }

	private GameObject buildSlot(Weapon weapon, Vector2 pos, float width, RectTransform parent) {
		GameObject panelObject = Instantiate(weaponPanelPrefab);
		panelObject.name = "weapon_" + weapon.name;
		panelObject.transform.SetParent(parent);

		RectTransform rt = panelObject.GetComponent<RectTransform>();
		RectTransformExtensions.SetWidth(rt, width);
		RectTransformExtensions.SetPivotAndAnchors(rt, new Vector2(0, 1));

		pos.y += 250; // I don't know why, but I need to apply this offset. So the RectTransform starts at y = 0
		RectTransformExtensions.SetPositionOfPivot(rt, pos);

		CraftingSlot craftingScript = panelObject.GetComponent<CraftingSlot>();
		craftingScript.setup(weapon);

		//Debug.Log(weaponName + ": x=" + pos.x + ", y=" + pos.y + ", width=" + width);
		
		return panelObject;
	}

	public void switchPanel(int panel) {
		RectTransform newPanel = null;

		switch(panel) {
			case 1: newPanel = panelPistols; break;
			case 2: newPanel = panelAssaultRifles; break;
			case 3: newPanel = panelShotguns; break;
			case 4: newPanel = panelSniperRifles; break;
			case 5: newPanel = panelOthers; break;
		}

		if(newPanel == null || newPanel == currentPanel) return;

		// Hide the old panel and show the new one
		currentPanel.gameObject.SetActive(false);
		newPanel.gameObject.SetActive(true);

		currentPanel = newPanel;

		// Fire the show() event on all crafting slots
		triggerShowEvent();
    }
}
