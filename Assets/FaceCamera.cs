using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	void Update () {
		transform.rotation = Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime * 10);
	}
}
