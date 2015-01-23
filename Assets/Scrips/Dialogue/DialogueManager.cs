using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	public Dialogue currentDialogue = null;

	public void Update() {
		if(currentDialogue != null) {
			Dialogue.Conversation conv = currentDialogue.getCurrentConversation();
			if(conv != null) {
				int size = conv.getSize();

				for(int i=1; i <= size; i++) {
					if(Input.GetKeyDown(i.ToString()))
						selected(i-1);
				}
			}
		}
	}

	public void LateUpdate() {
		if(currentDialogue != null) {
			if(GameController.getFocused() && Input.GetButtonDown("Cancel")) {
				closeDialogue();
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
					if(GUI.Button(new Rect(padding, Screen.height - posY, btnWidth, btnHeight), new GUIContent((i + 1) + ". " + options[i])))
						selected(i);

					posY += paddingVert + btnHeight;
				}
			}
		}
	}

	private void selected(int i) {
		if(currentDialogue != null) {
			if(currentDialogue.selected(i)) {
				closeDialogue(); // Close the dialogue, as per the API
			}
		}
	}

	public void showDialogue(Dialogue dialogue) {
		Debug.Log("Opening dialogue " + dialogue.ToString());

		currentDialogue = dialogue;
		GameController.setFocused(true, false);
	}

	public void closeDialogue() {
		if(currentDialogue != null) {
			Debug.Log("Closing dialogue " + currentDialogue.ToString());

			// Send the event to the Dialogue class (in the event to rollback any change)
			currentDialogue.closingDialogue();
			// Null the reference to the dialogue
			currentDialogue = null;

			// Resume focus on the game
			GameController.setFocused(false);
		}
	}
}
