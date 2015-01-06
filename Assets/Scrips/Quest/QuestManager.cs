using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {
	public IQuest[] questList;

	private int activeQuestId;
	public Quest activeQuest = null;

	void Awake() {
		activeQuestId = PlayerPrefs.GetInt("active_quest", -1);

		if(activeQuestId != -1) {
			foreach(Quest quest in questList) {
				if(quest.id == activeQuestId) {
					activeQuest = quest;
					activeQuest.enable();
					break;
				}
			}
		}
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Y)) {
			Debug.Log("Enabling quest");
			enableQuest(1);
		} else if(Input.GetKeyDown(KeyCode.U)) {
			Debug.Log("Sending str progress to " + activeQuest);
			if(activeQuest != null)
				activeQuest.progress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr("Interacting!"));
		} else if(Input.GetKeyDown(KeyCode.I)) {
			Debug.Log("Sending int progress to " + activeQuest);
			if(activeQuest != null)
				activeQuest.progress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setNumber(1337));
		} 
	}

	/**
	 * Return true if quest has changed. False if, or no quest with ID was found, or quest was not changed. 
	 */
	bool enableQuest(int id) {
		if(activeQuestId != id) {
			foreach(Quest quest in questList) {
				if(quest == null) continue;

				if(quest.id == id) {
					// Disable the currently active quest
					if(activeQuest != null) activeQuest.disable();

					// Switch quest ID
					activeQuestId = id;
					PlayerPrefs.SetInt("active_quest", id);

					// Activate the new quest
					activeQuest = quest;
					activeQuest.enable();

					return true;
				}
			}
		}

		return false;
	}
}
