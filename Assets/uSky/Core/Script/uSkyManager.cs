using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("uSky/uSky Manager")]
public class uSkyManager : MonoBehaviour 
{

	public bool SkyUpdate = true;
//	public bool useTOD_Slider = true;
	[Range(0.0f, 24.0f)]
	public float TimeOfDay = 17.0f;
	[Range(0.0f, 360.0f)]
	public float Longitude = 0.0f;

	[Space(10)]
//	[Header("Day Sky")]
	[Range(0.0f, 5.0f)]
	public float Exposure = 1.0f;
	[Range(0.0f, 5.0f)]
	public float RayleighScattering = 1.0f;
	[Range(0.0f, 5.0f)]
	public float MieScattering = 1.0f;
	[Range (0.0f,0.9995f)]
	public float SunAnisotropyFactor = 0.76f;
	[Range (1e-3f,10.0f)]
	public float SunSize = 1.0f;
	[Range(0.0f, 4.0f)]
	public float DensityScale = 1.0f; // Aerosols density offset
//	[Range(1.0f, 3.0f)]
//	public float AltitudeScale = 1.0f; 
	[Range (0.0f, 1.0f)]
	public float SunsetTimeOffset = 0.0f;
	
	// Wavelengths for visible light ray from 380 to 780 
	public Vector3 Wavelengths = new Vector3(680f, 550f, 440f); // sea level mie

	public Color SkyTint = new Color(1f, 1f, 1f, 1f);
	public Color GroundColor = new Color(0.369f, 0.349f, 0.341f, 1f);
	public GameObject m_sunLight;

	[Space (10)]
//	[Header ("Night Sky")]
	public bool EnableNightSky = true;
	public Color NightZenithColor = new Color(0.29f,0.42f,0.58f,1f);
	public Color NightHorizonColor = new Color(0.43f,0.47f,0.5f,1f);

	[Range(0.0f, 5.0f)]
	public float StarIntensity = 2.0f;
	[Range(0.0f, 2.0f)]
	public float OuterSpaceIntensity = 0.25f;

	public Color MoonInnerCoronaColor = Color.white;
	[Range(0.0f, 5.0f)]
	public float MoonInnerCoronaScale = 1.0f;
	public Color MoonOuterCoronaColor = new Color(0.54f,0.66f,0.75f,1f);
	[Range(0.0f, 5.0f)]
	public float MoonOuterCoronaScale = 1.0f;
	[Range(0.0f, 1.0f)]
	public float MoonSize = 0.15f;
	public GameObject m_moonLight;

	public Material SkyboxMaterial;
	public bool AutoApplySkybox = true;
	public bool LinearSpace = false;
	public bool Tonemapping = false;

	private Vector3 euler;
	private Matrix4x4 moon_wtl;

	StarsField Stars ;
	private Mesh starsMesh;
	private Material m_starMaterial;

