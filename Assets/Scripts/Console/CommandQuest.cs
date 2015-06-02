using System;

public class CommandQuest : Command {

	public CommandQuest() {
		name = "quest";
	}

	public override string parse(string text) {
		int i = text.IndexOf(' ');
		if(i >= 0) {
			string cmdName = text.Substring(0, i).ToUpper();
			if(cmdName.Equals("GETLIST")) {
				return getList();
			} else if(cmdName.Equals("START")) {
				string sub = text.Substring(i + 1);
				Quest obj = QuestManager.getQuest(sub);
				if(obj != null) {
					obj.reset();
					return "Quest \"" + obj.name + "\" (" + sub + ") started.";
				} else
					return "Invalid quest ID, " + sub;
			} else if(cmdName.Equals("FINISH")) {
				string sub = text.Substring(i + 1);
				Quest obj = QuestManager.getQuest(sub);
				if(obj != null) {
					obj.complete();
					return "Quest \"" + obj.name + "\" (" + sub + ") finished.";
				} else
					return "Invalid quest ID, " + sub;
			} else
				return name.ToUpper() + ": Unknown sub-command.";
		} else {
			string cmdName = text.ToUpper();

			if(cmdName.Equals("GETLIST")) {
				return getList();
			} else if(cmdName.Equals("START") || cmdName.Equals("FINISH")) {
				return name.ToUpper() + ": Wrong use. Use: " + cmdName + " <quest id>";
			} else {
				return name.ToUpper() + ": Unknown sub-command. Supported: GETLIST, START, FINISH";
			}
		}
	}

	protected string getList() {
		return "Quest list:\n - " + String.Join("\n - ", QuestManager.getAllQuestNames());
	}
}
