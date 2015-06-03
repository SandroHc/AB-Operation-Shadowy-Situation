using System;

public class CommandRespawn : Command {

	public CommandRespawn() : base("respawn") { }

	public override string parse(string text) {
		GameController.playerController.respawn();
		return "Player respawned";
	}

	protected override void registerSubcommands() { }
}
