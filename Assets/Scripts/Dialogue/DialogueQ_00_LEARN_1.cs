using UnityEngine;
using System.Collections;

public class DialogueQ_00_LEARN_1 : Dialogue {

	public DialogueQ_00_LEARN_1() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		public Start() {
			text = "Well, hello there! Do you want me to explain the basics of this world?";
			nextDialogue = "DialogueQ_00_LEARN_1+SelectionYesNo";
		}
	}

	private class SelectionYesNo : DialogueSelectionYesNo {
		protected override void selectionYes() {
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_1";
		}

		protected override void selectionNo() {
			nextDialogue = "DialogueQ_00_LEARN_1+Path2_1";
		}
	}
	
	private class Path1_1 : DialogueTalk {
		public Path1_1() {
			text = "So, let’s start from the beginning. In this world no one can die, or get sick.";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_2";
		}
	}

	private class Path1_2 : DialogueTalk {
		public Path1_2() {
			text = "Our main objective as the Frente de Batalha is to reach to God and ask him why we had such miserable lives before dying, without accomplishing the things that were most important to us.";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_3";
		}
	}

	private class Path1_3 : DialogueTalk {
		public Path1_3() {
			type = Type.PLAYER;
			text = "But… you said no one dies here…?";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_4";
		}
	}

	private class Path1_4 : DialogueTalk {
		public Path1_4() {
			text = "That’s right, no one dies because we are already dead. This is the <b>Life After Death</b> after all.";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_5";
		}
	}

	private class Path1_5 : DialogueTalk {
		public Path1_5() {
			text = "So, are you interested in joining us?";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_6";
		}
	}

	private class Path1_6 : DialogueTalk {
		public Path1_6() {
			type = Type.PLAYER;
			text = "Well… do I have a choice?";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_7";
		}
	}

	private class Path1_7 : DialogueTalk {
		public Path1_7() {
			text = "You, I like you! Let’s get to work then. Your first assignment as a member of the Frente de Batalha will be to collect some materials.";
			nextDialogue = "DialogueQ_00_LEARN_1+Path1_8";
		}
	}

	private class Path1_8 : DialogueTalk {
		public Path1_8() {
			type = Type.SYSTEM;
			text = "Those materials are used to craft from tools to weapons. But we’ll get to that later. There are multiple spots where you can collect materials around this world. You will now be guided to the nearest one.";
			//nextDialogue = ""; // Terminate the dialogue
		}
	}

	private class Path2_1 : DialogueTalk {
		public Path2_1() {
			text = "So, let’s get on the work! Go and collect some materials.";
		}
	}
}
