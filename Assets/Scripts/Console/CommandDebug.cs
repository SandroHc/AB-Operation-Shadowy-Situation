using System;

public class CommandDebug : Command {

	public CommandDebug() {
		name = "debug";
	}

	public override string parse(string text) {
		return "Debug command.";
	}
}
