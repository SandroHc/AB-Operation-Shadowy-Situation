using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	public enum Type { Dialogue, ItemPickUp, MaterialPickUp }
	public Type type;			// Type of the interaction

	public new string name;		// Name of the final object of the interaction (like Door, NPC, ...)

	public float cooldown = 1f; // 1 second cooldown
	private bool isInCooldown = false;
	private float currentCooldown;

	/** Dialogue **/
	public string dialogue;

	/** Material pickup **/
	public int picksMax = 3;
	private float picksRemaining;
	private float currentStage = 3;
	
	private MeshFilter meshFilter;

	void Awake() {
		switch(type) {
		case Type.MaterialPickUp:
			// Try to obtain the MeshFilter
			meshFilter = gameObject.GetComponentInParent<MeshFilter>();
			// Reset the picks available back to the maximum
			picksRemaining = picksMax;
			break;
		}
	}

	void Update() {
		if(isInCooldown) {
			if(currentCooldown > 0) {
				currentCooldown -= Time.deltaTime;
			} else { // If the cooldown... cooled down, break the link
				isInCooldown = false;

				InteractDistance interact = GetComponent<InteractDistance>();
				if(interact != null) interact.cooldown = false;
			}
		}

		switch(type) {
		case Type.MaterialPickUp:
			if(picksRemaining < picksMax)
				picksRemaining += .4f * Time.deltaTime;
			
			if(picksRemaining != currentStage) {
				if(meshFilter != null) {
					float stagePerPick = picksMax / 3; // Number of stages

					if(picksRemaining >= stagePerPick * 3)
						meshFilter.mesh = GameController.materialManager.stage3;
					else if(picksRemaining >= stagePerPick * 2)
						meshFilter.mesh = GameController.materialManager.stage2;
					else if(picksRemaining >= stagePerPick)
						meshFilter.mesh = GameController.materialManager.stage1;
					else
						meshFilter.mesh = GameController.materialManager.stageDepleted;
				}
				
				currentStage = picksRemaining;
			}
			break;
		}
	}

	public void doAction() {
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr(this.name).setPosition(gameObject.transform.position));

		switch(type) {
		case Type.Dialogue:
			GameController.dialogueManager.showDialogue(System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(dialogue) as Dialogue);
			break;
		case Type.ItemPickUp:
			// Not yet implemented
			break;
		case Type.MaterialPickUp:			
			if(picksRemaining >= 1) {
				picksRemaining--;
				GameController.materialManager.pickUp();
			}
			break;
		}

		isInCooldown = true;
		currentCooldown = cooldown;

		InteractDistance interact = GetComponent<InteractDistance>();
		if(interact != null) interact.cooldown = true;
	}
}
