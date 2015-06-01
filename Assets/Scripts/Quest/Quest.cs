using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Utility class to store information related to any quest.
 */
public abstract class Quest {
	public enum STATUS { UNKNOWN = -1, INACTIVE = 0, ACTIVE = 1, COMPLETED = 2 };
	public STATUS status { get; set; }

	public string id { get; protected set; }
	public string name { get; protected set; }
	public string description { get; protected set; }

	public List<Stage> stages { get; protected set; }
	public int currentStage { get; protected set; }

	public Quest(string id, string name, string description) {
		this.id = id;
		this.name = name;
		this.description = description;

		this.status = (STATUS) PlayerPrefs.GetInt("quest-" + id + "-status", (int) STATUS.INACTIVE);
		this.currentStage = PlayerPrefs.GetInt("quest-" + id + "-stage", 0);

		this.stages = new List<Stage>();
		initStages();

		if(status == STATUS.ACTIVE) {
			// Check if the current stage is bigger than the last stage ID.
			// In that case, mark the quest as COMPLETED
			if(currentStage >= stages.Count)
				complete();
			else if(stages[currentStage] != null && stages[currentStage].setup()) // In case the stage was already completed on startup, to to the next one
				nextStage();
		}
	}

	public abstract void initStages();

	public bool progress(QuestProgress progress) {
		if(status != STATUS.ACTIVE) // Ignore progress calls if there is nothing to do here
			return false;

		if(currentStage < stages.Count && stages[currentStage].update(progress))
			nextStage();

		return true;
	}

	protected void nextStage() {
		// Fire the finish stage event
		if(stages[currentStage] != null)
			stages[currentStage].finish();

		// Update the current stage pointer, so new progress events will be redirected to there.
		currentStage++;

		// Save the current stage in the preferences
		PlayerPrefs.SetInt("quest-" + id + "-stage", currentStage);

		// Check if the current stage as the last one, and fire the quest finish event 
		if(currentStage >= stages.Count) {
			complete();
		} else {
			// Send the setup event to the new stage
			stages[currentStage].setup();

			GameController.questManager.stageUpdateEvent(stages[currentStage]);
		}
	}

	protected void complete() {
		// Update the status to COMPLETED
		setStatus(STATUS.COMPLETED);

		// Fire the quest finished event
		GameController.questManager.questFinishedEvent(this);
	}

	/**
	 * Used to reset the Quest to it's starting point
	 **/
	public void reset() {
		setStatus(STATUS.ACTIVE);

		currentStage = 0;
		PlayerPrefs.SetInt("quest-" + id + "-stage", currentStage);

		if(stages[currentStage] != null)
			stages[currentStage].setup();

		GameController.questManager.questStartedEvent(this);
	}

	/**
	 * Return the current status of the quest.
	 **/
	public STATUS getStatus() {
		return status;
	}

	public void setStatus(STATUS status) {
		Debug.Log("Quest " + name + " (" + id + ") changed from " + this.status + " to " + status);

		this.status = status;
		PlayerPrefs.SetInt("quest-" + id + "-status", (int) status);
	}

	public abstract class Stage {

		/**
		 * Used to setup any stage-related mechanics.
		 * Like spawn a mob, reset a door, etc.
		 * 
		 * Returns true if this stage's requirements were already met in the setup phase.
		 **/
		public virtual bool setup() {
			return false;
		}

		/**
		 * Function to update the current objective inside the current stage.
		 * Returns  true  if the objective was reached; false otherwise.
		 **/
		abstract public bool update(QuestProgress progress);

		/**
		 * Function called when the stage is complete.
		 **/
		public virtual void finish() {
			// NO-OP
		}

		/**
		 * Used to generate a text that states the current status of this stage.
		 * e.g. Talk to Deliora.
		 * 		Picked up 3 out of 9 flowers.
		 **/
		abstract public string getText();
	}

	/**
	 * 
	 * PRE-CREATED STAGES
	 * 
	**/
	
	protected class GoTo : Stage {
		private Vector3 objective;
		private GameObject sentinel;

		public GoTo(Vector3 pos) {
			objective = pos;
		}
		
		public override bool setup() {
			// Create a sentinel to check whenever the player sets foot on the target
			sentinel = Object.Instantiate(GameController.prefabManager.marker, objective, Quaternion.Euler(0, 0, 0)) as GameObject;
			sentinel.GetComponent<PositionSentinel>().setup();

			return false;
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

	protected class TalkTo : Stage {
		Interaction npc;
		string dialogue;

		public TalkTo(Interaction npcScript, string dialogueClass) {
			this.npc = npcScript;
			this.dialogue = dialogueClass;
		}
		
		public override bool setup() {
			npc.dialogue = dialogue;

			return false;
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.Type.DIALOGUE) {
				if(progress.getStr().Equals(npc.name)) {
					GameController.questManager.stageUpdateEvent(this);
					return true;
				}
			}
			
			return false;
		}
		
		public override string getText() {
			return "Talk to <b>" + npc.name + "</b>.";
		}
	}

	protected class Craft : Stage {
		Weapon weapon;
		
		public Craft(Weapon weapon) {
			this.weapon = weapon;
		}

		public override bool setup() {
			return weapon.isCrafted; // In case the weapon was already crafted, complete the stage
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.Type.ITEM_CRAFT) {
				if(progress.getStr().Equals(weapon.name)) {
					GameController.questManager.stageUpdateEvent(this);
					return true;
				}
			}
			
			return false;
		}
		
		public override string getText() {
			return "Craft <b>" + weapon.name + "</b>.";
		}
	}

	protected class Collect : Stage {
		private int current;
		private int ammount;

		private string key;

		public Collect(string key, int ammount) {
			this.ammount = ammount;
			this.key = key;
		}
		
		public override bool setup() {
			current = PlayerPrefs.GetInt(key, 0);

			return current >= ammount; // Finish the stage incase the collected ammount is enough.
		}
		
		public override bool update(QuestProgress progress) {
			if(progress.type == QuestProgress.Type.MATERIAL_PICKUP) {
				current += (int) progress.getNumber();
				
				PlayerPrefs.SetInt(key, current);


				if(current >= ammount)
					return true;

				GameController.questManager.stageUpdateEvent(this);
			}
			
			return false;
		}
		
		public override void finish() {
			PlayerPrefs.DeleteKey(key);
		}
		
		public override string getText() {
			return string.Format("Gathered {0} of {1} materials.", current, ammount);
		}
	}
}
