public class DialogueQ_01_01_3 : Dialogue {

	public DialogueQ_01_01_3() : base(new Start(), "Yurippe") {
		// NO-OP
	}

	private class Start : DialogueTalk {
		protected override void dialogue() {
			type = Type.PLAYER;
			text = "<i>You report the news.</i>";
			setDialogue("Path1_1");
		}
	}
	
	private class Path1_1 : DialogueTalk {
		protected override void dialogue() {
			text = "Very well. When you have the time, go and check if there is any progress on the matter.";
		}
	}
}
