using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {
	public Material[] bulletTexConcrete;
	public Material[] bulletTexGlass;
	public Material[] bulletTexMetal;
	public Material[] bulletTexWood;
	public Material[] bulletTexBlood;

	public GameObject bulletPrefab;
	private Renderer bulletMeshRenderer;

	void Start() {
		bulletMeshRenderer = (bulletPrefab.GetComponent("MeshFilter") as MeshFilter).renderer;
	}

	public GameObject getBullet(string type) {
		switch(type) {
		case "glass":
			bulletMeshRenderer.material = bulletTexGlass[Random.Range(0, bulletTexGlass.Length)];
			return bulletPrefab;
		case "metal":
			bulletMeshRenderer.material = bulletTexMetal[Random.Range(0, bulletTexMetal.Length)];
			return bulletPrefab;
		case "wood":
			bulletMeshRenderer.material = bulletTexWood[Random.Range(0, bulletTexWood.Length)];
			return bulletPrefab;
		case "blood":
			bulletMeshRenderer.material = bulletTexBlood[Random.Range(0, bulletTexBlood.Length)];
			return bulletPrefab;
		default:
		case "concrete":
			bulletMeshRenderer.material = bulletTexConcrete[Random.Range(0, bulletTexConcrete.Length)];
			return bulletPrefab;
		}
	}
}
