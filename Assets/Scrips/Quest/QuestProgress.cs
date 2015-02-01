using UnityEngine;
using System.Collections;

public class QuestProgress {
	public enum ProgressType { MONSTER_KILL, INTERACTION, MATERIAL_PICKUP };
	public ProgressType type;

	public string str = "";
	public int number = 0;
	public Vector3 pos = Vector3.zero;

	public QuestProgress(ProgressType type) {
		this.type = type;
	}

	public QuestProgress setStr(string str) {
		this.str = str;
		return this;
	}

	public QuestProgress setNumber(int number) {
		this.number = number;
		return this;
	}

	public QuestProgress setPosition(Vector3 pos) {
		this.pos = pos;
		return this;
	}

	public override string ToString() {
		return "{ type=" + type.ToString() + "; str=\"" + str + "\"; number=" + number + "; pos=" + pos.ToString() + " }";
	}
}
