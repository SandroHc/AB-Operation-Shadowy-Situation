using UnityEngine;
using System.Collections;

public class LogoRotation : MonoBehaviour {
	public float speed = 100f;
	
	void Update() {
		transform.Rotate(0, (speed * Time.deltaTime) % 360, 0);
	}
}
