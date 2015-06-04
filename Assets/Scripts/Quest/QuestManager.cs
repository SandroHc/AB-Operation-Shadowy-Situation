using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;

public class QuestManager : MonoBehaviour {
	private static Dictionary<string, Quest> quests;

	public GameObject panelJournal;
	public RectTransform panelList;
	public RectTransform panelDesc;

	public Text checkpointText;

	public GameObject questButtonPrefab;

	void Awake() {
		initQuests();

		// Populate the quest list
		updateQuestButtons();

		// Populate the checkpoint list
		updateCheckpoints();
	}

	public void LateUpdate() {
		// If the Joural button is pressed, show it!
		if(!GameController.isPausedOrFocused() && InputManager.getKeyDown("journal")) {
			GameController.setFocused(true, false);
			panelJournal.SetActive(true);
		}

		// If the Journal is visible and the Cancel button is pressed, close the Journal.
		if(panelJournal.activeSelf && InputManager.getKeyDown("cancel")) {
			GameController.setFocused(false);
			panelJournal.SetActive(false);
		}
	}

	public static void initQuests() {
		if(quests == null) {
			quests = new Dictionary<string, Quest>();

			// Get all subclasses of Quest, and register them
			foreach(Type type in typeof(Quest).Assembly.GetTypes()) {
				if(type.IsSubclassOf(typeof(Quest))) {
					register(Activator.CreateInstance(type) as Quest);
				}
			}
		}
	}

	/**
	 * Register the following quest to the pool.
	 */
	private static bool register(Quest quest) {
		if(quest == null) {
			Debug.Log("Tried to register invalid quest.");
			return false;
		}

		if(quests.ContainsKey(quest.id)) {
			Quest duplicate = quests[quest.id];

			Debug.Log("Can't register quest \"" + quest.name + "\". The ID \"" + quest.id + "\" is already registered to \"" + duplicate.name + "\". Ignoring.");
			return false;
		} else {
			quests.Add(quest.id, quest);
			return true;
		}
	}

	public void fireProgressEvent(QuestProgress progress) {
		if(quests == null) {
			//Debug.Log("Event not fired because the quest list has not been initialized yet. (" + progress + ")");
			return;
		}

		// TODO Prevent floading the debug console
		if(progress.type != QuestProgress.Type.INTERACTION) Debug.Log("Firing event: " + progress);

		foreach(KeyValuePair<string, Quest> quest in quests)
			quest.Value.progress(progress);
	}

	/**
	 * Return true if the quest received the command to be enabled; false otherwise
	 */
	public bool enableQuest(Quest quest) {
		if(quest == null) return false;

		// No need to update quests already completed
		if(quest.status == Quest.STATUS.COMPLETED)
			return false;

		quest.setStatus(Quest.STATUS.ACTIVE);

		// Fire the event
		questStartedEvent(quest);

		return true;
	}

	/**
	 * Returns the Quest with that ID.
	 * If no quest with that ID is found, returns null.
	 **/
	public static Quest getQuest(string id) {
		if(quests != null && quests.ContainsKey(id))
			return quests[id];
		else
			return null;
	}

	public static Quest[] getAllQuests() {
		Quest[] list = new Quest[quests.Count];

		quests.Values.CopyTo(list, 0);

		return list;
	}

	public static string[] getAllQuestNames() {
		string[] list = new string[quests.Count];

		quests.Keys.CopyTo(list, 0);

		return list;
	}

	public Quest.STATUS getQuestStatus(string id) {
		Quest target = getQuest(id);
		return target != null ? target.getStatus() : Quest.STATUS.UNKNOWN;
	}

	private Text panelDescName;
	private Text panelDescDesc;
	private Text panelDescStageList;

	private Quest currentInfo;

	/**
	 * Populate the Journal panel with information about the desired quest
	 **/
	private void showInfo(Quest quest) {
		currentInfo = quest;

		// This way, if the Quest is invalid, the whole panel is hidden. Also hiding previous information from other quest.
		panelDesc.gameObject.SetActive(quest != null);

		if(quest == null) return;

		if(panelDescName == null) {
			panelDescName = panelDesc.FindChild("name").FindChild("text").GetComponent<Text>();
			panelDescDesc = panelDesc.FindChild("description").GetComponent<Text>();
			panelDescStageList = panelDesc.FindChild("stage_list").GetComponent<Text>();
		}

		// Populate the labels with information about this Quest
		panelDescName.text = quest.name;
		panelDescDesc.text = quest.description;

		updateQuestDescription();
	}

