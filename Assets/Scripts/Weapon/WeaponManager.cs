using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {
	private static List<Weapon> weaponList;
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

	void Start() {
		weaponList = new List<Weapon>();
		registerWeapon(new WeaponTest());

		// Show the equipped weapon model.
		// Don't do this at the Awake event because at that time the weapons where not initialized yet (yup, Start() is called AFTER Awake())
		Weapon weapon = getCurrentWeapon();
		if(weapon != null) weapon.show();
	}

	void Awake() {
		raycast = cameraController.GetComponent<RaycastShoot>();
		audioSource = raycast.GetComponent<AudioSource>();

		camera = cameraController.GetComponent<Camera>();
		defaultFieldOfView = camera.fieldOfView;
		currentFieldOfView = defaultFieldOfView;

		isAimingLast = false; // Not aiming by default

		// Get the saved selected slot
		currentSlot = PlayerPrefs.GetInt("weapon_slot", (int) SLOT.MAIN);

		Debug.Log ("SLOT SELECTED: " + ((SLOT)currentSlot));

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

		// TODO: Udate label based on events (instead of updating constantly)
		Weapon weapon = getCurrentWeapon();
		if(weapon != null)
			weaponInfo.text = weapon.getAmmunition() + "/" + weapon.getAmmunitionPerMagazine() * (weapon.getMagazines() - 1);
		else
			weaponInfo.text = "NO WEAPON";


		// TODO Debug purposes
		if(Input.GetKeyDown(KeyCode.Alpha8)) {
			Debug.Log("Trying to equip M9");
			switchWeapon(getWeapon("M9"));
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)) {
			Debug.Log("Trying to change to slot SIDE");
			switchSlot((int) SLOT.SIDE);
		}
	}

	private void handleAim() {
		bool isAiming = Input.GetKey(InputManager.fire2);
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
		if(Input.GetKeyUp(InputManager.reload)) { // Only reload if the magazine is not full
			Weapon weapon = getCurrentWeapon();
			if(weapon != null) weapon.reload();
		}
	}

	private void handleShoot() {
		if(Input.GetKeyDown(InputManager.fire1)) {
			Weapon weapon = getCurrentWeapon();
			if(weapon != null && weapon.shoot()) {
				cameraController.GetComponentInParent<RecoilHandler>().recoil(weapon.getRecoil());
				
				RaycastHit hit = raycast.raycast(weapon.getRange());
				if(hit.transform != null)
					weapon.targetHit(hit.transform.gameObject, hit);
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
		for(int i=0; i < weaponList.Count; i++)
			if(weaponList[i].getName() == name)
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
			if(weapon.getName().Equals(obj.getName())) {
				Debug.Log("Weapon " + weapon.getName() + " was already registered. Ignoring.");
				return;
			}
		}

		// Add the weapon to the main list
		weaponList.Add(weapon);

		// Refresh the stats
		if(weapon.getDamage() > maxDamage)
			maxDamage = weapon.getDamage();

		if(weapon.getShootingCooldown() < maxFireRate)
			maxFireRate = weapon.getShootingCooldown();

		if(weapon.getRange() > maxRange)
			maxRange = weapon.getRange();
	}

	public static void loadWeaponIntoSlot(Weapon weapon, bool overrideExistingWeapon = false) {
		if(weapon == null) {
			Debug.Log("Tried to equip invalid weapon.");
			return;
		}

		int slot = weapon.getSlot();

		if(weaponSlots[slot] != null && !overrideExistingWeapon) {
			Debug.Log("Tried to equip weapon " + weapon.getName() + " into slot " + weapon.getWeaponType() + ", but the weapon " + weaponSlots[slot].getName() + " is already in that slot. Ignoring.");

			weapon.unequip();
			return;
		}

		weaponSlots[slot] = weapon;
		weaponSlots[slot].equip();

		Debug.Log("Loaded weapon " + weapon.getName() + " into slot " + weapon.getWeaponType());
	}
}
