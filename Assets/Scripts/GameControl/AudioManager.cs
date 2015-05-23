using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	private static float masterVolume;
	private static float effectsVolume;
	private static float ambienceVolume;

	public AudioClip footstepWood;
	public AudioClip footstepGrass;
	public AudioClip footstepMetal;
	public AudioClip footstepWater;

	public AudioClip waterFlowing;
	public AudioClip waterFlowingLoud;

	public AudioClip weaponPistolShoot;
	public AudioClip weaponPistolShootNoAmmo;
	public AudioClip weaponPistolReload;

	void Start() {
		load();
	}

	public static void load() {
		masterVolume = PlayerPrefs.GetFloat("volume_master", 1);
		effectsVolume = PlayerPrefs.GetFloat("volume_effects", .75f);
		ambienceVolume = PlayerPrefs.GetFloat("volume_ambience", .25f);
	}

	public static float getMasterVolume() {
		return masterVolume;
	}

	public static float getEffectsVolume() {
		return effectsVolume * masterVolume;
	}

	public static float getAmbienceVolume() {
		return ambienceVolume * masterVolume;
	}
}
