using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class WeaponManager : MonoBehaviour {
	public static List<Weapon> weaponList { get; private set; }
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
		// Do this here because Awake() is called BEFORE Start(). So, if a script tried to get a weapon instance in the Awake() event... he whould receive a NULL object.
		if(weaponList == null) {
			weaponList = new List<Weapon>();

			// Get all subclasses of Weapon, and register them
			foreach(Type type in typeof(Weapon).Assembly.GetTypes()) {
				if(type.IsSubclassOf(typeof(Weapon))) {
					registerWeapon(Activator.CreateInstance(type) as Weapon);
				}
			}
		}

		raycast = cameraController.GetComponent<RaycastShoot>();
		audioSource = raycast.GetComponent<AudioSource>();

		camera = cameraController.GetComponent<Camera>();
		defaultFieldOfView = camera.fieldOfView;
		currentFieldOfView = defaultFieldOfView;

		isAimingLast = false; // Not aiming by default

		// Get the saved selected slot
		currentSlot = PlayerPrefs.GetInt("weapon_slot", (int) SLOT.MAIN);

		//Debug.Log ("SLOT SELECTED: " + ((SLOT)currentSlot));

		// Update the weapon selection UI slot index
		WeaponSelection.index = currentSlot;


		// Show the equipped weapon model.
		Weapon weapon = getCurrentWeapon();
		if(weapon != null) weapon.show();
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

		// TODO: Udate label based on events (instead of updating constantly)
		Weapon weapon = getCurrentWeapon();
		if(weapon != null)
			weaponInfo.text = weapon.ammunition + "/" + weapon.getAmmunitionPerMagazine() * (weapon.magazines - 1);
		else
			weaponInfo.text = "NO WEAPON";

		if(Debug.isDebugBuild) {
			// TODO Debug purposes
			if(Input.GetKeyDown(KeyCode.Alpha8)) {
				Debug.Log("Trying to equip M9");
				switchWeapon(getWeapon("M9"));
				getWeapon("M9").unlock();
				getWeapon("M9").craft();
			}
			if(Input.GetKeyDown(KeyCode.Alpha7)) {
				Debug.Log("Trying to equip M16");
				switchWeapon(getWeapon("M16"));
				getWeapon("M16").unlock();
				getWeapon("M16").craft();
			}
		}
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
		}
	}

	private void handleShoot() {
		if(Input.GetKey(InputManager.getKey("fire1"))) {
			Weapon weapon = getCurrentWeapon();
			if(weapon != null && weapon.shoot()) {
				cameraController.GetComponentInParent<RecoilHandler>().recoil(weapon.recoil);
				
				RaycastHit hit = raycast.raycast(weapon.range);
				if(hit.transform != null)
					weapon.targetHit(hit.transform.gameObject);
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
		if(slot < 0 || slot >= weaponSlots.Length) return;

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
	}

	public static Weapon getWeapon(string name) {
		if(weaponList == null)
			return null;

		for(int i=0; i < weaponList.Count; i++)
			if(weaponList[i].name.Equals(name))
				return weaponList[i];

		return null;
	}

	public static Weapon getCurrentWeapon() {
		return weaponSlots[currentSlot];
	}

	private void registerWeapon(Weapon weapon) {
		if(weapon == null) {
			Debug.Log("Tried to register invalid weapon.");
			return;
		}
		
		foreach(Weapon obj in weaponList) {
			if(weapon.name.Equals(obj.name)) {
				Debug.Log("Weapon " + weapon.name + " was already registered. Ignoring.");
				return;
			}
		}

		// Add the weapon to the main list
		weaponList.Add(weapon);

		// Refresh the stats
		if(weapon.damage > maxDamage)
			maxDamage = weapon.damage;

		if(weapon.getShootingCooldown() < maxFireRate)
			maxFireRate = weapon.getShootingCooldown();

		if(weapon.range > maxRange)
			maxRange = weapon.range;
	}

	public static void loadWeaponIntoSlot(Weapon weapon, bool overrideExistingWeapon = false) {
		if(weapon == null) {
			Debug.Log("Tried to equip invalid weapon.");
			return;
		}

		int slot = weapon.getSlot();

		if(weaponSlots[slot] != null && !overrideExistingWeapon) {
			Debug.Log("Tried to equip weapon " + weapon.name + " into slot " + weapon.type + ", but the weapon " + weaponSlots[slot].name + " is already in that slot. Ignoring.");

			weapon.unequip();
			return;
		}

		weaponSlots[slot] = weapon;
		weaponSlots[slot].equip();

		Debug.Log("Loaded weapon " + weapon.name + " into slot " + weapon.type);
	}
}
