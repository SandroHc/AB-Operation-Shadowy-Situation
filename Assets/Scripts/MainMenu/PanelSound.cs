using UnityEngine;
using UnityEngine.UI;

public class PanelSound : MonoBehaviour {
	public Slider masterVolume;
	public Slider effectsVolume;
	public Slider ambienceVolume;

	void Start() {
		masterVolume.value = PlayerPrefs.GetFloat("volume_master", 1);
		effectsVolume.value = PlayerPrefs.GetFloat("volume_effects", .75f);
		ambienceVolume.value = PlayerPrefs.GetFloat("volume_ambience", .25f);
	}

	public void masterVolumeChanged() {
		PlayerPrefs.SetFloat("volume_master", masterVolume.value);

		fireVolumeChangedEvents();
	}

	public void effectsVolumeChanged() {
		PlayerPrefs.SetFloat("volume_effects", effectsVolume.value);

		fireVolumeChangedEvents();
	}

	public void ambienceVolumeChanged() {
		PlayerPrefs.SetFloat("volume_ambience", ambienceVolume.value);

		fireVolumeChangedEvents();

		// Reload the soundtrack volume after the AudioManager has been updated
		SoundtrackController.ambienceVolumeChanged();
	}

	private void fireVolumeChangedEvents() {
		// Force the AudioManager class to reload all the volume settings
		AudioManager.load();
	}
}
