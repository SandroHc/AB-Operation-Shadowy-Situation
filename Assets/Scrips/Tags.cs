using UnityEngine;
using System.Collections;

public class Tags : MonoBehaviour {
	public const string gameController = "GameController";
	public const string player = "Player";
	public const string enemy = "enemy";
	public const string npc = "npc";

	// Ground types
	public const string groundWood = "groundWood";
	public const string groundGrass = "groundGrass";

	public const string walls = "walls"; // Tag to specify map objects not related to the ground

	public const string none = ""; // Untagged objects

	public static bool isGround(string tag) {
		return tag.Equals(groundWood) || tag.Equals(groundGrass);
	}
}
