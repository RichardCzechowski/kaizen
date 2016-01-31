using UnityEngine;
using System.Collections;

public class ForSaleSign : MonoBehaviour {

	[HideInInspector]
	public Building building;

	public AudioClip upgradeSound;


	Renderer[] _renderers;
	Collider _collider;
	TextMesh _textMesh;
	void Start() {
		_renderers = GetComponentsInChildren<Renderer> ();
		_textMesh = GetComponentInChildren<TextMesh> ();
		_collider = GetComponent<Collider> ();
		building = transform.parent.parent.GetComponent<Building> ();
	}

	void Update () {
	
		if (building.CanUpgrade()) {
			foreach (var r in _renderers) {
				r.enabled = true;
			}
			_collider.enabled = true;
			_textMesh.text = building.upgradeCost + "";
		} else {
			foreach (var r in _renderers) {
				r.enabled = false;
			}
			_collider.enabled = false;
		}

	}

	void OnMouseUp() {
		building.Upgrade ();
		AudioSource.PlayClipAtPoint (upgradeSound, Camera.main.transform.position);
	}


}
