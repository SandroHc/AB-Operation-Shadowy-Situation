using System;
using UnityEngine;

public class CommandEnemyKillAll : CommandEnemy {

	public CommandEnemyKillAll() : base("killall") { }

	public override string parse(string text) {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.enemy);
		EnemyHP temp;
		foreach(GameObject obj in enemies) {
			temp = obj.GetComponent<EnemyHP>();
			if(temp != null) temp.Died();
		}

        return "Killed " + enemies.Length + " enemies.";
	}

	public override string use() {
		return "Syntax: " + name;
	}

	protected override void registerSubcommands() { }
}
