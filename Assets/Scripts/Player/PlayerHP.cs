using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour {
	public float maxHealth;
	public float currHealth;
	
	public Image hpBarImg;
	private bool updateBar = false;
	
	void Awake() {
		maxHealth = PlayerPrefs.GetFloat("player_health_max", 100f);
		currHealth = PlayerPrefs.GetFloat("player_health_current", maxHealth);

		if(currHealth <= 0)
			Died();

		updateBar = true; // Force a update
		updateHpBar();
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.T)) takeDamage(10f);
		if(Input.GetKeyDown(KeyCode.Y)) takeDamage(-10f);
		
		updateHpBar();
	}
	
	public void takeDamage(float damage) {
		currHealth -= damage;
		
		// Don't let the current HP be over the maximum or under zero!
		currHealth = Mathf.Max(Mathf.Min(currHealth, maxHealth), 0);
		
		if(currHealth <= 0)
			Died();
		
		// Request an update to the HP bar
		updateBar = true;
	}
	
	public Text infoText;
	
	void Died() {
		//infoText.text = "Oh noes! You died.";
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

	public void save() {
		// Only save if value differs from the default
		if(maxHealth != 100)
			PlayerPrefs.SetFloat("player_health_max", maxHealth);

		// Save current health, always
		PlayerPrefs.SetFloat("player_health_current", currHealth);
	}
}