	// NOTE: "Stars.Shader" need to be placed in Resources folder for mobile build!
	protected Material starMaterial {
		get {
			if (m_starMaterial == null) {
				m_starMaterial = new Material(Shader.Find("Hidden/uSky/Stars"));
				m_starMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_starMaterial;
		} 
	}
	protected void InitStarsMesh (){
		Stars = new StarsField();
		if (starsMesh == null){
			starsMesh = Stars.InitializeStarfield();
			starsMesh.hideFlags = HideFlags.HideAndDontSave;
			if (starsMesh == null)
				Debug.Log(" Can't find or read <b>StarsData.bytes</b> file.");
		}
	}
	protected void OnEnable() {
		if(m_sunLight == null)
			Debug.Log("Please apply the Directional Light to uSkyManager");
		if (SkyboxMaterial == null)
			Debug.Log("Please apply the Skybox Material to uSkyManager");

		if (EnableNightSky) {
			InitStarsMesh ();
		}
	}
	
	protected void OnDisable() {

		if(starsMesh) {
			DestroyImmediate(starsMesh);
		}
		if (m_starMaterial){
			DestroyImmediate(m_starMaterial);
		}
	}

	private void Start() 
	{
//		if(useTOD_Slider)
			InitSun();

		if(SkyboxMaterial){

			InitMaterial(SkyboxMaterial);
			if (AutoApplySkybox)
				ApplySkybox(SkyboxMaterial);
		}
		if (EnableNightSky )
			starTab ();
	}

//	private void OnLevelWasLoaded(int level) {
//		if(useTOD_Slider)
//			InitSun();
//		if(SkyboxMaterial){
//			InitMaterial(SkyboxMaterial);
//			if (AutoAsignSkyBox)
//				ApplySkybox(SkyboxMaterial);
//		}
//	}

	private void ApplySkybox(Material mat){
		RenderSettings.skybox = mat;
	}

	void Update()
	{

		if (SkyUpdate)
		{
			// reset Time of Day slider
			if (TimeOfDay >= 24.0f) TimeOfDay = 0.0f;

			// Update every frame for all shader Paramaters
			if(SkyboxMaterial != null)
			{
//				if(useTOD_Slider)
					InitSun();
					InitMaterial(SkyboxMaterial);
			}

			#if UNITY_EDITOR
			if (AutoApplySkybox && RenderSettings.skybox != SkyboxMaterial)
				ApplySkybox(SkyboxMaterial);
			if (EnableNightSky)
			{
				InitStarsMesh ();
				starTab ();
			}
			#endif
		}
		// Draw Stars field
		if (EnableNightSky && SunDir.y < 0.2f)
			Graphics.DrawMesh (starsMesh, Vector3.zero, Quaternion.identity, starMaterial, 0 );
	}

	// Align the sun direction with TOD slider
	void InitSun()
	{
		euler.x = TimeOfDay * 360.0f / 24.0f - 90.0f;
		euler.y = Longitude;
		if(m_sunLight != null)
			m_sunLight.transform.localEulerAngles = euler;
	}

	void InitMaterial(Material mat)
	{
		mat.SetVector ("_SunDir", SunDir); 
		mat.SetMatrix ("_Moon_wtl", getMoonMatrix);
		
		mat.SetVector ("_betaR", BetaR);
		mat.SetVector ("_betaM", BetaM);

		// x = Sunset, y = Day, z = Rayleigh, w = Night
		mat.SetVector ("_SkyMultiplier", skyMultiplier);

		mat.SetFloat ("_SunSize", 24.0f / SunSize);
		mat.SetVector ("_mieConst", mieConst);
		mat.SetVector ("_miePhase_g", miePhase_g);
		mat.SetVector ("_SkyTint", skyTint);
		mat.SetVector ("_GroundColor", bottomTint);
		mat.SetVector ("_NightHorizonColor", NightHorizonColor * nt);
		mat.SetVector ("_NightZenithColor", getNightZenithColor);
		mat.SetVector ("_MoonInnerCorona", getMoonInnerCorona);
		mat.SetVector ("_MoonOuterCorona", getMoonOuterCorona); 
		mat.SetFloat ("_MoonSize", MoonSize);
		mat.SetVector ("_colorCorrection", ColorCorrection);

		mat.shaderKeywords = hdrMode.ToArray ();

		if (EnableNightSky)
			mat.DisableKeyword("NIGHTSKY_OFF");
		else
			mat.EnableKeyword("NIGHTSKY_OFF");

		mat.SetFloat ("_OuterSpaceIntensity", OuterSpaceIntensity);
		starMaterial.SetFloat ("StarIntensity", starBrightness);

	}
	
	public Vector3 SunDir {
		get {
			return (m_sunLight != null)? m_sunLight.transform.forward * -1: new Vector3(0.321f,0.766f,-0.557f);
		}
	}

	private Matrix4x4 getMoonMatrix {
		get {
			if (m_moonLight == null) {
					// predefined Moon Direction
					moon_wtl = Matrix4x4.TRS (Vector3.zero, new Quaternion (-0.9238795f, 8.817204e-08f, 8.817204e-08f, 0.3826835f), Vector3.one);
			} else if (m_moonLight != null) {
					moon_wtl = m_moonLight.transform.worldToLocalMatrix;
					moon_wtl.SetColumn (2, Vector4.Scale (new Vector4 (1, 1, 1, -1), moon_wtl.GetColumn (2)));
			}
			return moon_wtl;
		}
	}
//	public Vector3 MoonDir {
//		get {
//			return getMoonMatrix.GetColumn(2);
//		}
//	}

	public Vector3 BetaR{
		get {
			// Evaluate Beta Rayleigh function based on A.J.Preetham

			Vector3 WL = Wavelengths * 1e-9f;

			const float Km = 1000.0f;
			const float n = 1.0003f;		// the index of refraction of air
			const float N = 2.545e25f;		// molecular density at sea level
			const float pn = 0.035f;		// depolatization factor for standard air

			Vector3 waveLength4 = new Vector3 (Mathf.Pow (WL.x, 4), Mathf.Pow (WL.y, 4), Mathf.Pow (WL.z, 4));

			Vector3 theta = 3.0f * N * waveLength4 * (6.0f - 7.0f * pn);

			float ray = (8 * Mathf.Pow (Mathf.PI, 3) * Mathf.Pow (n * n - 1.0f, 2) * (6.0f + 3.0f * pn));

			return Km * new Vector3 (ray / theta.x, ray / theta.y, ray / theta.z);
		}
	}
	
	public Vector3 BetaM{
		get {
			// Beta Mie (simplified) function based on Cryengine
			return new Vector3 (Mathf.Pow (Wavelengths.x, -0.84f), Mathf.Pow (Wavelengths.y, -0.84f), Mathf.Pow (Wavelengths.z, -0.84f));
		}
	}

	public float uMuS{
		get {
			// Sun fall ratio function based on Eric Bruneton 
			return Mathf.Atan (Mathf.Max (SunDir.y, -0.1975f) * 5.35f) / 1.1f + 0.74f;
		}
	}
	// night time
	public float nt {
		get {
				return 1 - Mathf.Min (1, uMuS);
		}
	}

	public Vector3 miePhase_g {
		get{
			// partial mie phase : approximated with the Cornette Shanks phase function
			float g2 = SunAnisotropyFactor * SunAnisotropyFactor;
			return new Vector3 (MieScattering * 1.25f * ((1.0f - g2) / (2.0f + g2)), 1.0f + g2, 2.0f * SunAnisotropyFactor);
		}
	}
	public Vector3 mieConst {
		get {
			return new Vector3 (1f, BetaR.x/ BetaR.y, BetaR.x/ BetaR.z) * 4e-3f / brignessConstant;
		}
	}

	// x = Sunset, y = Day, z = Rayleigh, w = Night
	public Vector4 skyMultiplier {
		get{
			return new Vector4 (Mathf.Clamp01 ((uMuS - 1.0f) * (2.0f - SunsetTimeOffset * 2.0f)),
			                    Exposure * 4 * Mathf.Min (1.0f, uMuS), 
			                    RayleighScattering * 0.75f / brignessConstant, nt);
		}
	}

	private float densityMoreThenOne {
		get{
			return Mathf.Lerp (Mathf.Max ( 1.0f, DensityScale), Mathf.Pow (DensityScale, 0.35f) , nt );
		}
	}
	private float brignessConstant {
		get {
			return Mathf.Pow (densityMoreThenOne, 0.5f);
		}
	}
			
	public Vector4 skyTint {
		get { 
			return new Vector4 (Mathf.Max (SkyTint.r ,1e-2f) * densityMoreThenOne,
			                    Mathf.Max (SkyTint.g ,1e-2f) * densityMoreThenOne,
			                    Mathf.Max (SkyTint.b ,1e-2f) * densityMoreThenOne, 
			                    Mathf.Min ( 1.0f, DensityScale));
		}
	}

	private Vector3 bottomTint{
		get {
			float cs = LinearSpace ? 1e-2f : 2e-2f;
			return new Vector3 (BetaR.x / (GroundColor.r * cs * densityMoreThenOne),
			                    BetaR.y / (GroundColor.g * cs * densityMoreThenOne),
			                    BetaR.z / (GroundColor.b * cs * densityMoreThenOne));
		}
	}

	public Vector2 ColorCorrection {
		get{
			return (LinearSpace && Tonemapping) ? new Vector2 (0.5f, 1.5f) :
			LinearSpace ? new Vector2 (1f, 2.2f) : new Vector2 (1f, 1f);
		}
	}

	public Vector3 getNightZenithColor {
		get{
			return new Vector3 (Mathf.Clamp( NightZenithColor.r,1e-3f,0.999f)* 1e-2f,
			                    Mathf.Clamp( NightZenithColor.g,1e-3f,0.999f)* 1e-2f,
			                    Mathf.Clamp( NightZenithColor.b,1e-3f,0.999f)* 1e-2f);
		}
	}

	public Vector4 getMoonInnerCorona {
		get {
			return new Vector4 (MoonInnerCoronaColor.r * nt,
					            MoonInnerCoronaColor.g * nt,
					            MoonInnerCoronaColor.b * nt,
								6e2f / MoonInnerCoronaScale);
		}
	}

	public Vector4 getMoonOuterCorona {
		get {
			float cs = LinearSpace ? 4.5f : 10f; 												//	default sky  
//			float cs = Tonemapping && LinearSpace? 4 : LinearSpace? 64 : Tonemapping? 2 : 40 ; //  standard sky 
			return new Vector4 (MoonOuterCoronaColor.r * 0.25f * nt,
			                    MoonOuterCoronaColor.g * 0.25f * nt,
			                    MoonOuterCoronaColor.b * 0.25f * nt,
			                    cs / MoonOuterCoronaScale); 
		}
	}

	private List<string> hdrMode {
		get {
			return new List<string> { Tonemapping ? "USKY_HDR_ON" : "USKY_HDR_OFF" };
		}
	}

	// Stars shader setting
	private float starBrightness {
		get {
			float cs = LinearSpace ? 1f : 2.2f;
			return StarIntensity * nt * cs;
		}
	}
	protected static readonly Vector2[] tab = 
	{
		new Vector2(0.897907815f,-0.347608525f),new Vector2(0.550299290f, 0.273586675f),new Vector2(0.823885965f, 0.098853070f),new Vector2(0.922739035f,-0.122108860f),
		new Vector2(0.800630175f,-0.088956800f),new Vector2(0.711673375f, 0.158864420f),new Vector2(0.870537795f, 0.085484560f),new Vector2(0.956022355f,-0.058114540f)
	};
	
	void starTab (){
			
		if (starMaterial != null)
		for (int i = 0; i < 8; i++) {
			string tabArray = "_tab" + i;
			starMaterial.SetVector(tabArray,tab[i]);
		}
	}
}

