using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Dialogue {
	protected List<Conversation> list = new List<Conversation>();
	protected int conversationIndex = 0;

	public Dialogue() {
		generate();
	}

	abstract public void generate();

	public void selected(int index) {
		Debug.Log("Dialogue: selected " + index);

		Conversation conv = getCurrentConversation();
		if(conv != null)
			conv.selected(index);
	}

	public Conversation getCurrentConversation() {
		return list[conversationIndex];
	}

	protected void addConversation(Conversation conv) {
		list.Add(conv);
	}

	public class Conversation {
		private List<string> options = new List<string>();

		public Conversation(List<string> options) {
			this.options = options;
		}

		public List<string> get() {
			return options;
		}

		public int getSize() {
			return options.Count;
		}

		public void selected(int index) { }
	}
}
