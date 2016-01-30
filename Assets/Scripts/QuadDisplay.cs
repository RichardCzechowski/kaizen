using UnityEngine;
using System.Collections;

public class QuadDisplay : MonoBehaviour {

	public Material startMaterial;

	public MeshRenderer[] quads;

	void Start() {
		if (startMaterial) {
			SetMaterial (startMaterial);
		}
	}

	public void SetMaterial(Material newMaterial) {
		foreach (var quad in quads) {
			quad.sharedMaterial = newMaterial;
		}
	}

}
