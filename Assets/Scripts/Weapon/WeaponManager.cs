using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {
	public static List<Weapon> weaponList;
	public static Weapon equippedWeapon;

	public static float weaponCooldown = 0;

	// Variables used when aiming
	private new Camera camera;

	private float defaultFieldOfView;
	private float currentFieldOfView;
	private float speedChangeFOV = 0.4f; // 40%/s

	private bool isAimingLast;

	public RaycastShoot raycast;
	public static AudioSource audioSource;

	public Text weaponInfo;

	public CameraController cameraController;

	void Start() {
		weaponList = new List<Weapon>();
		weaponList.Add(new WeaponTest());


		string equippedWeaponName = PlayerPrefs.GetString("weapon_equipped", "");
		if(equippedWeaponName == "")
			equippedWeapon = weaponList[0]; // TODO Have a default weapon
		else
			equippedWeapon = getWeapon(equippedWeaponName);

		// Prepare the weapon for use
		equippedWeapon.equip();
	}

	void Awake() {
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

		if(weaponCooldown > 0)
			weaponCooldown -= Time.deltaTime;

		// Check Aim controls
		bool isAiming = Input.GetKey(InputManager.fire2);
		currentFieldOfView = isAiming ? defaultFieldOfView * .6f /* shrink the FOV by 60% */ : defaultFieldOfView;

		if(isAiming != isAimingLast) {
			if(isAiming)
				equippedWeapon.aimEnter();
			else
				equippedWeapon.aimExit();
		}

		isAimingLast = isAiming;

		// Update the camera FOV
		camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, currentFieldOfView, ref speedChangeFOV, .3f);


		// Check Reload controls
		if(Input.GetKeyUp(InputManager.reload)) // Only reload if the magazine is not full
			equippedWeapon.reload();

		// Cehck Fire controls
		if(Input.GetKeyDown(InputManager.fire1)) {
			if(equippedWeapon.shoot()) {
				cameraController.recoil(equippedWeapon.getRecoil());

				RaycastHit hit = raycast.raycast(equippedWeapon.getRange());
				if(hit.transform != null)
					equippedWeapon.targetHit(hit.transform.gameObject, hit);
			}
		}

		// TODO: Udate label based on events (instead of updating constantly)
		weaponInfo.text = equippedWeapon.getAmmunition() + "/" + equippedWeapon.getAmmunitionPerMagazine() * (equippedWeapon.getMagazines() - 1);
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

	private static Weapon getWeapon(string name) {
		for(int i=0; i < weaponList.Count; i++)
			if(weaponList[i].getName() == name)
				return weaponList[i];

		return null;
	}
}
