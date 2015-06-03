using System;

public class CommandQuestList : CommandQuest {

	public CommandQuestList() : base("getlist") { }

	public override string parse(string text) {
		return getList();
	}

	public override string use() {
		return "Syntax: " + name;
	}

	protected string getList() {
		return "Quest list:\n - " + String.Join("\n - ", QuestManager.getAllQuestNames());
	}

	protected override void registerSubcommands() { }
}
