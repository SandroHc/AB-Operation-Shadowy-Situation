using System;

public class CommandWeapon : Command {

	public CommandWeapon() {
		name = "weapon";
	}

	public override string parse(string text) {
		int i = text.IndexOf(' ');
		if(i >= 0) {
			string cmdName = text.Substring(0, i).ToUpper();
			if(cmdName.Equals("GETLIST")) {
				return getList();
			} else if(cmdName.Equals("UNLOCK")) {
				string sub = text.Substring(i + 1);
				Weapon obj = WeaponManager.getWeapon(sub);
				if(obj != null) {
					obj.unlock();
					return "Weapon \"" + sub + "\" unlocked.";
				} else
					return "Invalid weapon ID, " + sub;
			} else if(cmdName.Equals("CRAFT")) {
				string sub = text.Substring(i + 1);
				Weapon obj = WeaponManager.getWeapon(sub);
				if(obj != null) {
					obj.craft();
					return "Weapon \"" + sub + "\" crafted.";
				} else
					return "Invalid weapon ID, " + sub;
			} else if(cmdName.Equals("EQUIP")) {
				string sub = text.Substring(i + 1);
				Weapon obj = WeaponManager.getWeapon(sub);
				if(obj != null) {
					obj.equip();
					return "Weapon \"" + sub + "\" equipped.";
				} else
					return "Invalid weapon ID, " + sub;
			} else if(cmdName.Equals("RELOAD")) {
				string sub = text.Substring(i + 1);
				Weapon obj = WeaponManager.getWeapon(sub);
				if(obj != null) {
					obj.reload();
					return "Weapon \"" + sub + "\" reloaded.";
				} else
					return "Invalid weapon ID, " + sub;
			} else
				return name.ToUpper() + ": Unknown sub-command.";
		} else {
			string cmdName = text.ToUpper();

			if(cmdName.Equals("GETLIST")) {
				return getList();
			} else if(cmdName.Equals("UNLOCK") || cmdName.Equals("CRAFT") || cmdName.Equals("EQUIP") || cmdName.Equals("RELOAD")) {
				return name.ToUpper() + ": Wrong use. Use: " + cmdName + " <weapon id>";
			} else {
				return name.ToUpper() + ": Unknown sub-command. Supported: GETLIST, UNLOCK, CRAFT, EQUIP, RELOAD";
			}
		}
	}

	protected string getList() {
		return "Weapon list:\n - " + String.Join("\n - ", WeaponManager.getAllWeaponNames());
	}
}
