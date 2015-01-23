using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	public GameObject prefabGroup;

	public void spawn(Vector3 pos) {
		GameObject go = Instantiate(prefabGroup) as GameObject;
		go.transform.position = pos;
	}
}
