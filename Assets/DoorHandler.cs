using UnityEngine;
using System.Collections;

public class DoorHandler : MonoBehaviour {

	public float angle;
	public float initialAngle;

	public float multiplier = 2f;

	// TODO: temp
	public bool open = false;

	void Awake() {
		initialAngle = transform.localRotation.z;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
		Vector3 temp = transform.localRotation.eulerAngles;
		temp.z = Mathf.LerpAngle(temp.z, open ? angle : initialAngle, Time.fixedDeltaTime * multiplier);
        transform.localRotation = Quaternion.Euler(temp);
	}

	public void toggle() {
		open = !open;
	}
}
