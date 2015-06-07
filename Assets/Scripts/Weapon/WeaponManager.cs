using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class WeaponManager : MonoBehaviour {
	private static WeaponManager INSTANCE;

	public static Dictionary<string, Weapon> weapons;
	public static Weapon[] weaponSlots = new Weapon[5];
	public enum SLOT { SIDE = 0, MAIN = 1, KNIFE = 2, GRENADE = 3, EQUIPMENT = 4 };

	public static int currentSlot;


	public static float weaponCooldown = 0;

	// Variables used when aiming
	private new Camera camera;

	private float defaultFieldOfView;
	private float currentFieldOfView;
	private float speedChangeFOV = 0.4f; // 40%/s

	private bool isAimingLast; // Used to fire aim enter/exit events

	private RaycastShoot raycast;
	public static AudioSource audioSource;

	public Text weaponInfo;

	public CameraController cameraController;

	public static float maxDamage;
	public static float maxFireRate;
	public static float maxRange;

	void Awake() {
		INSTANCE = this;

		// Get the saved selected slot
		// Needed to be loaded BEFORE the weapons. Because when a weapon is created, if it's equipped, it triggers loadWeaponIntoSlot() and if is the current slot, we show it.
		currentSlot = PlayerPrefs.GetInt("weapon_slot", (int)SLOT.MAIN);

		// Do this here because Awake() is called BEFORE Start(). So, if a script tried to get a weapon instance in the Awake() event... he whould receive a NULL object.
		if(weapons == null) {
			weapons = new Dictionary<string, Weapon>();

			// Get all subclasses of Weapon, and register them
			foreach(Type type in typeof(Weapon).Assembly.GetTypes()) {
				if(type.IsSubclassOf(typeof(Weapon)))
					register(Activator.CreateInstance(type) as Weapon);
			}
		}

		raycast = cameraController.GetComponent<RaycastShoot>();
		audioSource = raycast.GetComponent<AudioSource>();

		camera = cameraController.GetComponent<Camera>();
		defaultFieldOfView = camera.fieldOfView;
		currentFieldOfView = defaultFieldOfView;

		isAimingLast = false; // Not aiming by default

		// Update the weapon selection UI slot index
		WeaponSelection.index = currentSlot;
	}

	void Update() {
		// We don't want to be able to shoot in the Pause Menu, don't we?
		if(GameController.isPausedOrFocused())
			return;

		// Decrease the cooldown only during the gameplay. Else, someone could exploit this to reset the cooldown in the Pause menu
		if(weaponCooldown > 0)
			weaponCooldown -= Time.deltaTime;

		// Check Aim controls
		handleAim();

		// Check Reload controls
		handleReload();

		// Check Fire controls
		handleShoot();
	}

	private void handleAim() {
		bool isAiming = Input.GetKey(InputManager.getKey("fire2"));
		currentFieldOfView = isAiming ? defaultFieldOfView * .6f /* shrink the FOV by 60% */ : defaultFieldOfView;
		
		if(isAiming != isAimingLast) {
			Weapon weapon = getCurrentWeapon();

			if(isAiming) {
				if(weapon != null) weapon.aimEnter();
				cameraController.aimEnter();
			} else {
				if(weapon != null) weapon.aimExit();
				cameraController.aimExit();
			}
		}
		
		isAimingLast = isAiming;
		
		// Update the camera FOV
		camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, currentFieldOfView, ref speedChangeFOV, .3f);
	}

	private void handleReload() {
		if(Input.GetKeyUp(InputManager.getKey("reload"))) { // Only reload if the magazine is not full
			Weapon weapon = getCurrentWeapon();
			if(weapon != null) weapon.reload();

			updateWeaponText();
		}
	}

	private void handleShoot() {
		if(Input.GetKey(InputManager.getKey("fire1"))) {
			Weapon weapon = getCurrentWeapon();
			if(weapon != null && weapon.shoot()) {
				cameraController.GetComponentInParent<RecoilHandler>().recoil(weapon.recoil);

				RaycastHit hit = raycast.raycast(weapon.range);
				if(hit.transform != null)
					weapon.targetHit(hit);
				else
					weapon.targetMiss();

				updateWeaponText();
			}
		}
	}

	public static void switchWeapon(Weapon weapon) {
		if(weapon == null)
			return;

		// Set the weapon to the respective slot
		loadWeaponIntoSlot(weapon, true);
	}

	public static void switchSlot(int slot) {
		if(slot < 0 || slot >= weaponSlots.Length)
			return;
		if(slot == currentSlot)
			return;

		// Hide the old weapon model
		Weapon weapon = getCurrentWeapon();
		if(weapon != null) weapon.hide();

		// Set the new selected slot
		currentSlot = slot;
		PlayerPrefs.SetInt("weapon_slot", slot);

		// Show the new weapon model
		weapon = getCurrentWeapon();
		if(weapon != null) weapon.show();

		Debug.Log("New slot selected, " + ((SLOT) currentSlot));

		INSTANCE.updateWeaponText();
	}

	public static void loadWeaponIntoSlot(Weapon weapon, bool overrideExistingWeapon = false) {
		if(weapon == null) {
			Debug.Log("Tried to equip invalid weapon.");
			return;
		}

		int slot = weapon.getSlot();

		if(weaponSlots[slot] != null) {
			if(overrideExistingWeapon) {
				// Unequip the old weapon
				weaponSlots[slot].unequip();
			} else {
				Debug.Log("Tried to equip weapon " + weapon.name + " into slot " + weapon.type + ", but the weapon " + weaponSlots[slot].name + " is already in that slot. Ignoring.");

				weapon.unequip();
				return;
			}
		}

		weaponSlots[slot] = weapon;
		weaponSlots[slot].equip();

		// If the weapon is from the current slot, show it's instance
		if(slot == currentSlot) {
			weaponSlots[slot].show();

			INSTANCE.updateWeaponText();
		}

		// Update the weapon icon in the selection wheel
		WeaponSelection.updateIcon(weapon);

		//Debug.Log("Loaded weapon " + weapon.name + " into slot " + weapon.type);
    }

	public void updateWeaponText() {
		Weapon weapon = getCurrentWeapon();
		if(weapon != null) {
			weaponInfo.text = weapon.name;
			if(!weapon.unlimitedAmmo)
				weaponInfo.text += "\n" + weapon.ammunition + "/" + weapon.getAmmunitionPerMagazine() * (weapon.magazines - 1);
		} else
			weaponInfo.text = "";
	}

	public static Weapon getWeapon(string name) {
		if(weapons == null)
			return null;

		if(weapons.ContainsKey(name))
			return weapons[name];

		return null;
	}

	public static Weapon[] getAllWeapons() {
		Weapon[] list = new Weapon[weapons.Count];

		weapons.Values.CopyTo(list, 0);

		return list;
	}

	public static string[] getAllWeaponNames() {
		string[] list = new string[weapons.Count];

		weapons.Keys.CopyTo(list, 0);

		return list;
	}

	public static Weapon getCurrentWeapon() {
		return weaponSlots[currentSlot];
	}

	/**
	 * Register the following weapon to the pool.
	 */
	private void register(Weapon weapon) {
		if(weapon == null) {
			Debug.Log("Tried to register invalid weapon.");
			return;
		}

		if(weapons.ContainsKey(weapon.name)) {
			Debug.Log("A weapon with the name \"" + weapon.name + "\" is already registered. Ignoring.");
			return;
		} else {
			weapons.Add(weapon.name, weapon);
		}

		// Refresh the stats
		if(weapon.damage > maxDamage)
			maxDamage = weapon.damage;

		if(weapon.getShootingCooldown() < maxFireRate)
			maxFireRate = weapon.getShootingCooldown();

		if(weapon.range > maxRange)
			maxRange = weapon.range;
	}
}
