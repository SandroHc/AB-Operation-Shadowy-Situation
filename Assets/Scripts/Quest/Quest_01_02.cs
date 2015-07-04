using UnityEngine;

public class Quest_01_02 : Quest {

	public Quest_01_02() : base("The Unified Reverter", "Help the team assemble the machine to revert the shadows back to their original form.") { }

	public override void initStages() {
		stages.Add(new GoTo(LocationManager.npc_Yurippe));
		stages.Add(new TalkTo(GameController.npcManager.npc_Yurippe, "DialogueQ_01_02_1"));

		stages.Add(new GoTo(new Vector3(-169, -30, 30), "Go to the river."));
		stages.Add(new ItemPick("Flowers", 5, "quest-" + id + "-collect"));

		stages.Add(new TalkTo(GameController.npcManager.npc_Yurippe, "DialogueQ_01_02_2", "Return the flowers to <b>{0}</b>"));
	}

	public override void complete() {
		base.complete();

		QuestManager.getQuest("01_03").start();
	}
}
