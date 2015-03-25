using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {
	public static List<Weapon> weaponList;
	public static Weapon equippedWeapon;
	private static Weapon defaultWeapon;

	public static float weaponCooldown = 0;

	// Variables used when aiming
	private new Camera camera;

	private float defaultFieldOfView;
	private float currentFieldOfView;
	private float speedChangeFOV = 0.4f; // 40%/s

	private bool isAimingLast;

	private RaycastShoot raycast;
	public static AudioSource audioSource;

	public Text weaponInfo;

	public CameraController cameraController;

	void Start() {
		defaultWeapon = new WeaponTest();

		weaponList = new List<Weapon>();
		weaponList.Add(defaultWeapon);


		string equippedWeaponName = PlayerPrefs.GetString("weapon_equipped", "");
		// If the weapon name is not equal to "", try to find the weapon instance
		if(!"".Equals(equippedWeaponName))
			equippedWeapon = getWeapon(equippedWeaponName);

		// If no weapon instance was found, set the default one
		if(equippedWeapon == null)
			equippedWeapon = defaultWeapon;

		// Prepare the weapon for use
		equippedWeapon.equip();
	}

	void Awake() {
		raycast = cameraController.GetComponent<RaycastShoot>();
		audioSource = raycast.GetComponent<AudioSource>();

		camera = cameraController.GetComponent<Camera>();
		defaultFieldOfView = camera.fieldOfView;
		currentFieldOfView = defaultFieldOfView;

		isAimingLast = false; // Not aiming by default
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

		// Cehck Fire controls
		handleShoot();

		// TODO: Udate label based on events (instead of updating constantly)
		weaponInfo.text = equippedWeapon.getAmmunition() + "/" + equippedWeapon.getAmmunitionPerMagazine() * (equippedWeapon.getMagazines() - 1);
	}

	private void handleAim() {
		bool isAiming = Input.GetKey(InputManager.fire2);
		currentFieldOfView = isAiming ? defaultFieldOfView * .6f /* shrink the FOV by 60% */ : defaultFieldOfView;
		
		if(isAiming != isAimingLast) {
			if(isAiming) {
				equippedWeapon.aimEnter();
				cameraController.aimEnter();
			} else {
				equippedWeapon.aimExit();
				cameraController.aimExit();
			}
		}
		
		isAimingLast = isAiming;
		
		// Update the camera FOV
		camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, currentFieldOfView, ref speedChangeFOV, .3f);
	}

	private void handleReload() {
		if(Input.GetKeyUp(InputManager.reload)) // Only reload if the magazine is not full
			equippedWeapon.reload();
	}

	private void handleShoot() {
		if(Input.GetKeyDown(InputManager.fire1)) {
			if(equippedWeapon.shoot()) {
				cameraController.GetComponentInParent<RecoilHandler>().recoil(equippedWeapon.getRecoil());
				
				RaycastHit hit = raycast.raycast(equippedWeapon.getRange());
				if(hit.transform != null)
					equippedWeapon.targetHit(hit.transform.gameObject, hit);
			}
		}
	}

	public static void switchWeapon(Weapon weapon) {
		if(weapon == null)
			return;

		// Send the unequip event to the old weapon
		if(equippedWeapon != null)
			equippedWeapon.unequip();

		// Swap the weapons and send the equip event
		equippedWeapon = weapon;
		equippedWeapon.equip();
	}

	public static Weapon getWeapon(string name) {
		for(int i=0; i < weaponList.Count; i++)
			if(weaponList[i].getName() == name)
				return weaponList[i];

		return null;
	}
}
