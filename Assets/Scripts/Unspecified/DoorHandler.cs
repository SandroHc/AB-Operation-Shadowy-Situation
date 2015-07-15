using UnityEngine;

public class DoorHandler : MonoBehaviour {

	public float angle;
	public float initialAngle;

	public float speed = 4f;

	public bool open = false;

	void Awake() {
		// Store the initial angle
		initialAngle = transform.localRotation.z;
    }
	
	void FixedUpdate() {
		Vector3 temp = transform.localRotation.eulerAngles;
		temp.z = Mathf.LerpAngle(temp.z, open ? angle : initialAngle, Time.fixedDeltaTime * speed);
        transform.localRotation = Quaternion.Euler(temp);
	}

	public void toggle() {
		open = !open;
	}
}
