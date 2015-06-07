using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

	/** WEAPONS **/
	public GameObject weapon_M9;
	public GameObject weapon_M16;
	public GameObject weapon_Railgun;
	public GameObject weapon_grenade;
	public GameObject weapon_grenade_object; // Used when throwing

	/** PARTICLES **/
	public GameObject particles_fireworks;
	public GameObject particles_fire;
	public GameObject particles_wildfire;
	public GameObject particles_smoke;
	public GameObject particles_Steam;
	public GameObject particles_explosion;

	/** MISC **/
	public GameObject[] enemy;
	public GameObject marker;

	public GameObject lightningEmitter;
	public GameObject muzzleMesh;

	public GameObject interact;

	public static GameObject createLightningEmitter() {
		return Instantiate(GameController.prefabManager.lightningEmitter);
	}

	public static MuzzleEffect createMuzzleMesh(Transform gunTip) {
		GameObject go = Instantiate(GameController.prefabManager.muzzleMesh) as GameObject;
		go.transform.SetParent(gunTip);
		go.transform.localPosition = new Vector3(0, 0, -18);
		go.transform.localRotation = Quaternion.Euler(90, 270, 0);

		return go.GetComponent<MuzzleEffect>();
	}
}
