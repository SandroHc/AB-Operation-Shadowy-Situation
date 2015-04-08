using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingSlot : MonoBehaviour {

	public string weaponName;
	private Weapon weapon;

	private Image img;
	private Text cost;

	private Button btn;

	private Image damage;
	private Image rateOfFire;
	private Image range;

	public void setup() {
		weapon = WeaponManager.getWeapon(weaponName);

		img = transform.FindChild("img").GetComponent<Image>();
		cost = transform.FindChild("cost").GetComponent<Text>();
		btn = transform.FindChild("btn").GetComponent<Button>();
		
		Transform stats = transform.FindChild("stats").transform;
		damage	   = stats.FindChild("damage").FindChild("fill").GetComponent<Image>();
		rateOfFire = stats.FindChild("rateOfFire").FindChild("fill").GetComponent<Image>();
		range 	   = stats.FindChild("range").FindChild("fill").GetComponent<Image>();
	}

	public void show() {
		if(weapon == null) setup();

		if(weapon == null) return;

		img = weapon.getIcon();
		if(img == null) img = GameController.spriteManager.weaponNoIcon;

		cost.text = weapon.getCost().ToString();

		damage.fillAmount = weapon.getDamage() / WeaponManager.maxDamage;
		rateOfFire.fillAmount = weapon.getShootingCooldown() / WeaponManager.maxFireRate;
		range.fillAmount = weapon.getRange() / WeaponManager.maxRange;

		Text btnText = btn.transform.FindChild("text").GetComponent<Text>();
		if(weapon.isEquipped)
			btnText.text = "Refill ammo";
		else if(weapon.isCrafted)
			btnText.text = "Equip";
		else if(weapon.isUnlocked)
			btnText.text = "Craft";
		else
			btnText.text = "LOCKED";

		/*
		 * If locked:
		 * 		taint the panel red
		 * 		disable buttons
		 * 
		 * If unlocked:
		 * 		option to craft - FULL PRICE
		 * 
		 * If crafted:
		 * 		option to craft - 5-10% PRICE
		 * 
		 * If equipped:
		 * 		option to buy ammo
		 * 
		 */
	}
}
