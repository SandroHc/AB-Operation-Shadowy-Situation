using UnityEngine;

public class Quest_00_01 : Quest {

	public Quest_00_01() : base("Learning the Ropes", "This quest teaches the basic mechanics of the game.") { }

	public override void initStages() {
		stages.Add(new GoTo(LocationManager.npc_Yurippe));
		stages.Add(new TalkTo(GameController.npcManager.npc_Yurippe, "DialogueQ_00_01_1"));
		stages.Add(new MaterialPick("quest-" + id + "-collect", 10));

		Weapon obj = WeaponManager.getWeapon("M9"); // Weapon: Scissors
		stages.Add(new TalkTo_Craft(GameController.npcManager.npc_Yurippe, "DialogueQ_00_01_2", obj));
		stages.Add(new Craft(obj));
	}

	public override void complete() {
		base.complete();

		Quest quest = QuestManager.getQuest("01_01");
        quest.start();
	}

	/* After the dialogue is finished, unlock the specified weapon */
	protected class TalkTo_Craft : TalkTo {
		protected Weapon weapon;

		public TalkTo_Craft(Interaction npcScript, string dialogueClass, Weapon weapon) : base(npcScript, dialogueClass) {
			this.weapon = weapon;
		}

		public override void finish() {
			base.finish();

			// Unlock the Scissors after this dialogue
			if(weapon != null) weapon.unlock();
		}
	}
}
