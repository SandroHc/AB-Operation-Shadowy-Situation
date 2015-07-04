using System;

public class CommandNotice : Command {
	public CommandNotice() : base("notice") { }
	public CommandNotice(string name) : base(name) { }

	public override string parse(string text) {
		NoticeManager.addNotice(text);
		return "Notice <b>" + text + "</b> added";
	}

	protected override void registerSubcommands() {	}
}
