using System;

public class CommandWeaponList : CommandWeapon {

	public CommandWeaponList() : base("getlist") { }

	public override string parse(string text) {
		return getList();
	}

	public override string use() {
		return "Syntax: " + name;
	}

	protected string getList() {
		return "Weapon list:\n - " + String.Join("\n - ", WeaponManager.getAllWeaponNames());
	}

	protected override void registerSubcommands() { }
}
