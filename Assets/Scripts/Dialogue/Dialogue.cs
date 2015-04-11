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
			GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.Type.DIALOGUE).setStr(name));
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

		protected string nextDialogue = "";

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
		public virtual bool selected(int index) {
			if(!nextDialogue.Equals("")) { // If a nextDialogue is provived, pass on to the next one!
				next(nextDialogue);
				return false;
			} else { // Else, close the dialogue box
				return true;
			}
		}

		protected void next(string str) {
			DialogueAbstract dialogue = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(str) as DialogueAbstract;
			if(dialogue != null)
				DialogueManager.currentDialogue.showDialogue(dialogue);
			else
				Debug.Log("Error while creating an instance for \"" + str + "\"");
		}

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

	public abstract class DialogueSelectionYesNo : DialogueSelection {
		public DialogueSelectionYesNo() {
			options.Add("Yes");
			options.Add("No");
		}

		public override bool selected(int index) {
			if(index == 0)
				selectionYes();
			else
				selectionNo();

			return base.selected(index);
		}

		abstract protected void selectionYes();
		abstract protected void selectionNo();
	}

	public abstract class DialogueTalk : DialogueAbstract {
		public enum Type { NPC = 0xD71111, PLAYER = 0x4583FF, SYSTEM = 0xE68E00 }
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
			GameController.dialogueManager.showTalk(type, title ?? DialogueManager.currentDialogue.name, text);
		}
	}
}
