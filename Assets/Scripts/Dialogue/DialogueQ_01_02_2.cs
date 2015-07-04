public class DialogueQ_01_02_2 : Dialogue {

	public DialogueQ_01_02_2() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "<i>You hand the flowers.</i>";
			setDialogue("Path1_1");
		}
	}
	
	private class Path1_1 : DialogueTalk {
		protected override void dialogue() {
			text = "Thank you! Now, give us some time to collect some intel from the subjects.";
		}
	}
}
