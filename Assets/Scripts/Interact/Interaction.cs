﻿using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	public enum Type { Dialogue, ItemPickUp, MaterialPickUp }
	public Type type;			// Type of the interaction

	public new string name;		// Name of the final object of the interaction (like Door, NPC, ...)

	/** Dialogue **/
	public Dialogue dialogue;

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
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr(name).setPosition(gameObject.transform.position));

		switch(type) {
		case Type.Dialogue:
			GameController.dialogueManager.showDialogue(new DialogueTest());
			break;
		case Type.ItemPickUp: // Not yet implemented
			break;
		case Type.MaterialPickUp:			
			if(picksRemaining >= 1) {
				picksRemaining--;
				GameController.materialManager.pickUp();
			}
			break;
		}
	}
}
