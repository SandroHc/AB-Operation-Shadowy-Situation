
public abstract class Command {
	public string name { get; set; }

	public abstract string parse(string text);
}