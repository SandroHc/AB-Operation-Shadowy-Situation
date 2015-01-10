using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueTest : Dialogue {

	public override void generate() {
		List<string> conv = new List<string>();
		conv.Add("Elemento 1.");
		conv.Add("O que achas?");

		addConversation(new Conversation(conv));
	}
}