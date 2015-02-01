using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Utility class to store information related to any quest.
 */
public abstract class Quest {
	public int id;
	public string name;
	public string description;

	protected List<Stage> stageList = new List<Stage>();
	protected int currentStage;

	private bool isActive;
	private bool isComplete;

	public Quest(int id, string name, string description) {
		this.id = id;
		this.name = name;
		this.description = description;

		int status = PlayerPrefs.GetInt("quest-" + id + "-status", 0);

		this.isActive = status == 1;
		this.isComplete = status == 2;

		this.currentStage = PlayerPrefs.GetInt("quest-" + id + "-stage", 0);

		initStages();
	}

	public abstract void initStages();

	public bool progress(QuestProgress progress) {
		if(isComplete || !isActive) // Ignore progress calls if there is nothing to do here
			return false;


		Debug.Log("Quest " + name + " (" + id + ") update. " + progress.ToString());


		if(currentStage < stageList.Count && stageList[currentStage].update(progress))
			currentStage++;

		//Debug.Log("Reached stage " + currentStage + " of " + stageList.Count);

		if(currentStage >= stageList.Count)
			completeQuest();


		return true;
	}

	protected void completeQuest() {
		PlayerPrefs.SetInt("quest-" + id + "-status", 2);
		isActive = false;
		isComplete = true;

		Debug.Log("Quest " + name + " (" + id + ") completed");
	}

	public void setActive(bool state) {
		isActive = state;
		PlayerPrefs.SetInt("quest-" + id + "-status", state ? 1 : 0);
	}

	public bool getActive() {
		return isActive;
	}

	public bool getCompleted() {
		return isComplete;
	}

	public int getCurrentStage() {
		return currentStage;
	}

	public abstract class Stage {
		protected int objective;
		protected int current;

		public Stage() {
			objective = 10;
			current = 0; // TODO load from PlayerPrefs the current status
		}

		/**
		 * Function to update the current objective inside the current stage.
		 * Returns  true  if the objective was reached; false otherwise.
		 **/
		abstract public bool update(QuestProgress progress);
	}
}
