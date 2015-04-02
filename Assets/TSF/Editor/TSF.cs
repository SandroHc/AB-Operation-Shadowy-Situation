using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

enum TRANS_OPTIONS 
{
	Simple = 0,
	TwoPasses = 1,
	Cutout = 2
}

public class TSF: MaterialEditor {

	public MaterialProperty[] props;

	public override void OnInspectorGUI ()
	{
		if (!isVisible)
			return;
		props = MaterialEditor.GetMaterialProperties(targets);


		EditorGUIUtility.fieldWidth = 64.0f;


		
		if (!props[0].hasMixedValue )
		{
			if( props[0].floatValue == 1)
			{
				
					SetShader(Shader.Find("TSF/BaseOutline1"));
					props = MaterialEditor.GetMaterialProperties(targets);
					props[0].floatValue = 1;
					PropertiesChanged();
			}
			

			if ( props[0].floatValue == 0 )
			{
				SetShader(Shader.Find("TSF/Base1"));
				props = MaterialEditor.GetMaterialProperties(targets);
				props[0].floatValue = 0;
				PropertiesChanged();
			}
		}

		ShaderProperty(props[3], "Shade Tex");

		ShaderProperty(props[1], "Enable Detail Tex");

		if(props[1].floatValue == 1 )
		{
			ShaderProperty(props[2], "Detail Tex");
		}


		ShaderProperty(props[4], "Enable Color Tint");
		
		if(props[4].floatValue == 1)
		{
			ShaderProperty(props[5], "Color Tint");
		}

		ShaderProperty(props[7], "Brightness");

		ShaderProperty(props[8], "Double Sided");
		
		if(props[8].floatValue == 1)
		{
			props[9].floatValue = 0f;
		}
		else
		{
			props[9].floatValue = 2f;
		}

		if (!props[0].hasMixedValue)
			ShaderProperty(props[0], "Outline");

		if (!props[0].hasMixedValue)
		if(props[0].floatValue == 1f)
		{
			if (!props[10].hasMixedValue)
			ShaderProperty(props[10], "Outline Color");
			if (!props[11].hasMixedValue)
			ShaderProperty(props[11], "Outline Thickness");
		}
	}
}