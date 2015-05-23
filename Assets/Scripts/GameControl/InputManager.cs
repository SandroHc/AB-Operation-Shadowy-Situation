using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	private static KeyCode[] validKeyCodes;

	public static Dictionary<string, InputKey> keyList;

	void Start() {
		if(validKeyCodes == null)
			validKeyCodes = (KeyCode[]) System.Enum.GetValues(typeof(KeyCode));

		if(keyList == null) {
			keyList = new Dictionary<string, InputKey>();

			addKey("Fire 1", "fire1", KeyCode.Mouse0);
			addKey("Fire 2", "fire2", KeyCode.Mouse1);
			addKey("Fire 3", "fire3", KeyCode.Mouse2);
			addKey("Reload", "reload", KeyCode.R);
			addKey("Interact", "interact", KeyCode.E);
			addKey("Jump", "jump", KeyCode.Space);
			addKey("Sprint", "sprint", KeyCode.LeftShift);
			addKey("Crawl", "crawl", KeyCode.LeftControl);
			addKey("Submit", "submit", KeyCode.Return);
			addKey("Cancel", "cancel", KeyCode.Escape);
			addKey("Quest Journal", "quest_journal", KeyCode.J);
			addKey("Crafting", "crafting", KeyCode.K);
		}
	}

	void Awake() {
		// Reload the keys
		/*if(keyList.Count > 0) {
			foreach(Dictionary<string, InputKey> key in keyList)
				key.Value.load();
		}*/
	}

	public static KeyCode fetchKey() {
		for(int i = 0; i < validKeyCodes.Length; i++) {
			if(Input.GetKey(validKeyCodes[i]))
				return validKeyCodes[i];
		}
		return KeyCode.None;
	}

	public static void addKey(string label, string name, KeyCode defaultKey) {
		InputKey obj = new InputKey();

		obj.label = label;
		obj.name = name;
		obj.defaultKey = defaultKey;

		obj.load();

		keyList.Add(name, obj);
	}

	public static KeyCode getKey(string key) {
		if(keyList != null && keyList.ContainsKey(key))
			return keyList[key].keyCode;
		return KeyCode.None;
	}

	public static bool getKeyDown(string key) {
		return Input.GetKeyDown(getKey(key));
    }

	private static List<InteractControl> interactList;

	public static void register(InteractControl obj) {
		if(interactList == null) interactList = new List<InteractControl>(1);

		interactList.Add(obj);
	}

	public class InputKey {
		public string label;
		public string name;
		public KeyCode keyCode;

		public KeyCode defaultKey;

		public void load() {
			keyCode = (KeyCode) PlayerPrefs.GetInt("input_" + name, (int) defaultKey);
		}

		public void save() {
			PlayerPrefs.SetInt("input_" + name, (int) keyCode);
		}
	}
}
