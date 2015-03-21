using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractControl : MonoBehaviour {
	public Text key;
	public Image background;
	public Image fill;

	public Interaction target;
	public float interactMultiplier = 1f;

	void Awake() {
		key.text = InputManager.interact.ToString();

		Debug.Log (InputManager.interact);
	}
	
	void Update() {
		if(Input.GetKey(InputManager.interact) && !GameController.isPausedOrFocused()) {
			fill.fillAmount = (fill.fillAmount + Time.deltaTime * interactMultiplier) % 1; 
		} else {
			fill.fillAmount = Mathf.Lerp(fill.fillAmount, 0, Time.deltaTime * 10);
		}

		if(fill.fillAmount >= .95f) {
			target.doAction();
		}
	}
}
