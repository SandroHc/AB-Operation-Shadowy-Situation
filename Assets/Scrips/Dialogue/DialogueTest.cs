using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueTest : Dialogue {

	public override void generate() {
		List<string> conv = new List<string>();
		conv.Add("Esta e uma das possiveis falas.");
		conv.Add("Esta e outra");

		addConversation(new Conversation(conv));
	}
}