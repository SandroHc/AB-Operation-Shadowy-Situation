using UnityEngine;
using System.Collections;

public class ShatterGlass : MonoBehaviour {

	public GameObject shatteredObj;

	public void Shatter(Vector3 pos) {
		gameObject.GetComponent<MeshRenderer>().enabled = false;

		shatteredObj.SetActive(true);
		shatteredObj.GetComponent<FracturedObject>().Explode(transform.localPosition, 100f);
	}
}
