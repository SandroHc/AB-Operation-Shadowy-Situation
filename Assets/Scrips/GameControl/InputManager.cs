using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	private static KeyCode[] validKeyCodes;

	public static KeyCode fire1;
	public static KeyCode fire2;
	public static KeyCode fire3;
	public static KeyCode reload;
	public static KeyCode interact;
	public static KeyCode jump;
	public static KeyCode sprint;
	public static KeyCode crouch;
	public static KeyCode submit;
	public static KeyCode cancel;

	void Start() {
		if(validKeyCodes == null)
			validKeyCodes = (KeyCode[]) System.Enum.GetValues(typeof(KeyCode));

		fire1 = (KeyCode) PlayerPrefs.GetInt("input_fire1", (int) KeyCode.Mouse0);
		fire2 = (KeyCode) PlayerPrefs.GetInt("input_fire2", (int) KeyCode.Mouse1);
		fire3 = (KeyCode) PlayerPrefs.GetInt("input_fire3", (int) KeyCode.Mouse2);
		reload = (KeyCode) PlayerPrefs.GetInt("input_reload", (int) KeyCode.R);
		interact = (KeyCode) PlayerPrefs.GetInt("input_interact", (int) KeyCode.E);
		jump = (KeyCode) PlayerPrefs.GetInt("input_jump", (int) KeyCode.Space);
		sprint = (KeyCode) PlayerPrefs.GetInt("input_sprint", (int) KeyCode.LeftShift);
		crouch = (KeyCode) PlayerPrefs.GetInt("input_crouch", (int) KeyCode.LeftControl);
		submit = (KeyCode) PlayerPrefs.GetInt("input_submit", (int) KeyCode.Return);
		cancel = (KeyCode) PlayerPrefs.GetInt("input_cancel", (int) KeyCode.Escape);
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
}
