using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Dialogue {
	public string name;

	protected DialogueAbstract currentDialogue;

	public Dialogue(DialogueAbstract startingDialogue, string npcName = "") {
		name = npcName;
		currentDialogue = startingDialogue;
	}

	public void show() {
		currentDialogue.show();
	}

	public void update() {
		currentDialogue.update();
	}

	/**
	 * Return value will decide if the dialogue is closed.
	 * e.g. returning false will keep the dialogue (used to show more conversations, etc)
	 */
	public void selected(int index) {
		//Debug.Log("Dialogue: selected " + index);

		if(currentDialogue.selected(index)) {
			GameController.dialogueManager.closeDialogue();

			// Fire the dialogue finished event to the Quest System
			// Do not fire on the close() method because the player might have canceled the dialogue
			GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.DIALOGUE).setStr(name));
		}
	}

	public void close() { }

	public void showDialogue(Dialogue.DialogueAbstract dialogue) {
		currentDialogue = dialogue;
		dialogue.show();
	}

	public enum DialogueType { TALK, SELECTION } 

	public abstract class DialogueAbstract {
	//	protected Dialogue super;

		private DialogueType type;

		public DialogueAbstract(DialogueType type) {
			this.type = type;

		//	this.super = DialogueManager.currentDialogue;
		}

		abstract public void show();
		abstract public void update();
		
		/**
		 * Receive the selected index.
		 * 
		 * In a selection dialogue, the index is the selected button;
		 * In a talk dialogue, the index is irrelevant.
		 * 
		 * Returns a boolean saying if the dialogue window should be closed (e.g. no more conversations).
		 * 	If true: close the dialogue window
		 * 	If false: go to the next dialogue
		 **/
		abstract public bool selected(int index);

		public DialogueType getType() {
			return type;
		}
	}

	public abstract class DialogueSelection : DialogueAbstract {
		protected List<string> options = new List<string>();

		public DialogueSelection() : base(DialogueType.SELECTION) {
			// NO-OP
		}

		public override void update() {
			// Check for key input
			for(int i=1; i <= options.Count; i++) {
				if(Input.GetKeyDown(i.ToString()))
					DialogueManager.currentDialogue.selected(i-1);
			}
		}

		public List<string> get() {
			return options;
		}

		public int getSize() {
			return options.Count;
		}

		public override void show() {
			GameController.dialogueManager.showSelection(options.ToArray());
		}
	}

	public abstract class DialogueTalk : DialogueAbstract {
		public enum Type { NPC = 0xFF0000, PLAYER = 0x0000FF, SYSTEM = 0xAA6622 }
		protected Type type = Type.NPC;

		protected string title;
		protected string text;

		public DialogueTalk() : base(DialogueType.TALK) {
			// NO-OP
		}

		public override void update() {
			if(Input.GetKeyDown(KeyCode.Space))
				DialogueManager.currentDialogue.selected(0);
		}
		
		public override void show() {
			// If a title is available, use it. Else, use the Dialogue name (usually the NPC name)
			GameController.dialogueManager.showTalk(type, title != null ? title : DialogueManager.currentDialogue.name, text);
		}
	}
}
