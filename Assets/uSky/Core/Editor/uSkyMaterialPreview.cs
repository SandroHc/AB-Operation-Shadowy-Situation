using UnityEngine;
using UnityEditor;

[CustomPreview(typeof(uSkyManager))]
public class uSkyMaterialPreview : ObjectPreview {
	
	Editor tmpEditor;

	public override bool HasPreviewGUI()
	{

		// The linear in RenderSetting does not affect the preview,
		// preview is too dark when material is in linear mode.
		// That seems broken in Unity 5 beta, it is working fine in Unity 4.5.X
		// Temporary disable for Unity 5 beta
		#if UNITY_5_0
			return false;
		#else
			return true;
		#endif
	}
	public override void OnPreviewGUI(Rect r, GUIStyle background)
	{
		uSkyManager sr = (uSkyManager)target;
		Material mat = sr.SkyboxMaterial;

		if (mat != null && tmpEditor == null)
			tmpEditor = Editor.CreateEditor(mat);

		if (mat != null && tmpEditor != null)
			tmpEditor.DrawPreview(r);

	}
}
