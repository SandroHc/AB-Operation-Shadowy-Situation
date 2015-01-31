using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {
	private List<Quest> questList = new List<Quest>();
	
	public GameObject panelList;
	public GameObject panelDescription;

	void Awake() {
		questList.Clear();
		questList.Add(new QuestTest());

		foreach(Quest quest in questList) {
			GameObject buttonObject = new GameObject("btn_" + quest.id);

			/*Image image = buttonObject.AddComponent<Image>();
			image.transform.parent = panelList.transform;
			image.rectTransform.sizeDelta = new Vector2(180, 50);
			image.rectTransform.anchoredPosition = Vector3.zero;
			image.color = new Color(1f, .3f, .3f, .5f);*/
			Text text = buttonObject.AddComponent<Text>();
			text.text = quest.name;
			
			Button button = buttonObject.AddComponent<Button>();
			button.targetGraphic = text;//image;
			button.onClick.AddListener(() => Debug.Log("Button clicked!"));
		}
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Y)) {
			enableQuest(1);
		} else if(Input.GetKeyDown(KeyCode.U)) {
			foreach(Quest quest in questList)
				quest.progress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr("Interacting!"));
		} else if(Input.GetKeyDown(KeyCode.I)) {
			foreach(Quest quest in questList)
				quest.progress(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setNumber(1337));
		} 
	}

	/**
	 * Return true if the quest received the command to be enabled; false otherwise
	 */
	public bool enableQuest(int id) {
		Quest quest = getQuest(id);
		if(quest != null) {
			quest.enable();
			return true;
		} else {
			return false;
		}
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
}
 