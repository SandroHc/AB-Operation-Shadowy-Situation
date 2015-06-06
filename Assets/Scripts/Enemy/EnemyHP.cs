using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	private float _maxHealth = 100f;
	public float maxHealth {
		get { return _maxHealth; }
		protected set { _maxHealth = value; }
	}

	private float _health;
	public float health {
		get { return _health; }
		set { _health = Mathf.Max(Mathf.Min(value, maxHealth), 0); updateBar = true; }
	}

	//public float maxHealth = 100f;
	
	public Image hpBarImg;
	private bool updateBar = false;
	
	void Awake() {
		health = maxHealth;
	}

	void Update() {
		if(hpBarImg != null && updateBar)
			updateHpBar();
	}
	
	public void takeDamage(float damage) {
		// The property controls the min and max values and updates the bar.
		health -= damage;
		
		if(health == 0)
			Died();
	}

	public void Died() {
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.MONSTER_KILL).setPosition(transform.position));
		GameController.enemyManager.kill();

		Destroy(gameObject);
	}

	private void updateHpBar() {
		float currAmount = health / maxHealth;
		hpBarImg.fillAmount = Mathf.Lerp(hpBarImg.fillAmount, currAmount, Time.deltaTime * 5);
			
		if(Mathf.Abs(hpBarImg.fillAmount - currAmount) < .001f) {
			hpBarImg.fillAmount = currAmount;
			updateBar = false;
		}
	}
}
