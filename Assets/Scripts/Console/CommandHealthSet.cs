using System;

public class CommandHealthSet : CommandHealth {

	public CommandHealthSet() : base("set") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			float value;
			if(float.TryParse(text, out value)) {
				PlayerHP.health = value;
				return "Health points set to " + value;
            } else {
				return "<b>" + text + "</b> is not a number.";
			}
		}
	}

	public override string use() {
		return "Syntax: " + name + " <health points>";
	}

	protected override void registerSubcommands() { }
}
