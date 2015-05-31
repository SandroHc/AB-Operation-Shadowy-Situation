using UnityEngine;
using System.Collections.Generic;

public class GrenadeTimer : MonoBehaviour {
	public float timer = 5f; // 5 seconds to detonate

	private List<Collider> colliders = new List<Collider>();
	private List<string> tags = new List<string>(3) { Tags.player, Tags.enemy, Tags.npc };

	public void activate() {
		Invoke("explode", timer);
	}

	public void explode() {
		// Make the grenade invisible
		GetComponent<MeshRenderer>().enabled = false;

		// Create a new insntance of the particles
		GameObject particles = Instantiate(GameController.prefabManager.particles_explosion);
		particles.transform.SetParent(gameObject.transform);
		particles.transform.localPosition = Vector3.zero;

		playExplosionSound();
		causeDamage();

		// Destroy this GameObject in 3 seconds
		Destroy(gameObject, 3.3f);
	}

	private void playExplosionSound() {
		AudioSource source = GetComponent<AudioSource>();
		if(source == null) source = gameObject.AddComponent<AudioSource>();

		source.PlayOneShot(GameController.audioManager.weapon_grenadeExplode);
    }

	public void causeDamage() {
		foreach(Collider col in colliders) {
			// Calculate the damage using the formula: 100 - x^2  (where x is the distance from the center of the explosion)
			float damage = 100 - Mathf.Pow(Vector3.Distance(col.gameObject.transform.position, gameObject.transform.position), 2);
			if(damage > 0)
				col.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerEnter(Collider other) {
		// If the collider is not in the list, add it
		if(tags.Contains(other.tag) && !colliders.Contains(other))
			colliders.Add(other);
	}

	void OnTriggerExit(Collider other) {
		// If the collider is in the list, remove it
		if(colliders.Contains(other))
			colliders.Remove(other);
	}
}
