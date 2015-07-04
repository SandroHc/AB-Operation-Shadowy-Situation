using UnityEngine;
using System.Collections;

public class DialogueQ_00_01_1 : Dialogue {

	public DialogueQ_00_01_1() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		protected override void dialogue() {
			text = "Well, hello there! Do you want me to explain the basics of this world?";
			setDialogue("SelectionYesNo");
		}
	}

	private class SelectionYesNo : DialogueSelectionYesNo {
		protected override void selectionYes() {
			setDialogue("Path1_1");
		}

		protected override void selectionNo() {
			setDialogue("Path2_1");
		}
	}

	private class Path1_1 : DialogueTalk {
		protected override void dialogue() {
			text = "So, let’s start from the beginning. In this world no one can die, or get sick.";
			setDialogue("Path1_2");
		}
	}

	private class Path1_2 : DialogueTalk {
		protected override void dialogue() {
			text = "Our main objective as the Frente de Batalha is to reach to God and ask him why we had such miserable lives before dying, without accomplishing the things that were most important to us.";
			setDialogue("Path1_3");
		}
	}

	private class Path1_3 : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "But… you said no one dies here…?";
			setDialogue("Path1_4");
		}
	}

	private class Path1_4 : DialogueTalk {
		protected override void dialogue() {
			text = "That’s right, no one dies because we are already dead. This is the <b>Life After Death</b> after all.";
			setDialogue("Path1_5");
		}
	}

	private class Path1_5 : DialogueTalk {
		protected override void dialogue() {
			text = "So, are you interested in joining us?";
			setDialogue("Path1_6");
		}
	}

	private class Path1_6 : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "Well… do I have a choice?";
			setDialogue("Path1_7");
		}
	}

	private class Path1_7 : DialogueTalk {
		protected override void dialogue() {
			text = "You, I like you! Let’s get to work then. Your first assignment as a member of the Frente de Batalha will be to collect some materials.";
			setDialogue("Path1_8");
		}
	}

	private class Path1_8 : DialogueTalk {
		protected override void dialogue() {
			type = Type.SYSTEM;
			text = "Those materials are used to craft from tools to weapons. But we’ll get to that later. There are multiple spots where you can collect materials around this world. You will now be guided to the nearest one.";
		}
	}

	private class Path2_1 : DialogueTalk {
		protected override void dialogue() {
			text = "So, let’s get on the work! Go and collect some materials.";
		}
	}
}
