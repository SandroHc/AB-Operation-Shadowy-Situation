using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue1Quest1 : Dialogue {
	
	public override void generate() {
		addConversation(new Conversation1());
	}
	
	private class Conversation1 : Conversation {
		public Conversation1() {
			options.Add("Opçao 1");
			options.Add("Opçao 2");
			options.Add("Opçao 3");
		}
		
		public override bool selected(int index) {
			Debug.Log ("Selected: " + index);
			
			return true;
		}
	}
}