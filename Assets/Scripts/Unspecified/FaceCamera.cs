using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	void Update() {
		if(Camera.main != null) // Only update if the main camera is active (e.g. not in a cutscene)
			transform.rotation = Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime * 10);
	}
}
