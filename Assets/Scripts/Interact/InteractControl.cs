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

		// Try to get the target interaction
		target = gameObject.GetComponent<Interaction>();
		if(target == null)
			target = gameObject.GetComponentInChildren<Interaction>();
	}
	
	void Update() {
		if(Input.GetKey(InputManager.interact) && !GameController.isPausedOrFocused())
			fill.fillAmount = (fill.fillAmount + Time.deltaTime * interactMultiplier) % 1; // Increment the fill amount. Reset after the number 1.
		else
			fill.fillAmount = Mathf.Lerp(fill.fillAmount, 0, Time.deltaTime * 10); // Decrement the fill amount back to 0

		// If the amount is over 95%, start the interaction
		if(fill.fillAmount >= .95f)
			target.doAction();
	}
}
