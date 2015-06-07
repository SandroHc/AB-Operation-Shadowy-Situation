using UnityEngine;
using System.Collections;

public class WeaponM9 : Weapon {

	public WeaponM9() : base() {
		this.name = "M9";
		//this.icon = GameController.spriteManager.weaponM9;
		this.weaponPrefab = GameController.prefabManager.weapon_M9;
		this.type = Type.Pistol;

		this.damage = 1;
		this.range = 50;
		this.recoil = 5;

		this.cooldownShoot = .5f;

		this.defaultMagazines = 2;
		this.defaultMaxAmmunition = 10;

		this.baseCost = 160;

		populateSounds(GameController.audioManager.weapon_pistolShoot, GameController.audioManager.weapon_pistolShootNoAmmo, GameController.audioManager.weapon_pistolReload);
		populateCraftingStatus();
	}
}
