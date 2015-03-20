using UnityEngine;
using System.Collections;

public class QuestProgress {
	public enum ProgressType { MONSTER_KILL, INTERACTION, MATERIAL_PICKUP, DIALOGUE, CUTSCENE, POSITION };
	public ProgressType type;

	private string str = "";
	private float number = 0;
	private Vector3 pos = Vector3.zero;

	public QuestProgress(ProgressType type) {
		this.type = type;
	}

	public string getStr() {
		return str;
	}

	public QuestProgress setStr(string str) {
		this.str = str;
		return this;
	}

	public float getNumber() {
		return number;
	}

	public QuestProgress setNumber(int number) {
		this.number = number;
		return this;
	}

	public Vector3 getPosition() {
		return pos;
	}

	public QuestProgress setPosition(Vector3 pos) {
		this.pos = pos;
		return this;
	}

	public override string ToString() {
		return "{ type=" + type.ToString() + "; str=\"" + str + "\"; number=" + number + "; pos=" + pos.ToString() + " }";
	}
}
