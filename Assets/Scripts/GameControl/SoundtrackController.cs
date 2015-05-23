using UnityEngine;

public class SoundtrackController : MonoBehaviour {
	public AudioClip[] soundtracks;
	private int current = -1;

	private static AudioSource source;

	void Start() {
		if(source == null)
			source = GetComponent<AudioSource>();
		if(source == null)
			gameObject.AddComponent<AudioSource>();
		
		// Set the volume according to the settings (also, applying master volume)
		source.volume = AudioManager.getAmbienceVolume();

		// Initiate the loop
		nextClip();
	}

	void Awake() {
		// Called before the "Start" event...
	}

	private void nextClip() {
		// Update the clip and play it
		source.clip = getRandomClip();
		source.Play();

		// Invoke this method again after the music ends to play a new one
		Invoke("nextClip", source.clip.length);
	}

	private AudioClip getRandomClip() {
		if(soundtracks.Length < 1) // If the array is empty, ignore
			return null;
		else if(soundtracks.Length == 1) // If onlt one element, send him
			return soundtracks[0];

		// Get a random index NOT equal to the previous one
		int index;
		do {
			index = Random.Range(0, soundtracks.Length);
		} while(index == current);

		current = index;
		return soundtracks[index];
	}

	public static void ambienceVolumeChanged() {
		// Update the AudioSource volume!
		if(source != null)
			source.volume = AudioManager.getAmbienceVolume();
	}
}
