using System;

public class CommandMaterial : Command {
	public CommandMaterial() : base("material") { }
	public CommandMaterial(string name) : base(name) { }

	protected override void registerSubcommands() {
		// Get all subcommands, and register them
		Type thisType = this.GetType();
		foreach(Type type in thisType.Assembly.GetTypes()) {
			if(type.BaseType == thisType)
				register(Activator.CreateInstance(type) as Command);
		}
	}
}
