using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingSlot : MonoBehaviour {

	public string weaponName;
	private Weapon weapon;

	private Image img;
	private Text cost;

	private Image damage;
	private Image rateOfFire;
	private Image range;
	
	void Start() {
		weapon = WeaponManager.getWeapon(weaponName);
	}

	void Akawe() {
		img = transform.FindChild("img").GetComponent<Image>();
		cost = transform.FindChild("cost").GetComponent<Text>();

		Transform stats = transform.FindChild("stats").transform;
		damage	   = stats.FindChild("damage").FindChild("fill").GetComponent<Image>();
		rateOfFire = stats.FindChild("rateOfFire").FindChild("fill").GetComponent<Image>();
		range 	   = stats.FindChild("range").FindChild("fill").GetComponent<Image>();
	}

	void show() {
		img = weapon.getIcon();
		if(img == null) img = GameController.spriteManager.weaponNoIcon;

		damage.fillAmount = weapon.getDamage() / WeaponManager.maxDamage;
		rateOfFire.fillAmount = weapon.getShootingCooldown() / WeaponManager.maxFireRate;
		range.fillAmount = weapon.getRange() / WeaponManager.maxRange;

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
