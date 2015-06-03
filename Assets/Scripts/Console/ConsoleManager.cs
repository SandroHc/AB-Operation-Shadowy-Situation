using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour {
	public static ConsoleManager INSTANCE;

	public static Dictionary<string, Command> commands;
	public static Queue<string> logs;
	public static int maxLogs = 20;

	public GameObject ui;

	public InputField input;
	public Text output;

	private bool isShowing = false;
	
	void Start() {
		if(!Debug.isDebugBuild)
			gameObject.SetActive(false);

		INSTANCE = this;

		commands = new Dictionary<string, Command>();
		// Get all subclasses of Command, and register them
		foreach(Type type in typeof(Command).Assembly.GetTypes()) {
			if(type.BaseType == typeof(Command))
				register(Activator.CreateInstance(type) as Command);
		}

		logs = new Queue<string>(maxLogs);
	}

	void Update() {
		if(isShowing) {
			if(Input.GetKeyDown(KeyCode.Escape))	hide();
		} else {
			if(Input.GetKeyDown(KeyCode.Backslash))	show();
		}
	}

	private void show() {
		ui.SetActive(true);

		GameController.setFocused(true, false);

		// Focus the InputField
		EventSystem.current.SetSelectedGameObject(input.gameObject, null);
		input.OnPointerClick(new PointerEventData(EventSystem.current));

		isShowing = true;
	}

	private void hide() {
		ui.SetActive(false);

		GameController.setFocused(false, true);

		isShowing = false;
	}

	public void onCommand() {
		parse(input.textComponent.text);

		updateOutput();

		// Focus the InputField
		EventSystem.current.SetSelectedGameObject(input.gameObject, null);
		input.OnPointerClick(new PointerEventData(EventSystem.current));
	}

	public static void add(string text) {
		string[] parts = text.Split('\n');
		for(int i = 0; i < parts.Length; i++)
			logs.Enqueue(parts[i]);
	}

	public void updateOutput() {
		while(logs.Count > maxLogs)
			logs.Dequeue();

		string[] arr = logs.ToArray();

		StringBuilder sb = new StringBuilder(maxLogs * 2 - 1); // One space for each line and other for line breaks (excluding the last line)
		for(int i = 0 ; i < arr.Length; i++) {
			sb.Append(arr[i]);
			if(i + 1 < arr.Length)
				sb.Append("\n");
		}

		output.text = sb.ToString();
    }

	public void parse(string cmd) {
		int i = cmd.IndexOf(' ');
		if(i >= 0) {
			string cmdName = cmd.Substring(0, i).ToUpper();
			if(commands.ContainsKey(cmdName)) {
				string cmdText = cmd.Substring(i + 1);
				add(commands[cmdName].parse(cmdText));
			} else
				add("Unknown command. Supported commands: " + String.Join(", ", getList()));
		} else {
			string cmdName = cmd.ToUpper();
			if(commands.ContainsKey(cmdName))
				add(commands[cmdName].parse(String.Empty));
			else
				add("Unknown command. Supported commands: " + String.Join(", ", getList()));
		}
	}

	private static void register(Command cmd) {
		if(!commands.ContainsKey(cmd.name))
			commands.Add(cmd.name, cmd);
	}

	private static string[] getList() {
		string[] list = new string[commands.Count];

		commands.Keys.CopyTo(list, 0);

		return list;
	}
}