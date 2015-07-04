using UnityEngine;

public class Quest_01_01 : Quest {

	public Quest_01_01() : base("Your First Assignment", "Gather information about the shadows from the recon team!") { }

	public override void initStages() {
		stages.Add(new GoTo(LocationManager.npc_Yurippe));
		stages.Add(new TalkTo(GameController.npcManager.npc_Yurippe, "DialogueQ_01_01_1"));

		stages.Add(new GoTo(new Vector3(-17, 0, 0), "Reach the recon team"));
		stages.Add(new TalkTo(GameController.npcManager.npc_Yurippe, "DialogueQ_01_01_2"));

		stages.Add(new TalkTo(GameController.npcManager.npc_Yurippe, "DialogueQ_01_01_3", "Report back to <b>{0}</b>"));
	}

	public override void complete() {
		base.complete();

		QuestManager.getQuest("01_02").start();
	}
}
