using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaycastShoot : MonoBehaviour {
	public GameObject[] bulletTex; // Creates an array to use random textures of bullet holes
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
				playAudio(1); // No ammo sound
		}


		// Change the camera FOV smoothly
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
				GameObject obj = Instantiate(bulletTex[Random.Range(0, bulletTex.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Then we'll instantiate a random bullet hole texture from our array and apply it where we click and adjust the position and rotation of textures to match the object being hit
				obj.transform.parent = hit.transform;
				Destroy(obj, 10);
			}

			hit.transform.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}

		ammoRemaining--;

		cooldown = cooldownShoot;
		playAudio(0); // Shoot sound

		updateUI();
	}

	void reload() {
		if(magazines > 1) {
			magazines--;
			ammoRemaining = ammoPerMagazine;
			cooldown = cooldownReload;

			playAudio(2); // Reload sound
		}

		updateUI();
	}

	void playAudio(int index) {
		if(index > audioWeapon.Length - 1)
			return;

		audio.clip = audioWeapon[index];

		if(audio.clip == null)
			return;

		audio.Play();
	}

	void updateUI() {
		bulletUI.text = ammoRemaining + "/" + ammoPerMagazine * (magazines - 1);
	}
}
