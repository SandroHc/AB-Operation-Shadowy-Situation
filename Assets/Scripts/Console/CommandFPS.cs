using System;

public class CommandFPS : Command {
	public CommandFPS() : base("fps") { }
	public CommandFPS(string name) : base(name) { }

	public override string parse(string text) {
		text = text.ToUpper();

		if(text.StartsWith("ON")) {
			GameController.fpsCounter.enabled = true;

			return "FPS counter is now ON";
		} else if(text.StartsWith("OFF")) {
			// Disabled teh counter and clears the text from the label
			GameController.fpsCounter.display.text = "";
			GameController.fpsCounter.enabled = false;

			return "FPS counter is now OFF";
		} else {
			return "<color=orange>Only ON or OFF.</color>";
		}
	}

	protected override void registerSubcommands() {	}
}
