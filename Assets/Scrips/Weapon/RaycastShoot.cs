using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaycastShoot : MonoBehaviour {
	public AudioClip[] audioWeapon; // 0 - shoot; 1 - shoot no ammo; 2 - reload

	public int magazines = 2;
	public int ammoPerMagazine = 10;
	private int ammoRemaining;

	public float range = 50f;
	public float damage = 1f;

	public float cooldownShoot = .3f;
	public float cooldownReload = 1f;
	private float cooldown = 0f;

	public Text bulletUI;

	// Variables used when aiming
	private float currentFOV = 60f;
	private float dampVelocity = 0.4f;

	public float recoil = .1f;
	// TODO Temporary reference
	public GameObject camera; // Used to emulate the effect of recoil

	void Awake() {
		ammoRemaining = ammoPerMagazine;
		updateUI();
	}

	void Update() {
		// Draws our raycast and gives it a green color and a length of 10 meters
		//Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.green, 0, false);

		if(cooldown > 0)
			cooldown -= Time.deltaTime;

		if(Input.GetKeyUp(KeyCode.R) && ammoRemaining < ammoPerMagazine) // Only reload if the magazine is not full
			reload();
		
		if(Input.GetButtonDown("Fire1") && cooldown <= 0) {
			if(ammoRemaining > 0)
				shoot();
			else
				audio.PlayOneShot(audioWeapon[1]); // No ammo sound
		}


		// Change the camera FOV smoothly
		if(Camera.main != null) // When in cutscenes, the main camera is disabled
			Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, currentFOV, ref dampVelocity, .3f);

		bool isAiming = Input.GetButton("Fire2"); // Right-click by default
		currentFOV = isAiming ? 40 : 60;
	}

	void shoot() {
		RaycastHit hit;

		// When we left click and our raycast hits something
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range)) {
			// Do not render bullet holes on enemies (because the bullet will be shown on the capsule collider, not the mesh)
			if(hit.transform.gameObject.tag != Tags.enemy) {
				Vector3 position = hit.point;
				position += hit.normal * .0001f; // Lift the object a bit to prevent z-fighting

				GameObject obj = Instantiate(GameController.spriteManager.getBullet(convertTag(hit.transform.tag)), position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Then we'll instantiate a random bullet hole texture from our array and apply it where we click and adjust the position and rotation of textures to match the object being hit
				obj.transform.parent = hit.transform;
				Destroy(obj, 10);
			}

			hit.transform.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}

		ammoRemaining--;
		camera.SendMessage("addRecoid", recoil);
		camera.SendMessage("doRecoid");

		cooldown = cooldownShoot;
		audio.PlayOneShot(audioWeapon[0]); // Shoot sound

		updateUI();
	}

	void reload() {
		if(magazines > 1) {
			magazines--;
			ammoRemaining = ammoPerMagazine;
			cooldown = cooldownReload;

			audio.PlayOneShot(audioWeapon[2]); // Reload sound
		}

		updateUI();
	}

	void updateUI() {
		bulletUI.text = ammoRemaining + "/" + ammoPerMagazine * (magazines - 1);
	}

	/**
	 * Picks the object tag and converts it to the tags used in the SpriteManager
	 */
	string convertTag(string tag) {
		switch(tag) {
		case Tags.wallConcrete:	return "concrete";
		case Tags.wallGlass:	return "glass";
		case Tags.wallMetal:	return "metal";
		case Tags.wallWood:		return "wood";
		default:				return "none";
		}
	}
}
