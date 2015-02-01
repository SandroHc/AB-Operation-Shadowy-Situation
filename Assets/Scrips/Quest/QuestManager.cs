using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {
	private List<Quest> questList = new List<Quest>();

	public GameObject panelJournal;
	public GameObject panelList;
	public GameObject panelDescription;
	public GameObject panelDescriptionStages;

	void Awake() {
		questList.Clear();
		registerQuest(new QuestTest());

		// Get the panel's RectTransform to get it's width
		RectTransform panelListTransform = panelList.GetComponent<RectTransform>();

		foreach(Quest quest in questList) {
			GameObject buttonObject = new GameObject("btn_" + quest.id);

			Image image = buttonObject.AddComponent<Image>();
			image.transform.parent = panelList.transform;
			image.rectTransform.sizeDelta = new Vector2(panelListTransform.rect.width, 50);
			image.rectTransform.anchoredPosition = Vector3.zero;
			image.color = new Color(1f, 1f, 1f, .5f);
			
			Button button = buttonObject.AddComponent<Button>();
			button.targetGraphic = image;

			GameObject textObject = new GameObject("text");
			textObject.transform.parent = buttonObject.transform;
			Text text = textObject.AddComponent<Text>();
			text.rectTransform.sizeDelta = Vector2.zero;
			text.rectTransform.anchorMin = Vector2.zero;
			text.rectTransform.anchorMax = Vector2.one;
			text.rectTransform.anchoredPosition = new Vector2(.5f, .5f);
			text.text = quest.name;
			text.font = GameController.textManager.font;
			text.fontSize = 20;
			text.color = Color.black;
			text.alignment = TextAnchor.MiddleCenter;

			button.onClick.AddListener(() => showInfo(getQuest(quest.id)));
		}
	}

	public void LateUpdate() {
		if(Input.GetKeyDown(KeyCode.U))
			sendProgress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr("Interacting!"));
		else if(Input.GetKeyDown(KeyCode.I))
			sendProgress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setNumber(1337));

		// If the Joural button os pressed, show it!
		if(Input.GetKeyDown(InputManager.journal)) {
			if(!GameController.isPausedOrFocused()) {
				GameController.setFocused(true, false);
				panelJournal.SetActive(true);
			}
		}

		// If the Journal is visible and the Cancel button is pressed, close the Journal.
		if(panelJournal.activeSelf && Input.GetKeyDown(InputManager.cancel)) {
			GameController.setFocused(false);
			panelJournal.SetActive(false);
		}
	}

	private bool registerQuest(Quest quest) {
		if(quest == null) {
			Debug.Log("Tried to register invalid quest.");
			return false;
		}

		foreach(Quest obj in questList) {
			if(obj != null && obj.id == quest.id) {
				Debug.Log("Quest " + quest.name + " and " + obj.name + " both have the same id , " + quest.id + ". Ignoring.");
				return false;
			}
		}

		questList.Add(quest);
		return true;
	}

	public void sendProgress(QuestProgress progress) {
		foreach(Quest quest in questList) {
			if(quest == null) continue;
			quest.progress(progress);
		}			
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
		return true;
	}

	/**
	 * Returns the Quest with that ID.
	 * If no quest with that ID is found, returns null.
	 **/
	private Quest getQuest(int id) {
		foreach(Quest quest in questList) {
			if(quest == null)
				continue;
			else if(quest.id == id)
				return quest;
		}
		return null;
	}

	public Text panelDescriptionName;
	public Text panelDescriptionDesc;
	public Text panelDescriptionStatus;
	public Text panelDescriptionStage;

	/**
	 * Populate the Journal panel with information about the desired quest
	 **/
	private void showInfo(Quest quest) {
		if(quest == null) return;// TODO Clear UI quest description, to clear the content of the previous selected quest

		// if is the first time showing quest info, activate the respective panel
		if(!panelDescription.activeSelf)
			panelDescription.SetActive(true);

		// Populate the labels with information about this Quest
		panelDescriptionName.text = quest.name;
		panelDescriptionDesc.text = quest.description;
		panelDescriptionStatus.text = quest.getStatus().ToString();
		panelDescriptionStage.text = quest.getCurrentStage().ToString();

		// Delete any outdated stages from other Quest
		foreach(Transform child in panelDescriptionStages.transform)
			GameObject.Destroy(child.gameObject);

		int index = 0;
		foreach(Quest.Stage stage in quest.getStages()) {
			GameObject textObject = new GameObject("stage_" + index + "_text");
			textObject.transform.parent = panelDescriptionStages.transform;
			Text text = textObject.AddComponent<Text>();
			text.rectTransform.sizeDelta = Vector2.zero;
			text.rectTransform.anchorMin = Vector2.zero;
			text.rectTransform.anchorMax = Vector2.one;
			text.rectTransform.anchoredPosition = new Vector2(.5f, .5f);
			text.text = stage.getText();
			text.font = GameController.textManager.font;
			text.fontSize = 20;
			text.color = Color.black;
			text.alignment = TextAnchor.MiddleCenter;

			index++;
		}


		// TODO For debug purposes
		debugCurrentQuest = quest;
	}

	private Quest debugCurrentQuest = null;

	public void debugBtn() {
		if(debugCurrentQuest == null) return;

		debugCurrentQuest.setStatus(debugCurrentQuest.getStatus() == Quest.QUEST_STATUS.ACTIVE ? Quest.QUEST_STATUS.INACTIVE : Quest.QUEST_STATUS.ACTIVE);
		panelDescriptionStatus.text = debugCurrentQuest.getStatus().ToString();
	}
}
 