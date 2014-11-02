using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour {
	private float maxHealth = 100f;
	private float currHealth;

	public Image hpBarImg;
	private bool updateBar = false;

	void Awake() {
		currHealth = maxHealth;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.T)) takeDamage(10f);
		if(Input.GetKeyDown(KeyCode.Y)) takeDamage(-10f);

		updateHpBar();
	}

	public Text infoText;

	void PlayerDied() {
		infoText.text = "Oh noes! You died.";
	}

	public void takeDamage(float damage) {
		currHealth -= damage;

		// Don't let the current HP be over the maximum or under zero!
		currHealth = Mathf.Max(Mathf.Min(currHealth, maxHealth), 0);

		if(currHealth == 0)
			PlayerDied();

		// Request an update to the HP bar
		updateBar = true;
	}

	public float getHealth() {
		return currHealth;
	}

	private void updateHpBar() {
		if(updateBar) {
			float currAmount = currHealth / maxHealth;
			hpBarImg.fillAmount = Mathf.Lerp(hpBarImg.fillAmount, currAmount, Time.deltaTime * 5);
			
			if(Mathf.Abs(hpBarImg.fillAmount - currAmount) < .001f) {
				hpBarImg.fillAmount = currAmount;
				updateBar = false;
			}
		}
	}
}
