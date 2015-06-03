using System;

public class CommandDebug : Command {

	public CommandDebug() : base("debug") { }

	public override string parse(string text) {
		return "Debug command.";
	}

	protected override void registerSubcommands() {
		// Get all subcommands, and register them
		Type thisType = this.GetType();
		foreach(Type type in thisType.Assembly.GetTypes()) {
			if(type.BaseType == thisType)
				register(Activator.CreateInstance(type) as Command);
		}
	}
}
