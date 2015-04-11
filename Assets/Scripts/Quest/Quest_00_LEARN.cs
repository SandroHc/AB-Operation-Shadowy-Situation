using UnityEngine;
using System.Collections;

public class Quest_00_LEARN : Quest {

	public Quest_00_LEARN() : base(1, "Learning the Ropes", "This is the description for the test quest that was setup just to test the... Quest System") { }

	public override void initStages() {
		Interaction npc = GameObject.FindGameObjectWithTag(Tags.npc).GetComponent<Interaction>();

		stages.Add(new GoTo(new Vector3(0,0,0)));
		stages.Add(new TalkTo(npc, "DialogueQ_00_LEARN_1"));
		stages.Add(new Collect("quest_" + id + "_collect", 10));
		stages.Add(new TalkTo_1(npc, "DialogueQ_00_LEARN_2"));
		stages.Add(new Craft(WeaponManager.getWeapon("Scissors")));
	}

	protected class TalkTo_1 : TalkTo {

		public TalkTo_1(Interaction npcScript, string dialogueClass) : base(npcScript, dialogueClass) {	}

		public override void finish() {
			base.finish();

			// Unlock the Scissors after this dialogue
			Weapon weapon = WeaponManager.getWeapon("Scissors");
			if(weapon != null) weapon.unlock();
		}
	}
}
