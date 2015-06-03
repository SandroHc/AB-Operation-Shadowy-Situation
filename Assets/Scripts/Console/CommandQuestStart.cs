using System;

public class CommandQuestStart : CommandQuest {

	public CommandQuestStart() : base("start") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			Quest obj = QuestManager.getQuest(text);
			if(obj != null) {
				obj.reset();
				return "Quest \"" + obj.name + "\" (" + text + ") started.";
			} else {
				return "Invalid quest ID, " + text;
			}
		}
	}

	public override string use() {
		return "Syntax: " + name + " <quest id>";
	}

	protected override void registerSubcommands() { }
}
