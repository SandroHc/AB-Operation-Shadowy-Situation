using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueTest : Dialogue {

	public override void generate() {
		addConversation(new Conversation1());
	}

	private class Conversation1 : Conversation {
		private Quest questTest;

		public Conversation1() {
			questTest = GameController.questManager.getQuest(1);
			string questTestName = questTest != null ? questTest.name : "INVALID_QUEST"; 
			if(questTest != null) {
				switch(questTest.getStatus()) {
				case Quest.QUEST_STATUS.INACTIVE:
					options.Add("Começar '" + questTestName + "'");
					break;
				case Quest.QUEST_STATUS.ACTIVE:
					options.Add("'" + questTestName + "' ja em progresso");
					break;
				case Quest.QUEST_STATUS.COMPLETED:
					options.Add("'" + questTestName + "' ja concluida");
					break;
				}
			} else {
				options.Add("Voltar");
			}
		}

		public override bool selected(int index) {
			if(questTest == null)
				return true;

			switch(index) {
			case 0:
				switch(questTest.getStatus()) {
				case Quest.QUEST_STATUS.INACTIVE:
						questTest.setStatus(Quest.QUEST_STATUS.ACTIVE);
					break;
				}
				break;
			}

			return true;
		}
	}
}