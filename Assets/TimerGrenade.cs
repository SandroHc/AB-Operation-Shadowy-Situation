using UnityEngine;
using System.Collections;

public class TimerGrenade : MonoBehaviour {
	public GameObject particleExplosion;
	
	public float timer = 5f; // 5 seconds to detonate
	public float counter;
	private bool countdown = false;

	// Update is called once per frame
	void Update () {
		if(countdown) {
			counter += Time.deltaTime * timer;

			if(counter >= timer) {
				explode();
				countdown = false;
			}
		}
	}

	public void activate() {
		countdown = true;
	}

	public void explode() {
		// Make the grenade invisible
		GetComponent<MeshRenderer>().enabled = false;

		GameObject particles = GameObject.Instantiate(particleExplosion);
		particles.transform.SetParent(gameObject.transform);
		particles.transform.localPosition = Vector3.zero;

		// Destroy this GameObject in 3 seconds
		Destroy(gameObject, 3.3f);
	}
}
