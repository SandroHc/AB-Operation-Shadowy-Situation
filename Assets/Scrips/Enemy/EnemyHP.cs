using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	public float maxHealth = 100f;
	private float currHealth;
		
	void Awake() {
		currHealth = maxHealth;
	}
	
	public void takeDamage(float damage) {
		currHealth -= damage;
		
		// Don't let the current HP be over the maximum or under zero!
		currHealth = Mathf.Max(Mathf.Min(currHealth, maxHealth), 0);
		
		if(currHealth == 0)
			Died();
	}

	void Died() {
		Destroy(gameObject);
	}
}
