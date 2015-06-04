using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour {
	public static float maxHealth { get; protected set; }

	private static float _health;
	public static float health {
		get { return _health; }
		set { _health = Mathf.Max(Mathf.Min(value, maxHealth), 0); updateBar = true; }
	}

	private static bool godMode = false;
	
	public Image hpBarImg;
	private static bool updateBar = false;
	
	void Awake() {
		maxHealth = PlayerPrefs.GetFloat("player_health_max", 100f);
		health = PlayerPrefs.GetFloat("player_health_current", maxHealth);

		if(health <= 0)
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
		// In god mode, all damage is ignored
		if(godMode) return;

		// The property controls the min and max values and updates the bar.
		health -= damage;
		
		if(health <= 0)
			Died();
	}
	
	public Text infoText;
	
	public static void Died() {
		//infoText.text = "Oh noes! You died.";
	}
	
	private void updateHpBar() {
		if(updateBar) {
			float currAmount = health / maxHealth;
			hpBarImg.fillAmount = Mathf.Lerp(hpBarImg.fillAmount, currAmount, Time.deltaTime * 5);
			
			if(Mathf.Abs(hpBarImg.fillAmount - currAmount) < .001f) {
				hpBarImg.fillAmount = currAmount;
				updateBar = false;
			}
		}
	}

	public static bool god() {
		return godMode = !godMode;
    }

	public static void save() {
		// Only save if value differs from the default
		if(maxHealth != 100)
			PlayerPrefs.SetFloat("player_health_max", maxHealth);

		// Save current health, always
		PlayerPrefs.SetFloat("player_health_current", health);
	}
}
