using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue1Quest1 : Dialogue {

	public Dialogue1Quest1() : base(new Conversation1()) {
		// NO-OP
	}
	
	private class Conversation1 : DialogueSelection {
		public Conversation1() {
			options.Add("Opçao 1");
			options.Add("Opçao 2");
			options.Add("Opçao 3");
		}
		
		public override bool selected(int index) {
			return true;
		}
	}
}