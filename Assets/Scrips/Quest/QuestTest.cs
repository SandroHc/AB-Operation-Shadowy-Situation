using UnityEngine;
using System.Collections;

public class QuestTest : Quest {

	public QuestTest() : base(1, "Test quest", "This is the description for the test quest that was setup just to test the... Quest System") {
		// NO-OP
	}

	public override void initStages() {
		stageList.Add(new Stage(0));
	}

	public void enable() {
		base.enable(); // Call the main function
		Debug.Log(name + " received activation call");
	}

	public void disable() {
		base.disable(); // Call the main function
		Debug.Log(name + " received deactivation call");
	}
	
	public bool progress(QuestProgress progress) {
		if(!base.progress(progress)) return false; // Check if this progress should be ignored. Like when the quest is not activated.

		string progressValue = progress.type == QuestProgress.ProgressType.INTERACTION ? progress.str : progress.number.ToString();
		Debug.Log(name + " received progress call. Value: " + progressValue);

		return true;
	}
}
