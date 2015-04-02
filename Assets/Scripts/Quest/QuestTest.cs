using UnityEngine;
using System.Collections;

public class QuestTest : Quest {

	public QuestTest() : base(0, "Test quest", "This is the description for the test quest that was setup just to test the... Quest System") {
		// NO-OP
	}

	public override void initStages() {
		stageList.Add(new StageTest());
		stageList.Add(new Stage2());
	}

	protected class StageTest : Stage {
		private int current, objective;

		public StageTest() {
			current = 0;
			objective = 10;
		}

		public override void setup() {
		}

		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.ProgressType.MATERIAL_PICKUP) {
				current += (int) progress.getNumber();

				GameController.questManager.stageUpdateEvent(this);
			}
			return current >= objective;
		}

		public override string getText() {
			return string.Format("Gathered {0} of {1} material.", current, objective);
		}
	}

	protected class Stage2 : Stage {
		private int current, objective;
		
		public Stage2() {
			current = 0;
			objective = 100;
		}

		public override void setup() {
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.ProgressType.MATERIAL_PICKUP) {
				current += (int) progress.getNumber();

				GameController.questManager.stageUpdateEvent(this);
			}
			return current >= objective;
		}
		
		public override string getText() {
			return string.Format("Gathered {0} of {1} material.", current, objective);
		}
	}
}
