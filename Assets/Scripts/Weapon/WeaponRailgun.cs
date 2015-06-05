
using UnityEngine;

public class WeaponRailgun : Weapon {
	
	public WeaponRailgun() {
		this.name = "Railgun";
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

	public override void targetHit(RaycastHit target, float damageCustom = -1) {
		base.targetHit(target, damageCustom);

		createLightning(target.transform.position);
	}

	public override void targetMiss() {
		base.targetMiss();

		createLightning(Vector3.forward * 3);
	}

	private void createLightning(Vector3 end) {

	}
}
