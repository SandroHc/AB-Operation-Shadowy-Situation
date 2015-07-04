public class DialogueQ_01_02_1 : Dialogue {

	public DialogueQ_01_02_1() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "Is there any progress in reverting the effects?";
			setDialogue("Path1_1");
		}
	}
	
	private class Path1_1 : DialogueTalk {
		protected override void dialogue() {
			text = "Yup! We found a special flower that temporarily returns the consciousness to the shadows. If we can harvest a good amount of those flowers, it may be possible to discover the origin of those transformations.";
			setDialogue("Path1_2");
		}
	}

	private class Path1_2 : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "I just need to grab some flowers? Seem easy enough. Where can I find them?";
			setDialogue("Path1_3");
		}
	}

	private class Path1_3 : DialogueTalk {
		protected override void dialogue() {
			text = "Some say that those flowers grow by the river.";
			setDialogue("Path1_4");
		}
	}

	private class Path1_4 : DialogueTalk {
		protected override void dialogue() {
			type = Type.SYSTEM;
			text = "A new waypoint was added to your map.";
		}
	}
}
