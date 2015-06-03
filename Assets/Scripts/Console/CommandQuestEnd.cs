using System;

public class CommandQuestEnd : CommandQuest {

	public CommandQuestEnd() : base("finish") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			Quest obj = QuestManager.getQuest(text);
			if(obj != null) {
				obj.complete();
				return "Quest \"" + obj.name + "\" (" + text + ") finished.";
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
