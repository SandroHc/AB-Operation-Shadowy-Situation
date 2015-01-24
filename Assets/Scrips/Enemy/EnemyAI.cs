using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform player;
	/*float moveSpeed = 4;
	float rotationSpeed = 4;
	float followDist = 10;*/
	float attackDist = 3;

	private float cooldownAttack = .4f;
	private float cooldown;

	public float damage = 3;

	private RectTransform hpBarCanvas;

	void Awake() {
		hpBarCanvas = transform.Find("default_hp_bar").GetComponent<RectTransform>();

		player = GameController.playerController.transform;
	}

	void Update() {
		if(cooldown >= 0) cooldown -= Time.deltaTime;

		// Make the canvas look athe the player (the canvas local rotation is set to 180º for this to work, as the canvas will be flipped horizontally)
		hpBarCanvas.rotation = player.rotation;

		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationSpeed * Time.deltaTime);

		if(Vector3.Distance(transform.position, player.position) <= attackDist)
			attack();
	}

	void attack() {
		if(cooldown <= 0) {
			//Debug.Log("Attacking!");
			player.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.RequireReceiver);
			cooldown = cooldownAttack;
		}
	}
}
