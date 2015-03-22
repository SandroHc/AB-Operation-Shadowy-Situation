using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingManager : MonoBehaviour {
	public GameObject panelCrafting;

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
	 */

	void LateUpdate() {
		// If the Crafting button is pressed, show the panel!
		if(Input.GetKeyDown(InputManager.crafting) && !GameController.isPausedOrFocused()) {
			GameController.setFocused(true, false);
			panelCrafting.SetActive(true);
		}
		
		// If the Crafting panel is visible and the Cancel button is pressed, close it.
		if(panelCrafting.activeSelf && Input.GetKeyDown(InputManager.cancel)) {
			GameController.setFocused(false);
			panelCrafting.SetActive(false);
		}
	}
}
