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
			if(GameController.getFocused() && Input.GetKeyDown(InputManager.cancel)) {
				closeDialogue();
			}
		}
	}

	public void OnGUI() {
		if(currentDialogue != null) {
			Dialogue.Conversation conv = currentDialogue.getCurrentConversation();
			if(conv != null)
				conv.draw();
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
