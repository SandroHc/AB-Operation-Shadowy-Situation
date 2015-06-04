using UnityEngine;

public class AudioManager : MonoBehaviour {
	public enum TYPE { EFFECTS, AMBIENCE }

	private static float masterVolume;
	private static float effectsVolume;
	private static float ambienceVolume;

	public AudioClip footstepWood;
	public AudioClip footstepGrass;
	public AudioClip footstepMetal;
	public AudioClip footstepWater;

	public AudioClip waterFlowing;
	public AudioClip waterFlowingLoud;

	public AudioClip weapon_pistolShoot;
	public AudioClip weapon_pistolShootNoAmmo;
	public AudioClip weapon_pistolReload;

	public AudioClip weapon_grenadeThrow;
	public AudioClip weapon_grenadeExplode;

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

	public static void playOn(AudioSource source, AudioClip clip, TYPE type) {
		float volume = 1f;
		switch(type) {
		case TYPE.AMBIENCE:	volume = getAmbienceVolume(); break;
		case TYPE.EFFECTS:	volume = getEffectsVolume(); break;
		}

		//float oldVolume = source.volume;

		source.volume = volume;
		source.PlayOneShot(clip);

		//source.volume = oldVolume;
	}
}
