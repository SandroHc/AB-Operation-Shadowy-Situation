using UnityEngine;
using System.Collections;

[AddComponentMenu("uSky/Play TOD")]
[RequireComponent (typeof (uSkyManager))]
public class PlayTOD : MonoBehaviour {

	public bool PlayTimelapse = true;
	public float PlaySpeed = 0.1f;

	private uSkyManager m_uSM;

	protected uSkyManager uSM {
		get{
			if (m_uSM == null) {
				m_uSM = this.gameObject.GetComponent<uSkyManager>();
				if (m_uSM == null)
					Debug.Log("Can't not find uSkyManager");
			}
			return m_uSM;
		}
	}
	void Start (){
		if (PlayTimelapse)
			uSM.SkyUpdate = true;
	}

	// Update is called once per frame
	void Update () {
		if (PlayTimelapse) 
			uSM.TimeOfDay = uSM.TimeOfDay + Time.deltaTime * PlaySpeed;
	}
}
