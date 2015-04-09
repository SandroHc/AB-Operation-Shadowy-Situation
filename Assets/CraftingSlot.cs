using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingSlot : MonoBehaviour {

	public string weaponName;
	private Weapon weapon;

	private Image img;
	private Text cost;

	private Button btn;
	private Text btnText;

	private Image damage;
	private Image rateOfFire;
	private Image range;

	public void setup() {
		weapon = WeaponManager.getWeapon(weaponName);

		img = transform.FindChild("img").GetComponent<Image>();
		cost = transform.FindChild("cost").GetComponent<Text>();
		btn = transform.FindChild("btn").GetComponent<Button>();
		btnText = btn.transform.FindChild("text").GetComponent<Text>();
		
		Transform stats = transform.FindChild("stats").transform;
		damage	   = stats.FindChild("damage").FindChild("fill").GetComponent<Image>();
		rateOfFire = stats.FindChild("rateOfFire").FindChild("fill").GetComponent<Image>();
		range 	   = stats.FindChild("range").FindChild("fill").GetComponent<Image>();

		Text name = transform.FindChild("name").GetComponent<Text>();
		name.text = weapon.name;

		Button button = transform.FindChild("btn").GetComponent<Button>();
		button.onClick.AddListener(() => btnClick());
	}

	public void show() {
		if(weapon == null) return;

		img = weapon.icon ?? GameController.spriteManager.weaponNoIcon;

		cost.text = weapon.getCost().ToString();

		damage.fillAmount = weapon.damage / WeaponManager.maxDamage;
		rateOfFire.fillAmount = weapon.getShootingCooldown() / WeaponManager.maxFireRate;
		range.fillAmount = weapon.range / WeaponManager.maxRange;

		updateBtnText();

		/*
		 * If locked:
		 * 		taint the panel red
		 * 		disable buttons
		 * 
		 */
	}

	private void btnClick() {
		if(weapon.isEquipped) // Refill ammo
			weapon.refillAmmo();
		else if(weapon.isCrafted) // Equip weapon
			WeaponManager.switchWeapon(weapon);
		else if(weapon.isUnlocked) // Craft the weapon
			weapon.craft();

		updateBtnText();
	}

	private void updateBtnText() {
		if(weapon.isEquipped)
			btnText.text = "Refill ammo";
		else if(weapon.isCrafted)
			btnText.text = "Equip";
		else if(weapon.isUnlocked)
			btnText.text = "Craft";
		else
			btnText.text = "LOCKED";
	}
}
