using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour {
	private float maxHealth = 100f;
	private float currHealth;

	public Image hpBarImg;

	void Awake() {
		currHealth = maxHealth;
	}

	void LateUpdate() {
		hpBarImg.fillAmount = currHealth / maxHealth;

		takeDamage(0.2f);
	}

	void PlayerDied() {

	}

	public void takeDamage(float damage) {
		currHealth -= damage;
		if(currHealth <= 0) {
			PlayerDied();
			currHealth = 0;
		}
	}

	public float getHealth() {
		return currHealth;
	}
}
