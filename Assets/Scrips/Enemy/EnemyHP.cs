using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	public float maxHealth = 100f;
	private float currHealth;
	
	public Image hpBarImg;
	private bool updateBar = false;
	
	void Awake() {
		currHealth = maxHealth;
	}

	void Update() {
		if(hpBarImg != null)
			updateHpBar();
	}
	
	public void takeDamage(float damage) {
		currHealth -= damage;
		
		// Don't let the current HP be over the maximum or under zero!
		currHealth = Mathf.Max(Mathf.Min(currHealth, maxHealth), 0);
		
		if(currHealth == 0)
			Died();

		// Request an update to the HP bar
		updateBar = true;
	}

	void Died() {
		Destroy(gameObject);
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
