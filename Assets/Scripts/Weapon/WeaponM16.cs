using UnityEngine;
using System.Collections;

public class WeaponM16 : Weapon {
	
	public WeaponM16() {
		this.name = "M16";
		this.icon = GameController.spriteManager.weaponM16;
		this.weaponPrefab = GameController.prefabManager.weapon_M16;
		this.type = Type.AssaultRifle;
		
		this.damage = 1;
		this.range = 100;
		this.recoil = 10;

		this.cooldownShoot = .05f;

		this.defaultMagazines = 4;
		this.defaultMaxAmmunition = 30;

		this.baseCost = 1000;
		
		populateSounds(GameController.audioManager.weapon_pistolShoot, GameController.audioManager.weapon_pistolShootNoAmmo, GameController.audioManager.weapon_pistolReload);
		populateCraftingStatus();
	}
}
