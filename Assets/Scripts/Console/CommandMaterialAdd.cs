using System;

public class CommandMaterialAdd : CommandMaterial {

	public CommandMaterialAdd() : base("add") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			int value;
			if(int.TryParse(text, out value)) {
				MaterialManager.increase(value);
				return "Materials incremented in " + value;
            } else {
				return "<b>" + text + "</b> is not a number.";
			}
		}
	}

	public override string use() {
		return "Syntax: " + name + " <number>";
	}

	protected override void registerSubcommands() { }
}
