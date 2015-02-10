using UnityEngine;
using System.Collections;

public class Tags : MonoBehaviour {
	public const string gameController	= "GameController";
	public const string player			= "Player";
	public const string enemy			= "enemy";
	public const string npc				= "npc";

	// Ground types
	public const string groundWood		= "groundWood";
	public const string groundGrass		= "groundGrass";
	public const string groundMetal		= "groundMetal";
	public const string groundWater		= "groundWater";

	// Tag to specify map objects not related to the ground
	// Wall types (used to use dynamic bullet hole types depending on the wall)
	public const string wallConcrete	= "wallConcrete";
	public const string wallGlass		= "wallGlass";
	public const string wallMetal		= "wallMetal";
	public const string wallWood		= "wallWood";

	public const string rubbish			= "rubbish";

	public const string cutsceneCamera 	= "cutsceneCamera";

	public const string none = ""; // Untagged objects

	public static bool isGround(string tag) {
		return tag.Equals(groundWood) || tag.Equals(groundGrass) || tag.Equals(groundMetal) || tag.Equals(groundWater);
	}
}
