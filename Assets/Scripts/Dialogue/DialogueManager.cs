using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	public static Dialogue currentDialogue = null;

	public GameObject panelDialogue;
	private GameObject panelTalk;
	private GameObject panelSelection;

	public GameObject dialogueButtonPrefab;

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

		if(InputManager.getKeyDown("cancel"))
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

		// Hide the dialogue 1/2 second afterwards to allow the fading out animation
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

		// Remove all previously added buttons 
		foreach(Transform child in panelSelection.transform)
			Object.Destroy(child.gameObject);

		RectTransform sel = panelSelection.GetComponent<RectTransform>();

		int relativeHeight = options.Length * 30 + (options.Length-1) * 5;
		int initialPos;

		// Invert the value because negative numbers are "down", and positive ones are "up" in the UI position
		if(relativeHeight > sel.rect.height)
			initialPos = (int) (relativeHeight - sel.rect.height) + (options.Length) * 5;
		else
			initialPos = (int) ((sel.rect.height - relativeHeight) / 2);

		GameObject[] list = new GameObject[options.Length];

		for(int i=0; i < options.Length; i++) {
			list[i] = generateButton(i, options[i], initialPos);
			initialPos -= 35; // 30px by height; 5px by padding
		}

		// Set the largest width to the whole list
		setPreferredWidth(list, 200);
	}

	private GameObject generateButton(int id, string str, int y) {
		GameObject buttonObject = Object.Instantiate(dialogueButtonPrefab);
		buttonObject.name = "btn_selection_" + id;
		buttonObject.transform.SetParent(panelSelection.transform);


		RectTransform rect = buttonObject.GetComponent<RectTransform>();
		rect.localPosition = new Vector3(0, y, 0);


		Button button = buttonObject.GetComponent<Button>();
		button.onClick.AddListener(() => currentDialogue.selected(id));


		Text text = buttonObject.GetComponentInChildren<Text>();
		text.text = "<b>" + (id+1) + ".</b> " + str;

		return buttonObject;
	}

	private void setPreferredWidth(GameObject[] list, float minWidth) {
		float largest = minWidth;

		// Get the largest preferred width
		for(int i=0; i < list.Length; i++) {
			Text text = list[i].GetComponentInChildren<Text>();
			if(text != null && text.preferredWidth > largest)
				largest = text.preferredWidth;

			ContentSizeFitter fitter = list[i].GetComponentInChildren<ContentSizeFitter>();
			if(fitter != null) fitter.enabled = false;
		}

		// Apply the preferred width to all options
		for(int i=0; i < list.Length; i++) {
			RectTransform rt = list[i].GetComponent<RectTransform>();
			if(rt != null) RectTransformExtensions.SetWidth(rt, largest + 20); // Left and right paddings are 10px; so sum 20
		}
	}

	private void hideDialogue() {
		panelDialogue.gameObject.SetActive(false);
	}
}
