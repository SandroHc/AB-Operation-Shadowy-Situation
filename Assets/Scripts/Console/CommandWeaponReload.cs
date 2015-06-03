using System;

public class CommandWeaponReload : CommandWeapon {

	public CommandWeaponReload() : base("reload") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			Weapon obj = WeaponManager.getWeapon(text);
			if(obj != null) {
				obj.reload();
				return "Weapon \"" + text + "\" reloaded.";
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
