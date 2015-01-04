using UnityEngine;
using System.Collections;

/**
 * Utility class to store information related to any quest.
 */
public abstract class Quest : MonoBehaviour {
	public int id;
	public new string name;
	public string description;

	public Quest(int id, string name, string description) {
		this.id = id;
		this.name = name;
		this.description = description;
	}

	abstract public void enable();

	abstract public void disable();

	abstract public void progress(QuestProgress progress);
}
