
using System;
using System.Collections.Generic;

public abstract class Command {
	Dictionary<string, Command> subcommands = new Dictionary<string, Command>();

	public string name { get; private set; }

	public Command(string name) {
		this.name = name.ToUpper();

		registerSubcommands();
    }		

	public virtual string parse(string text) {
		int i = text.IndexOf(' ');

		string cmd;
		string param = String.Empty;
		if(i < 0) {
			cmd = text.ToUpper();
		} else {
			cmd = text.Substring(0, i).ToUpper();
			param = text.Substring(i + 1);
        }

		if(subcommands.ContainsKey(cmd))
			return subcommands[cmd].parse(param);
		else
			return "<color=orange>Unknown sub-command.</color>\n" + use();
	}

	public virtual string use() {
		string[] list = new string[subcommands.Count];
		subcommands.Keys.CopyTo(list, 0);

		return "List of available sub-commands: " + String.Join(", ", list);
	}

	protected abstract void registerSubcommands();

	protected void register(Command cmd) {
		if(!subcommands.ContainsKey(cmd.name))
			subcommands.Add(cmd.name, cmd);
	}
}