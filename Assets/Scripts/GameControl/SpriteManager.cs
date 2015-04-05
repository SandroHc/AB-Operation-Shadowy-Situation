﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteManager : MonoBehaviour {
	public Material[] bulletTexConcrete;
	public Material[] bulletTexGlass;
	public Material[] bulletTexMetal;
	public Material[] bulletTexWood;
	public Material[] bulletTexBlood;

	public Image weaponNoIcon;
	public Image weaponM9;
	public Image weaponM16;

	public GameObject bulletPrefab;
	private Renderer bulletMeshRenderer;

	void Start() {
		bulletMeshRenderer = bulletPrefab.GetComponent<MeshFilter>().GetComponent<Renderer>();
	}

	public GameObject getBullet(string tag) {
		switch(tag) {
		default:
		case Tags.wallConcrete:
			bulletMeshRenderer.material = bulletTexConcrete[Random.Range(0, bulletTexConcrete.Length)];
			return bulletPrefab;
		case Tags.wallGlass:
			bulletMeshRenderer.material = bulletTexGlass[Random.Range(0, bulletTexGlass.Length)];
			return bulletPrefab;
		case Tags.wallMetal:
			bulletMeshRenderer.material = bulletTexMetal[Random.Range(0, bulletTexMetal.Length)];
			return bulletPrefab;
		case Tags.wallWood:
			bulletMeshRenderer.material = bulletTexWood[Random.Range(0, bulletTexWood.Length)];
			return bulletPrefab;
		case Tags.enemy:
		case Tags.player:
			bulletMeshRenderer.material = bulletTexBlood[Random.Range(0, bulletTexBlood.Length)];
			return bulletPrefab;
		}
	}
}
