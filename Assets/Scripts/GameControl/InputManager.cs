using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	private static KeyCode[] validKeyCodes;

	public static KeyCode fire1;
	public static KeyCode fire2;
	public static KeyCode fire3;
	public static KeyCode reload;
	public static KeyCode interact;
	public static KeyCode jump;
	public static KeyCode sprint;
	public static KeyCode crawl;
	public static KeyCode submit;
	public static KeyCode cancel;
	public static KeyCode journal;
	public static KeyCode crafting;

	void Awake() {
		if(validKeyCodes == null)
			validKeyCodes = (KeyCode[]) System.Enum.GetValues(typeof(KeyCode));

		fire1 = (KeyCode) PlayerPrefs.GetInt("input_fire1", (int) KeyCode.Mouse0);
		fire2 = (KeyCode) PlayerPrefs.GetInt("input_fire2", (int) KeyCode.Mouse1);
		fire3 = (KeyCode) PlayerPrefs.GetInt("input_fire3", (int) KeyCode.Mouse2);
		reload = (KeyCode) PlayerPrefs.GetInt("input_reload", (int) KeyCode.R);
		interact = (KeyCode) PlayerPrefs.GetInt("input_interact", (int) KeyCode.E);
		jump = (KeyCode) PlayerPrefs.GetInt("input_jump", (int) KeyCode.Space);
		sprint = (KeyCode) PlayerPrefs.GetInt("input_sprint", (int) KeyCode.LeftShift);
		crawl = (KeyCode) PlayerPrefs.GetInt("input_crawl", (int) KeyCode.LeftControl);
		submit = (KeyCode) PlayerPrefs.GetInt("input_submit", (int) KeyCode.Return);
		cancel = (KeyCode) PlayerPrefs.GetInt("input_cancel", (int) KeyCode.Escape);
		journal = (KeyCode) PlayerPrefs.GetInt("input_quest_journal", (int) KeyCode.J);
		crafting = (KeyCode) PlayerPrefs.GetInt("input_crafting", (int) KeyCode.K);
	}

	public static void saveKey(string name, int value) {
		PlayerPrefs.SetInt("input_" + name, value);
	}

	public static KeyCode fetchKey() {
		for(int i = 0; i < validKeyCodes.Length; i++) {
			if(Input.GetKey(validKeyCodes[i]))
				return validKeyCodes[i];
		}
		return KeyCode.None;
	}

	private static List<InteractControl> interactList;

	public static void register(InteractControl obj) {
		if(interactList == null) interactList = new List<InteractControl>(1);

		interactList.Add(obj);
	}
}
