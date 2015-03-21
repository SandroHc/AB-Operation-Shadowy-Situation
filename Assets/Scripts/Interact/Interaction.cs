using UnityEngine;
using System.Collections;

abstract public class Interaction : MonoBehaviour {
	public enum Type { Dialogue, PickUp, Custom }
	protected Type type;		// Type of the interaction

	public new string name;		// Name of the final object of the interaction (like Door, NPC, ...)

	abstract public void doAction();

	public Type getType() {
		return type;
	}
}
