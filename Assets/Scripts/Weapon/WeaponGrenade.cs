
using UnityEngine;

public class WeaponGrenade : Weapon {
	public float throwForce = 10f;

	public WeaponGrenade() {
		this.name = "Grenade";
		//this.icon = GameController.spriteManager.weaponM9;
		this.weaponPrefab = GameController.prefabManager.weapon_grenade;
		this.type = Type.Grenade;

		this.damage = 1;
		this.recoil = 5;

		this.cooldownShoot = 3f;

		this.defaultMagazines = 1;
		this.defaultMaxAmmunition = 2;

		this.baseCost = 150;

		populateSounds(GameController.audioManager.weapon_grenadeThrow, null, null);
		populateCraftingStatus();
	}

	GameObject grenadeObj;

	public override bool shoot() {
		if(base.shoot()) {
			grenadeObj = Object.Instantiate(GameController.prefabManager.weapon_grenade);

			grenadeObj.transform.position = Camera.main.transform.position;
			grenadeObj.transform.localPosition += Vector3.forward * 3;
			
			// Initialize the explosion timer
			grenadeObj.GetComponent<GrenadeTimer>().activate();


			throwGrenade();

			return true;
		}
		return false;
	}

	private void throwGrenade() {
		Rigidbody body = grenadeObj.GetComponent<Rigidbody>();

		// Enable physics
		body.isKinematic = false;
		// And launch the grenade!
        body.AddRelativeForce(Vector3.forward * throwForce);
	}
}
