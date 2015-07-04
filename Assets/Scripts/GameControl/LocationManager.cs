using UnityEngine;

public class LocationManager : MonoBehaviour {
	public static Vector3 npc_Yurippe;

	void Awake() {
		npc_Yurippe = GameController.npcManager.npc_Yurippe.gameObject.transform.position;
    }
}
