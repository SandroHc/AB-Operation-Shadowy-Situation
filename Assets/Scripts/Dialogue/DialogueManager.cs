using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	public static Dialogue currentDialogue = null;

	public GameObject panelDialogue;
	private GameObject panelTalk;
	private GameObject panelSelection;

	public GameObject buttonPrefab;

	void Awake() {
		panelTalk = panelDialogue.transform.FindChild("talk").gameObject;
		panelSelection = panelDialogue.transform.FindChild("selection").gameObject;
	}

	void LateUpdate() {
		if(currentDialogue == null)
			return;

		currentDialogue.update();

		if(Input.GetKeyDown(InputManager.cancel))
			closeDialogue();
	}

	public void showDialogue(Dialogue dialogue) {
		Debug.Log("Opening dialogue " + dialogue);

		if(dialogue == null)
			return;
				
		panelDialogue.SetActive(true);

		GameController.setFocused(true, false);

		currentDialogue = dialogue;
		currentDialogue.show();
	}

	public void closeDialogue() {
		if(currentDialogue == null)
			return;

		//Debug.Log("Closing dialogue " + currentDialogue.ToString());

		// Send the event to the Dialogue class (to be able to rollback any changes)
		currentDialogue.close();
		// Null the reference to the dialogue
		currentDialogue = null;

		// Resume focus on the game
		GameController.setFocused(false);

		panelDialogue.gameObject.SetActive(false);
	}

	public void showTalk(Dialogue.DialogueTalk.Type type, string title, string text) {
		panelTalk.SetActive(true);
		panelSelection.gameObject.SetActive(false);

		switch(type) {
		case Dialogue.DialogueTalk.Type.PLAYER: title = "Player"; break;
		case Dialogue.DialogueTalk.Type.SYSTEM: title = "System"; break;
		}

		Text textComp = panelTalk.transform.FindChild("title").GetComponent<Text>();
		textComp.text = title;
		
		Color color = new Color((((int) type) >> 16) & 0xff, (((int) type) >> 8) & 0xff, (((int) type) >> 0) & 0xff);
		textComp.color = color;

		panelTalk.transform.FindChild("text").GetComponent<Text>().text = text;
	}

	public void showSelection(string[] options) {
		panelTalk.SetActive(false);
		panelSelection.SetActive(true);

		// Remove all previous added buttons 
		foreach(Transform child in panelSelection.transform)
			GameObject.Destroy(child.gameObject);

		for(int i=0; i < options.Length; i++) {
			generateButton(i, options[i]);
		}
	}

	private GameObject generateButton(int id, string str) {
		GameObject buttonObject = GameObject.Instantiate(buttonPrefab);
		buttonObject.name = "btn_selection_" + id;
		buttonObject.transform.SetParent(panelSelection.transform);


		RectTransform rect = buttonObject.GetComponent<RectTransform>();
		rect.localPosition = new Vector3(0, -(id * 35) + 67.5f, 0);


		Button button = buttonObject.GetComponent<Button>();
		button.onClick.AddListener(() => currentDialogue.selected(id));


		Text text = buttonObject.GetComponentInChildren<Text>();
		text.text = "<b>" + (id+1) + ".</b> " + str;	

		return buttonObject;
	}
}
