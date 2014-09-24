using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour {
	private float maxHealth = 100f;
	private float curHealth = 100f;

	public float barDisplay = 0;
	public Vector2 pos = new Vector2(20, 40);
	public Vector2 size = new Vector2(60, 20);
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	
	void OnGUI() {
		// draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty);
		
		// draw the filled-in part:
		GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
		GUI.EndGroup();
		
		GUI.EndGroup();
	}
	
	void Update() {
		barDisplay = Time.time * 0.05f;
	}

	void playerDied() {

	}

	public void takeDamage(float damage) {
		curHealth -= damage;
		if(curHealth <= 0) {
			playerDied();
			curHealth = 0;
		}
	}

	public float getHealth() {
		return curHealth;
	}
}
