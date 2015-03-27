using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Interaction))]
public class PropertyHolderEditor : Editor {
//	private Interaction instance;
	
	public SerializedProperty type_prop;
	public SerializedProperty name_prop;

	/* Dialogue */
	public SerializedProperty dialogue_prop;

	/* Material pick up */
	public SerializedProperty picksMax_prop;

	void OnEnable() {
//		instance = (Interaction) target;

		// Setup the SerializedProperties
		type_prop = serializedObject.FindProperty("type");
		name_prop = serializedObject.FindProperty("name");

		/* Dialogue */
		dialogue_prop = serializedObject.FindProperty("dialogue");

		/* Material pick up */
		picksMax_prop = serializedObject.FindProperty("picksMax");    
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update();
		
		EditorGUILayout.PropertyField(type_prop);
		EditorGUILayout.PropertyField(name_prop);

		switch((Interaction.Type) type_prop.intValue) {
		case Interaction.Type.Dialogue:
			//instance.dialogue = EditorGUILayout.ObjectField(dialogue_prop.serializedObject, typeof(Dialogue), true) as Dialogue;
			break;
		case Interaction.Type.ItemPickUp: // Not yet implemented
			break;
		case Interaction.Type.MaterialPickUp:
			EditorGUILayout.IntSlider(picksMax_prop, 0, 10, new GUIContent("Maximum Picks"));
			break;
		}
		
		serializedObject.ApplyModifiedProperties();
	}
}