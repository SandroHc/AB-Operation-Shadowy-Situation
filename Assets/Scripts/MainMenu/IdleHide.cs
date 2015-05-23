using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IdleHide : MonoBehaviour {

	public float idleTime = 0f;
	
	public Image logo;

	void Update () {
		if(!Input.anyKey && Input.GetAxis("Mouse X") < Mathf.Epsilon && Input.GetAxis("Mouse Y") < Mathf.Epsilon)
			idleTime++;
		else
			idleTime = 0f;

		Color temp = logo.color;
		if(idleTime > 300)
			temp.a = Mathf.Lerp(logo.color.a, 0, .025f);
		else
			temp.a = Mathf.Lerp(logo.color.a, 1, .3f);
		logo.color = temp;
	}
}
