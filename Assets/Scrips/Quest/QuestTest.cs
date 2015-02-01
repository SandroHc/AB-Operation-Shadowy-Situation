using UnityEngine;
using System.Collections;

public class QuestTest : Quest {

	public QuestTest() : base(1, "Test quest", "This is the description for the test quest that was setup just to test the... Quest System") {
		// NO-OP
	}

	public override void initStages() {
		stageList.Add(new StageTest());
	}

	protected class StageTest : Stage {
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.ProgressType.MATERIAL_PICKUP) {
				current += progress.number;
				Debug.Log("Stage updated. " + current + " of " + objective + " materials picked up");
			}
			return current >= objective;
		}
	}
}
