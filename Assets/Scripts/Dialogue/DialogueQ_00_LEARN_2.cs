using UnityEngine;
using System.Collections;

public class DialogueQ_00_LEARN_2 : Dialogue {

	public DialogueQ_00_LEARN_2() : base(new Start_1(), "Yurippe") {
		// NO-OP
	}
	
	private class Start_1 : DialogueTalk {
		public Start_1() {
			text = "I see you collected the materials I asked you for. Good.";
			nextDialogue = "DialogueQ_00_LEARN_2+Start_2";
		}
	}

	private class Start_2 : DialogueTalk {
		public Start_2() {
			text = "But you are still defenseless. Maybe it's time to craft your first weapon.\nDo you want me to explain the basics of <b>Crafting</b>?";
			nextDialogue = "DialogueQ_00_LEARN_2+SelectionYesNo";
		}
	}

	private class SelectionYesNo : DialogueSelectionYesNo {
		protected override void selectionYes() {
			nextDialogue = "DialogueQ_00_LEARN_2+Path1_1";
		}
		
		protected override void selectionNo() {
			nextDialogue = "DialogueQ_00_LEARN_2+Path2_1";
		}
	}
	
	private class Path1_1 : DialogueTalk {
		public Path1_1() {
			text = "To open the crafting menu, you must click in the shortkey <b>" + InputManager.getKey("crafting") + "</b>.";
			nextDialogue = "DialogueQ_00_LEARN_2+Path1_2";
		}
	}
	
	private class Path1_2 : DialogueTalk {
		public Path1_2() {
			text = "There, you will have vaious equipment spread among categories.\nThose categories are <b>Pistols</b>, <b>Assault Rifles</b>, <b>Shotguns</b>, <b>Sniper Rifles</b>, and <b>Others</b>.";
			nextDialogue = "DialogueQ_00_LEARN_2+Path1_3";
		}
	}
	
	private class Path1_3 : DialogueTalk {
		public Path1_3() {
			text = "To be able to craft something you must first unlock it's blueprint.\nYou can get blueprints by finishing quests and by buying from special NPCs.";
			nextDialogue = "DialogueQ_00_LEARN_2+Path1_4";
		}
	}
	
	private class Path1_4 : DialogueTalk {
		public Path1_4() {
			text = "Once you have a blueprint, you can finally start crafting the item. And, in the process of crafting, you consume materials (those that you collected earlier).\nThe better the item, the more materials it will require.";
			nextDialogue = "DialogueQ_00_LEARN_2+Path1_5";
		}
	}
	
	private class Path1_5 : DialogueTalk {
		public Path1_5() {
			text = "You can now try and craft <b>Scissors</b>, in the <b>Others</b> tab.";
		}
	}
	
	private class Path2_1 : DialogueTalk {
		public Path2_1() {
			text = "OK, OK. Craft <b>Scissors</b>, in the <b>Others</b> tab... b-baka~";
		}
	}
}
