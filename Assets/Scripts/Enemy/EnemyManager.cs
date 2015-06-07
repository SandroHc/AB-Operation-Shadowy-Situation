using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	public GameObject prefabGroup;

	private int killCount; // Total enemies killed
	public Text uiKillCounter;

	void Awake() {
		// Load total materials collected
		killCount = PlayerPrefs.GetInt("kill_count", 0);
		uiKillCounter.text = killCount.ToString();
	}

	public void spawn(Vector3 pos) {
		Instantiate(prefabGroup, pos, Quaternion.Euler(Vector3.zero));
	}

	public void kill() {
		killCount++;
		uiKillCounter.text = killCount.ToString();
	}
}
