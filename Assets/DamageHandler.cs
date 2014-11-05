using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {
	void receiveDamage(float damage) {
		Destroy(gameObject);
	}
}
