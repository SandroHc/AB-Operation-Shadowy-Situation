using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Utility class to store information related to any quest.
 */
public abstract class Quest {
	public enum QUEST_STATUS { UNKNOWN = -1, INACTIVE = 0, ACTIVE = 1, COMPLETED = 2 };
	public QUEST_STATUS status;

	public int id;
	public string name;
	public string description;

	protected List<Stage> stageList = new List<Stage>();
	protected int currentStage;

	public Quest(int id, string name, string description) {
		this.id = id;
		this.name = name;
		this.description = description;

		this.status = (QUEST_STATUS) PlayerPrefs.GetInt("quest-" + id + "-status", (int) QUEST_STATUS.INACTIVE);
		this.currentStage = PlayerPrefs.GetInt("quest-" + id + "-stage", 0);

		initStages();
	}

	public abstract void initStages();

	public bool progress(QuestProgress progress) {
		if(status != QUEST_STATUS.ACTIVE) // Ignore progress calls if there is nothing to do here
			return false;


		Debug.Log("Quest " + name + " (" + id + ") update. " + progress.ToString());


		if(currentStage < stageList.Count && stageList[currentStage].update(progress))
			currentStage++;

		if(currentStage >= stageList.Count)
			setStatus(QUEST_STATUS.COMPLETED);

		return true;
	}

	/**
	 * Return the current status of the quest.
	 **/
	public QUEST_STATUS getStatus() {
		return status;
	}

	public void setStatus(QUEST_STATUS status) {
		Debug.Log("Quest " + name + " (" + id + ") changed from " + this.status.ToString() + " to " + status.ToString());

		this.status = status;
		PlayerPrefs.SetInt("quest-" + id + "-status", (int) status);
	}

	public List<Stage> getStages() {
		return stageList;
	}

	public int getCurrentStage() {
		return currentStage;
	}

	public abstract class Stage {
		/**
		 * Function to update the current objective inside the current stage.
		 * Returns  true  if the objective was reached; false otherwise.
		 **/
		abstract public bool update(QuestProgress progress);

		/**
		 * Used to generate a text that states the current status of this stage.
		 * e.g. Talk to Deliora.
		 * 		Picked up 3 out of 9 flowers.
		 **/
		abstract public string getText();
	}
}
