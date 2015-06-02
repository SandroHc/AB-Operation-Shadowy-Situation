using System;

public class CommandRespawn : Command {

	public CommandRespawn() {
		name = "respawn";
	}

	public override string parse(string text) {
		GameController.playerController.respawn();
		return "Player respawned.";
	}
}
