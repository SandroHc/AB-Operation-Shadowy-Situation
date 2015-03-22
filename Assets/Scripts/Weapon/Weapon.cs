using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {
	protected string name;

	protected float damage = 1f;
	protected float range = 50f;
	
	protected float cooldownShoot = .3f;
	protected float cooldownReload = 1f;

	protected float recoil = .1f;

	protected int defaultMagazines = 2;
	protected int defaultMaxAmmunition = 10;

	protected int currentMagazines;
	protected int currentAmmunition;

	protected AudioClip[] sounds;
	protected enum SoundLabel { SHOOT, SHOOT_NO_AMMO, RELOAD };

	public Weapon() {
		currentMagazines = defaultMagazines;
		currentAmmunition = defaultMaxAmmunition;
	}

	public void targetHit(GameObject target, RaycastHit hit) {
		target.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
	}

	public bool eventShoot() {
		if(currentAmmunition > 0) {
			currentAmmunition--;

			playSound(SoundLabel.SHOOT);
			return true;
		} else {
			playSound(SoundLabel.SHOOT_NO_AMMO);
			return false;
		}
	}
	
	public bool eventReload() {
		if(currentMagazines > 1) {
			currentMagazines--;
			currentAmmunition = defaultMaxAmmunition;

			playSound(SoundLabel.RELOAD);
			return true;
		} else {
			return false;
		}
	}

	public void eventAimEnter() {

	}

	public void eventAimExit() {

	}

	private void playSound(SoundLabel label) {
		AudioClip clip = sounds[(int) label];
		if(clip != null)
			GetComponent<AudioSource>().PlayOneShot(clip);
	}

	public float getDamage() {
		return damage;
	}

	public float getRange() {
		return range;
	}
}
