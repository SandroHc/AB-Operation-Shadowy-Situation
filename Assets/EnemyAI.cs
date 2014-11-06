using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform target;
	float moveSpeed = 4;
	float rotationSpeed = 4;
	float followDist = 10;
	float attackDist = 3;

	private float cooldownAttack = .4f;
	private float cooldown;

	public float damage = 3;

	void Update() {
		if(cooldown >= 0) cooldown -= Time.deltaTime;

		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationSpeed * Time.deltaTime);

		if(Vector3.Distance(transform.position, target.position) <= followDist) {
			//Debug.Log("Following!");
			rigidbody.AddRelativeForce(transform.forward * moveSpeed * Time.deltaTime); // TODO Should be doing this at FixedUpdate()

			if(Vector3.Distance(transform.position, target.position) <= attackDist)
				attack();
		}
	}

	void attack() {
		if(cooldown <= 0) {
			//Debug.Log("Attacking!");
			target.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.RequireReceiver);
			cooldown = cooldownAttack;
		}
	}
}
