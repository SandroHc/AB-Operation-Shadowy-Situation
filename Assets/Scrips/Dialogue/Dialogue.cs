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

	public abstract class Conversation {
		protected List<string> options = new List<string>();

		public List<string> get() {
			return options;
		}

		public int getSize() {
			return options.Count;
		}

		public void draw() {
			int size = options.Count;
			
			int btnWidth = 400;
			int btnHeight = 30;
			
			int padding = Screen.width / 2 - btnWidth / 2;
			int paddingVert = 8;

			int posY = 0;

			for(int i = size-1; i >= 0; i--) {
				posY += paddingVert + btnHeight;

				if(GUI.Button(new Rect(padding, Screen.height - posY, btnWidth, btnHeight), new GUIContent((i + 1) + ". " + options[i])))
					selected(i);
			}
		}

		/**
		 * Returns true if the dialogue is over (e.g. no more conversations)
		 **/
		abstract public bool selected(int index);
	}
}
