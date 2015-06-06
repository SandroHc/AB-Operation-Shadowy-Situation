using System;

public class CommandMaterialRem : CommandMaterial {

	public CommandMaterialRem() : base("rem") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			int value;
			if(int.TryParse(text, out value)) {
				MaterialManager.decrease(value);
				return "Materials decremented in " + value;
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
