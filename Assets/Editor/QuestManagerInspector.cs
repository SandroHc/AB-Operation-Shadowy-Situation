using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (QuestManager))]
public class MyScriptInspector : Editor {
	void OnInspectorGUI() {
		QuestManager script = (QuestManager) target;
		script.questList = EditorGUILayout.ObjectField("Quest List", script.questList, typeof(IQuest), true);

		DrawDefaultInspector();
	}
}