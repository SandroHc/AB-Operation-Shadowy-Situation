using System;

public class CommandWeaponEquip : CommandWeapon {

	public CommandWeaponEquip() : base("equip") { }

	public override string parse(string text) {
		if(String.Empty.Equals(text)) {
			return " <color=red>Invalid parameter.</color> " + use();
        } else {
			Weapon obj = WeaponManager.getWeapon(text);
			if(obj != null) {
				obj.equip();
				return "Weapon \"" + text + "\" equipped.";
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
