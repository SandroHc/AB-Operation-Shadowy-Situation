using UnityEngine;
using System.Collections;

public class QuestTest : Quest {

	public QuestTest() : base(1, "Test quest", "This is the description for the test quest that was setup just to test the... Quest System") {
		// NO-OP
	}

	public override void enable() {
		Debug.Log(name + " received activation call");
	}

	public override void disable() {
		Debug.Log(name + " received deactivation call");
	}
	
	public override void progress(QuestProgress progress) {
		string progressValue = progress.type == QuestProgress.ProgressType.INTERACTION ? progress.str : progress.number.ToString();
		Debug.Log(name + " received progress call. Value: " + progressValue);
	}
}
