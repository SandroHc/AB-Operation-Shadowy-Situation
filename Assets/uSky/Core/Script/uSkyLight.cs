using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("uSky/uSky Light")]
[RequireComponent (typeof (uSkyManager))]
public class uSkyLight : MonoBehaviour {

	private uSkyManager m_uSM;
	
	[Range(0f,4f)]
	public float SunIntensity = 0.5f;

	public Gradient LightColor = new Gradient();

	public bool EnableMoonLighting = true;

	[Range(0f,2f)]
	public float MoonIntensity = 0.1f;
	
	private float currentTOD;
	private float dayTime;
	private float nightTime;
	
	protected uSkyManager uSM {
		get{
			if (m_uSM == null) {
				m_uSM = this.gameObject.GetComponent<uSkyManager>();
				if (m_uSM == null)
					Debug.Log(" Can't not find uSkyManager");
			}
			return m_uSM;
		}
	}
	// check sun light
	protected GameObject sunLight{
		get{
			if(uSM != null){
				return (uSM.m_sunLight != null)? uSM.m_sunLight : null;
			}else 
				return null;
		}
	}
	// check moon light
	protected GameObject moonLight{
		get{
			if(uSM != null){
				return (uSM.m_moonLight != null)? uSM.m_moonLight : null;
			}else 
				return null;
		}
	}

	 void SetGradientColorKey(){
 
		GradientColorKey[] sck = new GradientColorKey[7];

		sck [0].color = new Color (0.33f, 0.39f, 0.44f, 1f);
		sck [0].time = 0.25f;
		sck [1].color = new Color (0.96f, 0.6f, 0.15f, 1f);
		sck [1].time = 0.26f;
		sck [2].color = new Color (0.976f, 0.816f, 0.565f, 1f);
		sck [2].time = 0.32f;
		sck [3].color = new Color (0.984f, 0.871f, 0.729f, 1f);
		sck [3].time = 0.50f;
		sck [4].color = new Color (0.976f, 0.816f, 0.565f, 1f);
		sck [4].time = 0.68f;
		sck [5].color = new Color (0.96f, 0.6f, 0.15f, 1f);
		sck [5].time = 0.74f;
		sck [6].color = new Color (0.33f, 0.39f, 0.44f, 1f);
		sck [6].time = 0.75f;

		GradientAlphaKey[] sak = new GradientAlphaKey[2];
		sak [0].alpha = 1f;
		sak [0].time = 0f;
		sak [1].alpha = 1f;
		sak [1].time = 1f;

		LightColor.SetKeys (sck, sak);
	}


	void OnEnable (){
		// Check: if the gradient has only white color, then load the predefined gradient key setting.
		if (LightColor.Evaluate (0f).Equals (Color.white) && LightColor.Evaluate (0.5f).Equals (Color.white)&& LightColor.Evaluate (1f).Equals (Color.white))
		SetGradientColorKey();
	}

	void Start () {
		if (uSM != null){
			SunAndMoonLightUpdate ();
		}
	}

	void SunAndMoonLightUpdate (){
		currentTOD = uSM.TimeOfDay / 24;
		dayTime = Mathf.Min (1.0f, uSM.uMuS);
		nightTime = uSM.nt;

		if (sunLight != null)
		if (sunLight.GetComponent<Light> () != null) {
			sunLight.GetComponent<Light> ().intensity = uSM.Exposure * SunIntensity * dayTime;
			sunLight.GetComponent<Light> ().color = LightColor.Evaluate (currentTOD) * dayTime;
		}
		if (EnableMoonLighting && moonLight != null) {
			if (moonLight.GetComponent<Light> () != null) {
				moonLight.GetComponent<Light> ().enabled = true;
				moonLight.GetComponent<Light> ().intensity =  uSM.Exposure * MoonIntensity * nightTime;
				moonLight.GetComponent<Light> ().color = LightColor.Evaluate (currentTOD) * nightTime;
			}
		}else if(moonLight != null)
			if (moonLight.GetComponent<Light> () != null)
				moonLight.GetComponent<Light> ().enabled = false;
	}

	void Update (){
		if (uSM != null) {
			if (uSM.SkyUpdate) {

				SunAndMoonLightUpdate ();

			}
		}
	}

}
