using System;

public class CommandHealthGet : CommandHealth {

	public CommandHealthGet() : base("get") { }

	public override string parse(string text) {
		return "Current health points: " + PlayerHP.health;
	}

	public override string use() {
		return "Syntax: " + name;
	}

	protected override void registerSubcommands() { }
}
