using UnityEngine;

public class WeaponRailgun : Weapon {
	
	public WeaponRailgun() : base() {
		this.name = "Railgun";
		this.icon = GameController.spriteManager.weaponRailgun;
		this.weaponPrefab = GameController.prefabManager.weapon_Railgun;
		this.type = Type.AssaultRifle;
		
		this.damage = 1;
		this.range = 40;
		this.recoil = 1;

		this.cooldownShoot = .01f;

		this.spawnBulletDecal = false;
		this.unlimitedAmmo = true;

		this.baseCost = 10000;
		
		//populateSounds(null);
		populateCraftingStatus();
	}

	private LightningBolt bolt;
	private ParticleEmitter emitter;

	private GameObject lightningPoint;

	private void initLightningObj() {
		if(gunMuzzle == null)
			return;

		GameObject obj = PrefabManager.createLightningEmitter();
		// Set the parent to the gun muzzle
		obj.transform.SetParent(gunMuzzle, false);
		// And reset the local position back to zero
		obj.transform.localPosition = Vector3.zero;

		bolt = obj.GetComponent<LightningBolt>();
		emitter = obj.GetComponent<ParticleEmitter>();
	}

	public override void targetHit(RaycastHit target, float damageCustom = -1) {
		base.targetHit(target, damageCustom);

		// Create a empty GameObject
		if(lightningPoint == null)
			lightningPoint = new GameObject("lightningPoint");

		// If the target point differs from the one of the object, update the object position
		if(target.point != lightningPoint.transform.position) {
			lightningPoint.transform.SetParent(target.transform);
			lightningPoint.transform.position = target.point;
		}

		startLightning(lightningPoint.transform);
		//Invoke("stopLightning", .3f);
	}

	public override void targetMiss() {
		base.targetMiss();

		// Create a empty GameObject
		if(lightningPoint == null)
			lightningPoint = new GameObject("lightningPoint");

		// If the target point differs from the one of the object, update the object position
		lightningPoint.transform.SetParent(GameController.playerController.transform);
		lightningPoint.transform.localPosition = Vector3.forward * 3 + Vector3.up * 1.5f;

		startLightning(lightningPoint.transform);
	}

	private void startLightning(Transform target) {
		if(gunMuzzle == null)
			return;
		
		if(bolt == null)
			initLightningObj();

		bolt.target = target;
		emitter.enabled = true;
	}

	private void stopLightning() {
		if(emitter != null)
			emitter.enabled = false;
	}
}
