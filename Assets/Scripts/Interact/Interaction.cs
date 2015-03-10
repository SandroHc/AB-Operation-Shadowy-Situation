using UnityEngine;
using System.Collections;

abstract public class Interaction : MonoBehaviour {
	public enum Type { Dialogue, PickUp, Custom }
	public Type type;			// Type of the interaction

	public string destName;		// Name of the final object of the interaction (like Door, NPC, ...)

	public float minDistance;	// Minimum distance required to interact

	abstract public void doAction(GameObject player);
}
