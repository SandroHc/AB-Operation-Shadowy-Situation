using System;

public class CommandWeaponUnlock : CommandWeapon {

	public CommandWeaponUnlock() : base("unlock") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
		} else {
			Weapon obj = WeaponManager.getWeapon(text);
			if(obj != null) {
				obj.unlock();
				return "Weapon \"" + text + "\" unlocked.";
			} else {
				return "Invalid weapon ID, " + text;
			}
		}
	}

	public override string use() {
		return "Syntax: " + name + " <weapon id>";
	}

	protected override void registerSubcommands() { }
}
