using UnityEngine;
using System.Collections;

public class Quest_00_LEARN : Quest {

	public Quest_00_LEARN() : base("00_LEARN", "Learning the Ropes", "This quest teaches the basic mechanics of the game.") { }

	public override void initStages() {
		Interaction npc = GameObject.FindGameObjectWithTag(Tags.npc).GetComponent<Interaction>();

		stages.Add(new GoTo(new Vector3(0,0,0)));
		stages.Add(new TalkTo(npc, "DialogueQ_00_LEARN_1"));
		stages.Add(new Collect("quest-" + id + "-collect", 10));
		stages.Add(new TalkTo_Craft(npc, "DialogueQ_00_LEARN_2"));
		stages.Add(new Craft(WeaponManager.getWeapon("M9"))); // Weapon: Scissors
	}

	protected class TalkTo_Craft : TalkTo {

		public TalkTo_Craft(Interaction npcScript, string dialogueClass) : base(npcScript, dialogueClass) {	}

		public override void finish() {
			base.finish();

			// Unlock the Scissors after this dialogue
			Weapon weapon = WeaponManager.getWeapon("M9"); // Weapon: Scissors
			if(weapon != null) weapon.unlock();
		}
	}
}
