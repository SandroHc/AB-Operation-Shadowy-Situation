public class DialogueQ_01_01_2 : Dialogue {

	public DialogueQ_01_01_2() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "Yurippe sent me to pick the latest news on the NPC-to-shadow thingy.";
			setDialogue("Path1_1");
		}
	}
	
	private class Path1_1 : DialogueTalk {
		protected override void dialogue() {
			text = "During our experiments, we gathered that the NPCs seem to turn into shadows unconsciously, like if someone or something triggered the transformation.";
			setDialogue("Path1_2");
		}
	}

	private class Path1_2 : DialogueTalk {
		protected override void dialogue() {
			text = "We are now working on a way to reverse the transformation. That way we may be able to discover what triggers those things.";
			setDialogue("Path1_3");
		}
	}

	private class Path1_3 : DialogueTalk {
		protected override void dialogue() {
			text = "I think this is all we’ve got for now.";
		}
	}
}
