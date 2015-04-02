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
		Image img = panelDialogue.GetComponent<Image>();
		Color color = img.color;
		color.a = Mathf.Lerp(color.a, currentDialogue == null ? 0 : 1, Time.deltaTime * 10);
		img.color = color;


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

		// Hide the dialogue 1/2 second asterwards to allow the fading out animation
		Invoke("hideDialogue", .15f);
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

		int col = (int) type;
		Color color = new Color(((col >> 16) & 0xff) / 255f, ((col >> 8) & 0xff) / 255f, ((col >> 0) & 0xff) / 255f); // 0xRRGGBB
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

	private void hideDialogue() {
		panelDialogue.gameObject.SetActive(false);
	}
}
