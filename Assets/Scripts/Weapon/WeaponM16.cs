using UnityEngine;
using System.Collections;

public class WeaponM16 : Weapon {
	
	public WeaponM16() {
		this.name = "M16";
		this.icon = GameController.spriteManager.weaponM16;
		this.weaponPrefab = GameController.prefabManager.weaponM16;
		this.type = Type.AssaultRifle;
		
		this.damage = 5;
		this.range = 100;
		this.recoil = 10;

		this.baseCost = 1000;
		
		populateSounds(GameController.audioManager.weaponPistolShoot, GameController.audioManager.weaponPistolShootNoAmmo, GameController.audioManager.weaponPistolReload);
		populateCraftingStatus();
	}
}
