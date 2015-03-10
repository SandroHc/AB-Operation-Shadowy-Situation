using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("uSky/uSky Demo GUI (Legacy)")]
public class uSky_GUI : MonoBehaviour {
	public uSkyManager m_uSky;
	public Texture GuiTex;
	
	private Rect rect = new Rect(2, 2 , 240, 550);
	private const int labelWidth = 115;
	
	private void resetAll() {
		
		m_uSky.Exposure = 1f;
		m_uSky.RayleighScattering = 1f;
		m_uSky.MieScattering = 1f;
		m_uSky.SunAnisotropyFactor = 0.76f;
		m_uSky.SunSize = 1.0f;
		m_uSky.DensityScale = 1.0f;
		m_uSky.SunsetTimeOffset = 0f;
		m_uSky.Wavelengths.x = 680f;
		m_uSky.Wavelengths.y = 550f;
		m_uSky.Wavelengths.z = 440f;
		m_uSky.StarIntensity = 2.0f;
		m_uSky.OuterSpaceIntensity = 0.25f;
		m_uSky.MoonSize = 0.15f;
		m_uSky.MoonInnerCoronaScale = 1.0f;
		m_uSky.MoonOuterCoronaScale = 1.0f;
	}

	void OnGUI () 
	{
		if (m_uSky == null) {
			Debug.Log("Please assign the <b>uSky</b> gameobject to GUI Script");
			return;
		}
		else
		{
			// Scale up for Retina display
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				GUIUtility.ScaleAroundPivot(Vector2.one * 2, Vector2.zero);
			
			GUILayout.BeginArea(rect, "", "Box");
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginHorizontal();
			GUI.DrawTexture(new Rect(72,3,96,48),GuiTex);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>Time of Day</b>", GUILayout.Width(labelWidth));
			GUILayout.Label(m_uSky.TimeOfDay.ToString("0.0"), GUILayout.Width(28));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			m_uSky.TimeOfDay = GUILayout.HorizontalSlider(m_uSky.TimeOfDay, 0, 24);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>Longitude</b>", GUILayout.Width(labelWidth));
			m_uSky.Longitude = GUILayout.HorizontalSlider(m_uSky.Longitude, 0, 360);
			GUILayout.Label(m_uSky.Longitude.ToString("##0."), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Exposure", GUILayout.Width(labelWidth));
			m_uSky.Exposure = GUILayout.HorizontalSlider(m_uSky.Exposure, 0, 5f);
			GUILayout.Label(m_uSky.Exposure.ToString("0.0"), GUILayout.Width(28));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Rayleigh Scattering", GUILayout.Width(labelWidth));
			m_uSky.RayleighScattering = GUILayout.HorizontalSlider(m_uSky.RayleighScattering, 0, 5f);
			GUILayout.Label(m_uSky.RayleighScattering.ToString("0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Mie Scattering", GUILayout.Width(labelWidth));
			m_uSky.MieScattering = GUILayout.HorizontalSlider(m_uSky.MieScattering, 0, 5f);
			GUILayout.Label(m_uSky.MieScattering.ToString("0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Sun Anisotropy Factor", GUILayout.Width(labelWidth));
			m_uSky.SunAnisotropyFactor = GUILayout.HorizontalSlider(m_uSky.SunAnisotropyFactor, 0, 1f);
			GUILayout.Label(m_uSky.SunAnisotropyFactor.ToString("0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Sun Size", GUILayout.Width(labelWidth));
			m_uSky.SunSize = GUILayout.HorizontalSlider(m_uSky.SunSize, 1e-3f, 10.0f);
			GUILayout.Label(m_uSky.SunSize.ToString("0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Density Scale", GUILayout.Width(labelWidth));
			m_uSky.DensityScale = GUILayout.HorizontalSlider(m_uSky.DensityScale, 0f, 4f);
			GUILayout.Label(m_uSky.DensityScale.ToString("0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sunset Time Offset", GUILayout.Width(labelWidth));
			m_uSky.SunsetTimeOffset = GUILayout.HorizontalSlider(m_uSky.SunsetTimeOffset, 0, 1.0f);
			GUILayout.Label(m_uSky.SunsetTimeOffset.ToString("0.0"), GUILayout.Width(24));
			GUILayout.EndHorizontal();

//			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Wavelength R", GUILayout.Width(labelWidth));
			m_uSky.Wavelengths.x = GUILayout.HorizontalSlider(m_uSky.Wavelengths.x, 380f, 780f);
			GUILayout.Label(m_uSky.Wavelengths.x.ToString("###"), GUILayout.Width(24));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Wavelength G", GUILayout.Width(labelWidth));
			m_uSky.Wavelengths.y = GUILayout.HorizontalSlider(m_uSky.Wavelengths.y, 380f, 780f);
			GUILayout.Label(m_uSky.Wavelengths.y.ToString("###"), GUILayout.Width(24));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Wavelength B", GUILayout.Width(labelWidth));
			m_uSky.Wavelengths.z = GUILayout.HorizontalSlider(m_uSky.Wavelengths.z, 380f, 780f);
			GUILayout.Label(m_uSky.Wavelengths.z.ToString("###"), GUILayout.Width(24));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Star Intensity", GUILayout.Width(labelWidth));
			m_uSky.StarIntensity = GUILayout.HorizontalSlider(m_uSky.StarIntensity, 0, 5);
			GUILayout.Label(m_uSky.StarIntensity.ToString("#0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Outer Space Intensity", GUILayout.Width(labelWidth));
			m_uSky.OuterSpaceIntensity = GUILayout.HorizontalSlider(m_uSky.OuterSpaceIntensity, 0, 2);
			GUILayout.Label(m_uSky.OuterSpaceIntensity.ToString("#0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Moon Size", GUILayout.Width(labelWidth));
			m_uSky.MoonSize = GUILayout.HorizontalSlider(m_uSky.MoonSize, 0, 1);
			GUILayout.Label(m_uSky.MoonSize.ToString("#0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Moon Inner Corona", GUILayout.Width(labelWidth));
			m_uSky.MoonInnerCoronaScale = GUILayout.HorizontalSlider(m_uSky.MoonInnerCoronaScale, 0, 5);
			GUILayout.Label(m_uSky.MoonInnerCoronaScale.ToString("#0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Moon Outer Corona", GUILayout.Width(labelWidth));
			m_uSky.MoonOuterCoronaScale = GUILayout.HorizontalSlider(m_uSky.MoonOuterCoronaScale, 0, 5);
			GUILayout.Label(m_uSky.MoonOuterCoronaScale.ToString("#0.0#"), GUILayout.Width(28));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Reset All", GUILayout.Width(labelWidth)))
				resetAll();
			GUILayout.EndHorizontal();
			
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
