using UnityEngine;

public class WeaponGrenade : Weapon {
	public float throwForce = 500;

	public WeaponGrenade() : base() {
		this.name = "Grenade";
		//this.icon = GameController.spriteManager.weaponM9;
		this.weaponPrefab = GameController.prefabManager.weapon_grenade;
		this.type = Type.Grenade;

		this.recoil = 5;

		this.cooldownShoot = .5f;

		this.defaultMagazines = 1;
		this.defaultMaxAmmunition = 20;

		// Those values are not used in grenades
		this.range = 0;
		this.damage = 0;

		this.baseCost = 150;

		populateSounds(GameController.audioManager.weapon_grenadeThrow, null, null);
		populateCraftingStatus();
	}

	GameObject grenadeObj;

	public override bool shoot() {
		if(base.shoot()) {
			grenadeObj = Object.Instantiate(GameController.prefabManager.weapon_grenade_object);

			grenadeObj.transform.rotation = Camera.main.transform.rotation;
			grenadeObj.transform.position = Camera.main.transform.position;
			grenadeObj.transform.position += grenadeObj.transform.forward * .5f;
			
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

		Vector3 force = Vector3.forward * throwForce + Vector3.up * throwForce / 4;

		// And launch the grenade!
        body.AddRelativeForce(force);
	}
}
