[System.Serializable]
public interface IQuest {
	void enable();
	void disable();
	void progress(QuestProgress progress);
}
