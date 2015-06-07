﻿using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon {
	public string name { get; protected set; }

	protected GameObject weaponPrefab;
	public GameObject weaponInstance;
	public Transform gunMuzzle;

	public Sprite icon { get; protected set; }
	
	public enum Type { Pistol, AssaultRifle, Shotgun, SniperRifle, Knife, Grenade, Equipment };
	public Type type { get; protected set; }

	public bool isUnlocked { get; protected set; }
	public bool isCrafted { get; protected set; }
	public bool isEquipped { get; protected set; } // Equipped as in the quick-bar (not the current one)

	public float damage { get; protected set; }
	public float range { get; protected set; }

	public float recoil { get; protected set; }
	
	protected float cooldownShoot;
	protected float cooldownReload = 1f;

	protected int defaultMagazines;
	protected int defaultMaxAmmunition;

	public int magazines { get; protected set; } // Current magazines
	public int ammunition { get; protected set; } // Current ammunition

	private bool _unlimitedAmmo = false;
	public bool unlimitedAmmo {
		get { return _unlimitedAmmo; }
		protected set { _unlimitedAmmo = value; }
	}

	protected float baseCost;
	public float cost {
		get {
			if(isEquipped) // Ammo = 5%
				return baseCost * .05f;
			else if(isCrafted) // Rebuild = 15%
				return baseCost * .15f;
			else if(isUnlocked) // First build = 100%
				return baseCost * 1;
			else // Can't build weapon
				return -1;
		}
		protected set { baseCost = value; }
	}

	protected AudioClip[] sounds = new AudioClip[3];
	protected enum SoundLabel { SHOOT, SHOOT_NO_AMMO, RELOAD };

    public bool spawnBulletDecal { get; protected set; }

	public Weapon() {
		name = "";

		spawnBulletDecal = true;
    }

	protected void populateCraftingStatus() {
		isUnlocked = PlayerPrefs.GetInt("weapon_" + name + "_unlocked", 0) == 1;
		isCrafted = PlayerPrefs.GetInt("weapon_" + name + "_crafted", 0) == 1;
		isEquipped = PlayerPrefs.GetInt("weapon_" + name + "_equipped", 0) == 1;

		if(isEquipped)
			WeaponManager.loadWeaponIntoSlot(this);
	}

	public virtual bool shoot() {
		if(WeaponManager.weaponCooldown > 0)
			return false;

		// Only shoot if ammunition is available
		// Else, try to reload the weapon
		if(ammunition > 0 || unlimitedAmmo) {
			// No need to save current ammo count if the weapon has unlimited ammo
			if(!unlimitedAmmo) {
				ammunition--;
				saveAmmoStatus();
			}

			WeaponManager.weaponCooldown = cooldownShoot;

			playSound(SoundLabel.SHOOT);
			return true;
		} else {
			if(!reload()) {
				WeaponManager.weaponCooldown = cooldownShoot * 3;

				playSound(SoundLabel.SHOOT_NO_AMMO);
			}
			return false;
		}
	}

	public virtual void targetHit(RaycastHit target, float damageCustom = -1) {
		target.transform.gameObject.SendMessage("takeDamage", damageCustom == -1 ? damage : damageCustom, SendMessageOptions.DontRequireReceiver);
	}

	public virtual void targetMiss() { }

	public virtual bool reload() {
		if(WeaponManager.weaponCooldown > 0)
			return false;

		// Only reload if there are magazines AND if the current one is not full
		if(magazines > 1 && ammunition != defaultMaxAmmunition) {
			magazines--;
			ammunition = defaultMaxAmmunition;

			saveAmmoStatus();

			WeaponManager.weaponCooldown = cooldownReload;

			playSound(SoundLabel.RELOAD);
			return true;
		} else {
			return false;
		}
	}

	public void aimEnter() {

	}

	public void aimExit() {

	}

	public void equip() {
		// Populate the available magazines & ammunition
		magazines = PlayerPrefs.GetInt("weapon_" + name + "_magazines", defaultMagazines);
		ammunition = PlayerPrefs.GetInt("weapon_" + name + "_ammo", defaultMaxAmmunition);

		isEquipped = true;
		PlayerPrefs.SetInt("weapon_" + name + "_equipped", 1);

		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.ITEM_EQUIP).setStr(name));
	}

	public void unequip() {
		isEquipped = false;
		// Delete the preference to, 1) unclutter the registry; 2) the default value is false
		PlayerPrefs.DeleteKey("weapon_" + name + "_equipped");

		saveAmmoStatus();
	}

	public void show() {
		// If there is no instance (AND there is a prefab), create one
		if(weaponInstance == null && weaponPrefab != null) {
			// Instantiate the prefab
			weaponInstance = Object.Instantiate(weaponPrefab);
			
			// Setup the new instance
			weaponInstance.transform.SetParent(Camera.main.transform);
			
			// Reset position and rotation (that might have changed with the change of parent)
			weaponInstance.transform.localPosition = Vector3.zero;
			weaponInstance.transform.localRotation = Quaternion.Euler(Vector3.zero);

			// Get the muzzle, if available. Used to create the muzzle effects
			gunMuzzle = weaponInstance.transform.Find("offset/muzzle");

			// If the weapon has a RigidBody, disable it's physics
			Rigidbody rb = weaponInstance.GetComponent<Rigidbody>();
            if(rb != null) rb.isKinematic = true;
		}

		// Verify if the the weapon instance was loaded
		if(weaponInstance != null)
			weaponInstance.SetActive(true);
	}

	public void hide() {
		if(weaponInstance != null)
			weaponInstance.SetActive(false);
	}

	public bool refillAmmo() {
		Debug.Log(name + ": Refilling ammo");

		if(isEquipped && ammunition < defaultMaxAmmunition) {
			int cost = Mathf.RoundToInt(this.cost);
			if(MaterialManager.getMaterials() > cost) {
				MaterialManager.decrease(cost);

				magazines = defaultMagazines;
				ammunition = defaultMaxAmmunition;

				saveAmmoStatus();
				
				return true;
			} else
				return false;
		} else
			return true; // Already equipped
	}

	public bool unlock() {
		Debug.Log(name + ": Unlocked");
		
		isUnlocked = true;
		PlayerPrefs.SetInt("weapon_" + name + "_unlocked", 1);

		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.ITEM_UNLOCK).setStr(name));

		return true;
	}

	public bool craft() {
		Debug.Log(name + ": Crafting");

		if(!isCrafted) {
			int cost = Mathf.RoundToInt(this.cost);
			if(MaterialManager.getMaterials() > cost) {
				MaterialManager.decrease(cost);

				isCrafted = true;
				PlayerPrefs.SetInt("weapon_" + name + "_crafted", 1);

				GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.ITEM_CRAFT).setStr(name));

				return true;
			} else
				return false;
		} else
			return true; // Already crafted
	}

	protected void playSound(SoundLabel label) {
		AudioClip clip = sounds[(int) label];
		if(clip != null)
			WeaponManager.audioSource.PlayOneShot(clip);
	}

	public void populateSounds(AudioClip shoot = null, AudioClip shootNoAmmo = null, AudioClip reload = null) {
		sounds[(int) SoundLabel.SHOOT] = shoot;
		sounds[(int) SoundLabel.SHOOT_NO_AMMO] = shootNoAmmo;
		sounds[(int) SoundLabel.RELOAD] = reload;
	} 

	public int getAmmunitionPerMagazine() {
		return defaultMaxAmmunition;
	}

	public float getShootingCooldown() {
		return cooldownShoot;
	}

	public int getSlot() {
		switch(type) {
			case Type.Pistol:		return 0;
			case Type.AssaultRifle:	return 1;
			case Type.Shotgun:		return 1;
			case Type.SniperRifle:	return 1;
			case Type.Knife:		return 2;
			case Type.Grenade:		return 3;
			//case Type.Equipment:	return 4;
			default:				return 4;
		}
	}

	private void saveAmmoStatus() {
		// Check if the save is redundant or not.
		// Store the avaiable magazines
		if(magazines != defaultMagazines)
			PlayerPrefs.SetInt("weapon_" + name + "_magazines", magazines);
		else
			PlayerPrefs.DeleteKey("weapon_" + name + "_magazines");
		
		// Store the available ammunition
		if(ammunition != defaultMaxAmmunition)
			PlayerPrefs.SetInt("weapon_" + name + "_ammo", ammunition);
		else
			PlayerPrefs.DeleteKey("weapon_" + name + "_ammo");
	}
}
