using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	public Dialogue currentDialogue = null;

	public void Start() {
		//currentDialogue = new DialogueTest();
	}

	public void Update() {
		if(currentDialogue != null) {
			Dialogue.Conversation conv = currentDialogue.getCurrentConversation();
			if(conv != null) {
				int size = conv.getSize();

				for(int i=1; i <= size; i++) {
					if(Input.GetKeyDown(i.ToString()))
						currentDialogue.selected(i-1);
				}
			}
		}
	}

	public void OnGUI() {
		if(currentDialogue != null) {
			Dialogue.Conversation conv = currentDialogue.getCurrentConversation();
			if(conv != null) {
				List<string> options = conv.get();
				int size = options.Count;

				int btnWidth = 400;
				int btnHeight = 30;

				int padding = Screen.width / 2 - btnWidth / 2;
				int paddingVert = 8;

				int posY = size * paddingVert + (size-1) * btnHeight;

				for(int i = size-1; i >= 0; i--) {
					GUI.Button(new Rect(padding, Screen.height - posY, btnWidth, btnHeight), new GUIContent((i + 1) + ". " + options[i]));

					posY += paddingVert + btnHeight;
				}
			}
		}
	}
}
