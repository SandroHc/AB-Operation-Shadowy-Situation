using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {
	void takeDamage(float damage) {
		Destroy(gameObject);
	}
}
