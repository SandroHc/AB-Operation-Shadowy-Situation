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

	public Quest(int id, string name, string description) {
		this.id = id;
		this.name = name;
		this.description = description;

		this.isActive = PlayerPrefs.GetInt("quest-" + id + "-status", 0) == 1;
		this.currentStage = PlayerPrefs.GetInt("quest-" + id + "-stage", 0);
	}

	public abstract void initStages();

	public bool progress(QuestProgress progress) {
		// TODO derpy way to test stages
		PlayerPrefs.SetInt("quest-" + id + "-stage", 0);


		return isActive;
	}

	public void setActive(bool state) {
		isActive = state;
		PlayerPrefs.SetInt("quest-" + id + "-status", state ? 1 : 0);


		Debug.Log("Quest " + id + ": status: " + isActive);
	}

	public bool getActive() {
		return isActive;
	}

	public int getCurrentStage() {
		return currentStage;
	}

	public class Stage {
		private int id;

		private int objective;
		private int current;

		public Stage(int stageId) {
			this.id = stageId;

			objective = 2;
			current = 0; // TODO load from PlayerPrefs the current status
		}

		public void update(QuestProgress progress) {
			current++;
		}

		public int getId() {
			return id;
		}
	}
}
