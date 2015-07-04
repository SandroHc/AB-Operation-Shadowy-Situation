public class DialogueQ_01_01_1 : Dialogue {

	public DialogueQ_01_01_1() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "Is there any “shadow-thing” for me to work on?";
			setDialogue("Path1_1");
		}
	}
	
	private class Path1_1 : DialogueTalk {
		protected override void dialogue() {
			text = "As a matter of fact, there is! The recon team made some progress related to the shadows.";
			setDialogue("Path1_2");
		}
	}

	private class Path1_2 : DialogueTalk {
		protected override void dialogue() {
			text = "Go there and collect the intel from the team.";
			setDialogue("Path1_3");
		}
	}

	private class Path1_3 : DialogueTalk {
		protected override void dialogue() {
			type = Type.SYSTEM;
			text = "A new waypoint was added to your map.";
		}
	}
}