	private void updateQuestDescription() {
		if(currentInfo == null) return;

		StringBuilder sb = new StringBuilder();
		if(currentInfo.status == Quest.STATUS.COMPLETED) {
			foreach(Quest.Stage stage in currentInfo.stages)
				sb.Append("  <color=green>✓</color> ").Append(stage.getText()).Append("\n");
		} else {
			for(int i=0; i < currentInfo.currentStage; i++)
				sb.Append("  <color=green>✓</color> ").Append(currentInfo.stages[i].getText()).Append("\n");
			sb.Append("  <color=red>✗</color> ").Append(currentInfo.stages[currentInfo.currentStage].getText());
		}
		
		panelDescStageList.text = sb.ToString();
	}

	private void updateQuestButtons() {
		foreach(Transform child in panelList) {
			if(child.GetComponent<Text>() != null)
				continue;
			else
				Destroy(child.gameObject);
		}

		// Separate the quest into two lists.
		List<Quest> active = new List<Quest>(quests.Count / 2);
		List<Quest> completed = new List<Quest>(quests.Count / 2);
		
		foreach(KeyValuePair<string, Quest> quest in quests) {
			switch(quest.Value.status) {
			case Quest.STATUS.ACTIVE: 	 active.Add(quest.Value); break;
			case Quest.STATUS.COMPLETED: completed.Add(quest.Value); break;
			}
			
		}

		float xPos = -62.5f;
		float yPos = 50; // The initial Y pos equals height of the label
		
		// Setup first the ACTIVE quests
		foreach(Quest quest in active) {
			generateButton(quest, new Vector2(xPos, -yPos));
			yPos += 50;
		}
		
		// Set the height value for the "Completed" label
		RectTransformExtensions.SetPositionOfPivot(panelList.FindChild("finished").GetComponent<RectTransform>(), new Vector2(xPos, -yPos));
		yPos += 50;
		
		// And finally set the COMPLETED quests
		foreach(Quest quest in completed) {
			generateButton(quest, new Vector2(xPos, -yPos));
			yPos += 50;
		}
		
		//RectTransformExtensions.SetHeight(panelList, -yPos);

		// Clear both lists. Not sure if this is necessary, as it may just be collected by GC later.
		active.Clear();
		completed.Clear();
	}

	private GameObject generateButton(Quest quest, Vector2 pos) {
		GameObject go = Instantiate(questButtonPrefab);
		go.name = "quest_" + quest.id;
		go.transform.SetParent(panelList);
		
		RectTransform rt = go.GetComponent<RectTransform>();
		RectTransformExtensions.SetDefaultScale(rt);
		RectTransformExtensions.SetSize(rt, new Vector2(125, 50));
		RectTransformExtensions.SetPositionOfPivot(rt, pos);

		Text text = go.transform.FindChild("text").GetComponent<Text>();
		text.text = quest.name;

		Button button =  go.GetComponent<Button>();
		button.onClick.AddListener(() => showInfo(quest));

		return go;
	}

	public void updateCheckpoints() {
		//Debug.Log ("Updating checkpoint list");

		StringBuilder sb = new StringBuilder();

		foreach(KeyValuePair<string, Quest> quest in quests) {
			if(quest.Value.getStatus() == Quest.STATUS.ACTIVE) {
				sb.Append("<b>").Append(quest.Value.name).Append("</b>\n");
				for(int i=0; i < quest.Value.currentStage; i++)
					sb.Append("  <color=green>✓</color> ").Append(quest.Value.stages[i].getText()).Append("\n");
				sb.Append("  <color=red>✗</color> ").Append(quest.Value.stages[quest.Value.currentStage].getText());
			}
		}

		checkpointText.text = sb.ToString();
	}

	public void stageUpdateEvent(Quest.Stage stage) {
		updateCheckpoints();
		updateQuestDescription();
	}

	public void questStartedEvent(Quest quest) {
		Debug.Log ("Quest started: " + quest.name + " (" + quest.id + ")");
		updateCheckpoints();
		updateQuestButtons();
	}

	public void questFinishedEvent(Quest quest) {
		Debug.Log ("Quest finished: " + quest.name + " (" + quest.id + ")");
		updateCheckpoints();
		updateQuestButtons();
	}

	public void setWaypoint(Vector3 position) {
		GameController.playerPathfind.setDestination(position);
	}
}
 