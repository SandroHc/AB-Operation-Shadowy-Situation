﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Weapon {
	protected string name = "";

	protected GameObject weaponPrefab;
	public GameObject weaponInstance;

	protected Image icon;
	
	public enum Type { Pistol = 0, AssaultRifle = 1, Shotgun = 1, SniperRifle = 1, Knife = 2, Grenade = 3, Equipment = 4 };
	protected Type type;

	public bool isUnlocked;
	public bool isCrafted;
	public bool isEquipped; // Equipped as in the quick-bar (not the current one)

	protected float damage;
	protected float range;

	protected float recoil;
	
	protected float cooldownShoot = .3f;
	protected float cooldownReload = 1f;

	protected int defaultMagazines = 2;
	protected int defaultMaxAmmunition = 10;

	protected int currentMagazines;
	protected int currentAmmunition;

	protected float baseCost;

	protected AudioClip[] sounds = new AudioClip[3];
	protected enum SoundLabel { SHOOT, SHOOT_NO_AMMO, RELOAD };

	public Weapon() {
		// NO-OP
	}

	protected void populateCraftingStatus() {
		isUnlocked = PlayerPrefs.GetInt("weapon_" + name + "_unlocked", 0) == 1;
		isCrafted = PlayerPrefs.GetInt("weapon_" + name + "_crafted", 0) == 1;
		isEquipped = PlayerPrefs.GetInt("weapon_" + name + "_equipped", 0) == 1;

		if(isEquipped)
			WeaponManager.loadWeaponIntoSlot(this);
	}

	public void targetHit(GameObject target, RaycastHit hit) {
		target.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
	}

	public bool shoot() {
		if(WeaponManager.weaponCooldown > 0)
			return false;

		// Only shoot if ammunition is available
		// Else, try to reload the weapon
		if(currentAmmunition > 0) {
			currentAmmunition--;

			WeaponManager.weaponCooldown = cooldownShoot;

			playSound(SoundLabel.SHOOT);
			return true;
		} else {
			if(!reload())
				playSound(SoundLabel.SHOOT_NO_AMMO);
			return false;
		}
	}
	
	public bool reload() {
		// TODO Testing code
		if(Input.GetKey(KeyCode.Return))
			currentAmmunition = defaultMaxAmmunition;

		if(WeaponManager.weaponCooldown > 0)
			return false;

		if(currentMagazines > 1) {
			currentMagazines--;
			currentAmmunition = defaultMaxAmmunition;

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
		currentMagazines = PlayerPrefs.GetInt("weapon_" + name + "_magazines", defaultMagazines);
		currentAmmunition = PlayerPrefs.GetInt("weapon_" + name + "_ammo", defaultMaxAmmunition);

		isEquipped = true;
		PlayerPrefs.SetInt("weapon_" + name + "_equipped", 1);
	}

	public void unequip() {
		isEquipped = false;
		// Delete the preference to, 1) unclutter the registry; 2) the default value is false
		PlayerPrefs.DeleteKey("weapon_" + name + "_equipped");

		// Check if the save is redundant or not.
		// Store the avaiable magazines
		if(currentMagazines != defaultMagazines)
			PlayerPrefs.SetInt("weapon_" + name + "_magazines", currentMagazines);
		else
			PlayerPrefs.DeleteKey("weapon_" + name + "_magazines");

		// Store the available ammunition
		if(currentAmmunition != defaultMaxAmmunition)
			PlayerPrefs.SetInt("weapon_" + name + "_magazines", currentAmmunition);
		else
			PlayerPrefs.DeleteKey("weapon_" + name + "_ammo");
	}

	public void show() {
		// If there is no instance (AND there is a prefab), create one
		if(weaponInstance == null && weaponPrefab != null) {
			// Instantiate the prefab
			weaponInstance = GameObject.Instantiate(weaponPrefab);
			
			// Setup the new instance
			weaponInstance.transform.SetParent(Camera.main.transform);
			
			// Reset position and rotation (that might have changed with the change of parent)
			weaponInstance.transform.localPosition = Vector3.zero;
			weaponInstance.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}

		weaponInstance.SetActive(true);
	}

	public void hide() {
		if(weaponInstance != null)
			weaponInstance.SetActive(false);
	}

	private void playSound(SoundLabel label) {
		AudioClip clip = sounds[(int) label];
		if(clip != null)
			WeaponManager.audioSource.PlayOneShot(clip);
	}

	public void populateSounds(AudioClip shoot = null, AudioClip shootNoAmmo = null, AudioClip reload = null) {
		sounds[(int) SoundLabel.SHOOT] = shoot;
		sounds[(int) SoundLabel.SHOOT_NO_AMMO] = shootNoAmmo;
		sounds[(int) SoundLabel.RELOAD] = reload;
	} 

	public float getDamage() {
		return damage;
	}

	public float getRange() {
		return range;
	}

	public float getRecoil() {
		return recoil;
	}

	public string getName() {
		return name;
	}

	public int getMagazines() {
		return currentMagazines;
	}

	public int getAmmunition() {
		return currentAmmunition;
	}

	public int getAmmunitionPerMagazine() {
		return defaultMaxAmmunition;
	}

	public float getShootingCooldown() {
		return cooldownShoot;
	}

	public GameObject getPrefab() {
		return weaponPrefab;
	}

	public Image getIcon() {
		return icon;
	}

	public Type getWeaponType() {
		return type;
	}

	public int getSlot() {
		return (int) type;
	}

	public float getCost() {
		if(isEquipped) // Ammo = 5%
			return baseCost * .05f;
		else if(isCrafted) // Rebuild = 15%
			return baseCost * .15f;
		else if(isUnlocked) // First build = 100%
			return baseCost * 1;
		else // Can't build weapon
			return -1;
	}
}
