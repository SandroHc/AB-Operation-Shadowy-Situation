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

	/**
	 * Return value will decide if the dialogue is closed.
	 * e.g. returning false will keep the dialogue (used to show more conversations, etc)
	 */
	public bool selected(int index) {
		Debug.Log("Dialogue: selected " + index);

		Conversation conv = getCurrentConversation();
		if(conv != null)
			return conv.selected(index);

		return true; // Default to returning true
	}

	public Conversation getCurrentConversation() {
		return list[conversationIndex];
	}

	protected void addConversation(Conversation conv) {
		list.Add(conv);
	}

	public void closingDialogue() { }

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

		public bool selected(int index) {
			return true;
		}
	}
}
