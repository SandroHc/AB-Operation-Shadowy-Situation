using System;

public class CommandHealthGod : CommandHealth {

	public CommandHealthGod() : base("god") { }

	public override string parse(string text) {
		bool state = PlayerHP.god();
		return "God mode " + (state ? "enabled" : "disabled");
	}

	public override string use() {
		return "Syntax: " + name;
	}

	protected override void registerSubcommands() { }
}
