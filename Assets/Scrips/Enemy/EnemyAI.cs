using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform player;

	private float cooldownAttack = .4f;
	private float cooldown;

	public float damage = 3;

	private RectTransform hpBarCanvas;

	void Awake() {
		hpBarCanvas = transform.Find("default_hp_bar").GetComponent<RectTransform>();
	}

	void Update() {
		if(cooldown >= 0) cooldown -= Time.deltaTime;

		hpBarCanvas.gameObject.SetActive(player != null);

		if(player != null) {
			// Make the canvas look athe the player (the canvas local rotation is set to 180º for this to work, as the canvas will be flipped horizontally)
			hpBarCanvas.rotation = player.rotation;

			attack();
		}
	}

	void attack() {
		if(Vector3.Distance(transform.position, player.position) > 3)
			return;

		if(cooldown <= 0) {
			//Debug.Log("Attacking!");
			player.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.RequireReceiver);
			cooldown = cooldownAttack;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == Tags.player)
			player = other.transform;
	}


	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == Tags.player)
			player = null;
	}
}
