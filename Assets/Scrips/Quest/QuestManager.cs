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

		RectTransform panelListTransform = panelList.GetComponent<RectTransform>();

		foreach(Quest quest in questList) {
			Debug.Log("Adding quest " + quest.name + " to Journal");
			GameObject buttonObject = new GameObject("btn_" + quest.id);

			Image image = buttonObject.AddComponent<Image>();
			image.transform.parent = panelList.transform;
			image.rectTransform.sizeDelta = new Vector2(panelListTransform.rect.width, 50);
			image.rectTransform.anchoredPosition = Vector3.zero;
			image.color = new Color(1f, 1f, 1f, .5f);
			
			Button button = buttonObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(() => showInfo(quest.id));

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
		}
	}

	public void LateUpdate() {
		if(Input.GetKeyDown(KeyCode.U))
			sendProgress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr("Interacting!"));
		else if(Input.GetKeyDown(KeyCode.I))
			sendProgress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setNumber(1337));


		if(Input.GetKeyDown(InputManager.journal)) {
			if(!GameController.isPausedOrFocused()) {
				GameController.setFocused(true, false);
				panelJournal.SetActive(true);
			}
		}

		if(panelJournal.activeSelf && Input.GetKeyDown(InputManager.cancel)) {
			GameController.setFocused(false);
			panelJournal.SetActive(false);
		}
	}

	private bool registerQuest(Quest quest) {
		foreach(Quest obj in questList) {
			if(obj.id == quest.id) {
				Debug.Log("Quest " + quest.name + " and " + obj.name + " both have the same id , " + quest.id + ". Ignoring.");
				return false;
			}
		}

		questList.Add(quest);
		return true;
	}

	public void sendProgress(QuestProgress progress) {
		foreach(Quest quest in questList) {
			if(quest == null)
				continue;

			quest.progress(progress);
		}			
	}

	/**
	 * Return true if the quest received the command to be enabled; false otherwise
	 */
	public bool enableQuest(int id) {
		Quest quest = getQuest(id);
		if(quest == null)
			return false;

		quest.setActive(true);
		return true;
	}

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

	private void showInfo(int id) {
		Quest quest = getQuest(id);

		if(quest == null) // TODO Clear UI quest description, to clear the content of the previous selected quest
			return;

		// if is the first time showing quest info, activate the respective panel
		if(!panelDescription.activeSelf)
			panelDescription.SetActive(true);

		panelDescriptionName.text = quest.name;
		panelDescriptionDesc.text = quest.description;
		panelDescriptionStatus.text = quest.getActive() ? "active" : quest.getCompleted() ? "completed" : "not active";
		panelDescriptionStage.text = quest.getCurrentStage().ToString();

		foreach(Transform child in panelDescriptionStages.transform)
			GameObject.Destroy(child.gameObject);


		// TODO For debug purposes
		debugCurrentQuest = id;
	}

	private int debugCurrentQuest = -1;

	public void debugBtn() {
		if(debugCurrentQuest < 0)
			return;

		Quest quest = getQuest(debugCurrentQuest);

		if(quest == null)
			return;

		quest.setActive(!quest.getActive());
		panelDescriptionStatus.text = quest.getActive() ? "active" : quest.getCompleted() ? "completed" : "not active";
	}
}
 