using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class MMD4MecanimMaterialInspector : MaterialEditor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();
		
		if( !isVisible ) {
			return;
		}
		
		Material targetMat = target as Material;
		string[] keyWords = targetMat.shaderKeywords;

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField( "Shader Keywords", EditorStyles.boldLabel );

		bool isSpecular = keyWords.Contains( "SPECULAR_ON" );
		EditorGUI.BeginChangeCheck();
		isSpecular = EditorGUILayout.Toggle( "Specular", isSpecular );
		if( EditorGUI.EndChangeCheck() ) {
			if( isSpecular ) {
				targetMat.EnableKeyword( "SPECULAR_ON" );
				targetMat.DisableKeyword( "SPECULAR_OFF" );
			} else {
				targetMat.DisableKeyword( "SPECULAR_ON" );
				targetMat.EnableKeyword( "SPECULAR_OFF" );
			}
			EditorUtility.SetDirty( targetMat );
		}
	}
}