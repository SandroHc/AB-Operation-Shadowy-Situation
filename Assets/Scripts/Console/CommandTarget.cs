using System;

public class CommandTarget : Command {
	public CommandTarget() : base("target") { }
	public CommandTarget(string name) : base(name) { }

	protected override void registerSubcommands() {
		// Get all subcommands, and register them
		Type thisType = this.GetType();
		foreach(Type type in thisType.Assembly.GetTypes()) {
			if(type.BaseType == thisType)
				register(Activator.CreateInstance(type) as Command);
		}
	}
}
