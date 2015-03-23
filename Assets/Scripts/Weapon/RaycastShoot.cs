using UnityEngine;
using System.Collections;

public class RaycastShoot : MonoBehaviour {

	public RaycastHit raycast(float range) {
		RaycastHit hit;

		if(Physics.Raycast(transform.position, transform.forward, out hit, range)) {
			// Do not render bullet holes on enemies (because the bullet will be shown on the capsule collider, not the mesh)
			if(hit.transform.gameObject.tag != Tags.enemy) {
				ShatterGlass shatter = hit.transform.gameObject.GetComponent<ShatterGlass>();
				if(shatter != null) {
					Debug.Log ("BREAKING GLASS!");
					shatter.Shatter(hit.point);
				} else {
					Vector3 position = hit.point;
					position += hit.normal * .0001f; // Lift the object a bit to prevent z-fighting

					GameObject obj = Instantiate(GameController.spriteManager.getBullet(convertTag(hit.transform.tag)), position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Then we'll instantiate a random bullet hole texture from our array and apply it where we click and adjust the position and rotation of textures to match the object being hit
					obj.transform.parent = hit.transform;
					Destroy(obj, 10);
				}
			}
		}


		return hit;
	}

	/**
	 * Picks the object tag and converts it to the tags used in the SpriteManager
	 */
	string convertTag(string tag) {
		switch(tag) {
		case Tags.wallConcrete:	return "concrete";
		case Tags.wallGlass:	return "glass";
		case Tags.wallMetal:	return "metal";
		case Tags.wallWood:		return "wood";
		default:				return "none";
		}
	}
}
