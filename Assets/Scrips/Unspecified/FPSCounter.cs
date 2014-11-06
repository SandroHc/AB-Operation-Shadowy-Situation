using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour {
	
	// Attach this to a GUIText to make a frames/second indicator.
	//
	// It calculates frames/second over each updateInterval,
	// so the display does not keep changing wildly.
	//
	// It is also fairly accurate at very low FPS counts (<10).
	// We do this not by simply counting frames per interval, but
	// by accumulating FPS for each frame. This way we end up with
	// correct overall FPS even if the interval renders something like
	// 5.5 frames.

	private Text display;
	
	public float updateInterval = 0.5F;
	public byte decimals = 0;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeLeft; // Left time for current interval
	
	void Start() {
		display = GetComponent<Text>();
		if(display == null) {
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}
		timeLeft = updateInterval;  
	}
	
	void Update() {
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if(timeLeft <= 0) {
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("{0:F" + decimals + "} FPS",fps);
			display.text = format;
			
			if(fps < 30)
				display.color = Color.yellow;
			else if(fps < 10)
				display.color = Color.red;
			else
				display.color = Color.green;

			timeLeft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}