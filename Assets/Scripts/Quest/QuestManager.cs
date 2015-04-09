﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {
	private List<Quest> questList;

	public GameObject panelJournal;
	public RectTransform panelList;
	public GameObject panelDescription;
	public GameObject panelDescriptionStages;

	public Text checkpointText;

	public GameObject questButtonPrefab;

	void Awake() {
		if(questList == null) {
			questList = new List<Quest>();
			registerQuests();
		}

		foreach(Quest quest in questList) {
			generateButton(quest);
		}

		// Populate the checkpoint list
		updateCheckpoints();
	}

	public void LateUpdate() {
		// If the Joural button is pressed, show it!
		if(Input.GetKeyDown(InputManager.journal) && !GameController.isPausedOrFocused()) {
			GameController.setFocused(true, false);
			panelJournal.SetActive(true);
		}

		// If the Journal is visible and the Cancel button is pressed, close the Journal.
		if(panelJournal.activeSelf && Input.GetKeyDown(InputManager.cancel)) {
			GameController.setFocused(false);
			panelJournal.SetActive(false);
		}

		// TODO Debug code
		if(Input.GetKeyDown(KeyCode.Alpha9)) {
			getQuest(1).reset();
		}
	}

	private void registerQuests() {
		registerQuest(new Quest_00_LEARN());
	}

	private bool registerQuest(Quest quest) {
		if(quest == null) {
			Debug.Log("Tried to register invalid quest.");
			return false;
		}

		foreach(Quest obj in questList) {
			if(obj.id == quest.id) {
				Debug.Log("Quest " + quest.name + " and " + obj.name + " both have the same id , " + quest.id + ". Ignoring.");
				return false;
			}
		}

		questList.Add(quest);
		return true;
	}

	public void fireProgressEvent(QuestProgress progress) {
		if(progress.type != QuestProgress.ProgressType.INTERACTION)
			Debug.Log("Firing event: " + progress.ToString());

		foreach(Quest quest in questList)
			quest.progress(progress);
	}

	/**
	 * Return true if the quest received the command to be enabled; false otherwise
	 */
	public bool enableQuest(Quest quest) {
		if(quest == null) return false;

		// No need to update quests already completed
		if(quest.status == Quest.QUEST_STATUS.COMPLETED)
			return false;

		quest.setStatus(Quest.QUEST_STATUS.ACTIVE);

		// Fire the event
		questStartedEvent(quest);

		return true;
	}

	/**
	 * Returns the Quest with that ID.
	 * If no quest with that ID is found, returns null.
	 **/
	public Quest getQuest(int id) {
		foreach(Quest quest in questList) {
			if(quest.id == id)
				return quest;
		}
		return null;
	}

	public Quest.QUEST_STATUS getQuestStatus(int id) {
		Quest target = getQuest(id);
		return target != null ? target.getStatus() : Quest.QUEST_STATUS.UNKNOWN;
	}

	public Text panelDescriptionName;
	public Text panelDescriptionDesc;
	public Text panelDescriptionStatus;
	public Text panelDescriptionStage;

	/**
	 * Populate the Journal panel with information about the desired quest
	 **/
	private void showInfo(Quest quest) {
		if(quest == null) return;// TODO Clear UI quest description, to clear the content of the previously selected quest

		// if is the first time showing quest info, activate the respective panel
		if(!panelDescription.activeSelf)
			panelDescription.SetActive(true);

		// Populate the labels with information about this Quest
		panelDescriptionName.text = quest.name;
		panelDescriptionDesc.text = quest.description;
		panelDescriptionStatus.text = quest.getStatus().ToString();
		panelDescriptionStage.text = quest.currentStage.ToString();

		// Delete any outdated stages from other Quest
		foreach(Transform child in panelDescriptionStages.transform)
			GameObject.Destroy(child.gameObject);

		int index = 0;
		foreach(Quest.Stage stage in quest.stages) {
			GameObject textObject = new GameObject("stage_" + index + "_text");
			textObject.transform.parent = panelDescriptionStages.transform;
			Text text = textObject.AddComponent<Text>();
			text.rectTransform.sizeDelta = Vector2.zero;
			text.rectTransform.anchorMin = Vector2.zero;
			text.rectTransform.anchorMax = Vector2.one;
			text.rectTransform.anchoredPosition = new Vector2(.5f, .5f);
			text.text = stage.getText();
			text.font = GameController.textManager.uiFont;
			text.fontSize = 20;
			text.color = Color.black;
			text.alignment = TextAnchor.MiddleCenter;

			index++;
		}


		// TODO For debug purposes
		debugCurrentQuest = quest;
	}

	private GameObject generateButton(Quest quest) {
		GameObject go = Object.Instantiate(questButtonPrefab);
		go.name = "quest_" + quest.id;
		go.transform.SetParent(panelList);
		
		RectTransform rt = go.GetComponent<RectTransform>();
		RectTransformExtensions.SetDefaultScale(rt);
		//RectTransformExtensions.SetWidth(rt, width);
		//RectTransformExtensions.SetLeftTopPosition(rt, pos);

		Text text = go.transform.FindChild("text").GetComponent<Text>();
		text.text = quest.name;

		Button button =  go.GetComponent<Button>();
		button.onClick.AddListener(() => showInfo(quest));

		return go;
	}

	public void updateCheckpoints() {
		//Debug.Log ("Updating checkpoint list");

		string str = "";

		foreach(Quest quest in questList) {
			if(quest.getStatus() == Quest.QUEST_STATUS.ACTIVE) {
				str += "<b>" + quest.name + "</b>\n";

				List<Quest.Stage> stages = quest.stages;
				for(int i=0; i < quest.currentStage; i++)
					str += "  <color=green>✓</color> " + stages[i].getText() + "\n";
			
				str += "  <color=red>✗</color> " + stages[quest.currentStage].getText() + "\n\n";
			}
		}

		checkpointText.text = str;
	}

	public void stageUpdateEvent(Quest.Stage stage) {
		updateCheckpoints();
	}

	public void questStartedEvent(Quest quest) {
		Debug.Log ("Quest started: " + quest.name + " (" + quest.id + ")");
		updateCheckpoints();
	}

	public void questFinishedEvent(Quest quest) {
		Debug.Log ("Quest finished: " + quest.name + " (" + quest.id + ")");
		updateCheckpoints();
	}

	public void setWaypoint(Vector3 position) {
		GameController.playerPathfind.setDestination(position);
	}

	private Quest debugCurrentQuest = null;

	public void debugBtn() {
		if(debugCurrentQuest == null) return;

		debugCurrentQuest.setStatus(debugCurrentQuest.getStatus() == Quest.QUEST_STATUS.ACTIVE ? Quest.QUEST_STATUS.INACTIVE : Quest.QUEST_STATUS.ACTIVE);
		panelDescriptionStatus.text = debugCurrentQuest.getStatus().ToString();
	}
}
 