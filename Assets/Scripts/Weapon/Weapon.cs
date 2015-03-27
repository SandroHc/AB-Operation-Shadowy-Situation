using UnityEngine;
using System.Collections;

public abstract class Weapon {
	protected string name = "";

	protected GameObject weaponPrefab;
	public GameObject weaponInstance;
	
	public enum WeaponType { Pistol, AssaultRifle, Shotgun, SniperRifle, Equipment };
	protected WeaponType type;

	protected float damage = 1f;
	protected float range = 50f;
	
	protected float cooldownShoot = .3f;
	protected float cooldownReload = 1f;

	protected float recoil = 5f;

	protected int defaultMagazines = 2;
	protected int defaultMaxAmmunition = 10;

	protected int currentMagazines;
	protected int currentAmmunition;

	protected AudioClip[] sounds = new AudioClip[3];
	protected enum SoundLabel { SHOOT, SHOOT_NO_AMMO, RELOAD };

	public Weapon() {

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

		PlayerPrefs.SetString("weapon_equipped", name);


		// If there is no instance (AND there is a prefab), create one
		if(weaponInstance == null && weaponPrefab != null) {
			// Instantiate the prefab
			weaponInstance = GameObject.Instantiate(weaponPrefab);

			// Setup the new instance
			weaponInstance.gameObject.transform.SetParent(Camera.main.transform);

			// Reset position and rotation (that might have changed with the change of parent)
			weaponInstance.gameObject.transform.localPosition = Vector3.zero;
			weaponInstance.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	public void unequip() {
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

	public void drop() {
		
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

	public GameObject getPrefab() {
		return weaponPrefab;
	}
}
