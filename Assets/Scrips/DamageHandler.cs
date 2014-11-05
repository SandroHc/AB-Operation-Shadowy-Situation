using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {
	public float damage = 1f;

	void takeDamage(float dmg) {
		damage -= dmg;

		if(damage <= 0)
			Destroy(gameObject);
	}
}
