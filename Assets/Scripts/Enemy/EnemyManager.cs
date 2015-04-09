using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	public GameObject prefabGroup;

	private int killCount; // Total material collected
	public Text uiKillCounter;

	void Awake() {
		// Load total materials collected
		killCount = PlayerPrefs.GetInt("kill_count", 0);
		uiKillCounter.text = killCount.ToString();
	}

	public void spawn(Vector3 pos) {
		GameObject go = Instantiate(prefabGroup) as GameObject;
		go.transform.position = pos;
	}

	public void kill() {
		killCount++;
		uiKillCounter.text = killCount.ToString();
	}
}
