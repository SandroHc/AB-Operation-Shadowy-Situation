using UnityEngine;
using System.Collections;

public class WeaponTest : Weapon {

	public WeaponTest() {
		this.name = "TEST";
		this.type = WeaponType.Pistol;

		this.damage = 1f;

		populateSounds(GameController.audioManager.weaponPistolShoot, GameController.audioManager.weaponPistolShootNoAmmo, GameController.audioManager.weaponPistolReload);
	}
}
