using UnityEngine;
using System.Collections;

public class Quest_00_LEARN : Quest {

	public Quest_00_LEARN() : base(1, "Learning the Ropes", "This is the description for the test quest that was setup just to test the... Quest System") {
		// NO-OP
	}

	public override void initStages() {
		stages.Add(new Stage1());
		stages.Add(new Stage2());
		stages.Add(new Stage3());
	}
	
	protected class Stage1 : Stage {
		private Vector3 objective = new Vector3(0,0,0);
		private GameObject sentinel;

		public override void setup() {
			// Create a sentinel to check whenever the player sets foot on the target
			sentinel = Object.Instantiate(GameController.prefabManager.marker, objective, Quaternion.Euler(0, 0, 0)) as GameObject;
			sentinel.GetComponent<PositionSentinel>().setup();
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.Type.POSITION) {
				if(Vector3.Distance(progress.getPosition(), objective) <= 5f) {
					GameController.questManager.stageUpdateEvent(this);
					return true;
				}
			}

			return false;
		}

		public override void finish() {
			// Destroy the sentinel when it is no longer needed
			Object.Destroy(sentinel);
		}
		
		public override string getText() {
			return "Go to the indicated zone.";
		}
	}

	protected class Stage2 : Stage {
		
		public override void setup() {
			GameObject.FindGameObjectWithTag(Tags.npc).GetComponent<Interaction>().dialogue = "DialogueQ_00_LEARN_1";
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.Type.DIALOGUE) {
				if(progress.getStr().Equals("Yurippe")) {
					GameController.questManager.stageUpdateEvent(this);
					return true;
				}
			}
			
			return false;
		}

		public override string getText() {
			return "Talk to Yurippe.";
		}
	}

	protected class Stage3 : Stage {
		private int current;
		private int objective = 10;

		public override void setup() {
			current = PlayerPrefs.GetInt("quest_00_stage3_current", 0);
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.Type.MATERIAL_PICKUP) {
				setProgress((int) progress.getNumber());

				if(current >= objective)
					return true;
			}
			
			return false;
		}

		public override void finish() {
			PlayerPrefs.DeleteKey("quest_00_stage3_current");
		}
		
		public override string getText() {
			return string.Format("Gathered {0} of {1} materials.", current, objective);
		}

		private void setProgress(int number) {
			current += number;

			PlayerPrefs.SetInt("quest_00_stage3_current", current);

			GameController.questManager.stageUpdateEvent(this);
		}
	}
}
