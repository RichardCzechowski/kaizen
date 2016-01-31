using UnityEngine;
using System.Collections;

public class ForSaleSign : MonoBehaviour {

	public Building building;

	Renderer[] _renderers;
	Collider _collider;
	void Start() {
		_renderers = GetComponentsInChildren<Renderer> ();
		_collider = GetComponent<Collider> ();
		building = transform.parent.parent.GetComponent<Building> ();
	}

	void Update () {
	
		if (building.CanUpgrade()) {
			foreach (var r in _renderers) {
				r.enabled = true;
			}
			_collider.enabled = true;
		} else {
			foreach (var r in _renderers) {
				r.enabled = false;
			}
			_collider.enabled = false;
		}

	}

	void OnMouseUp() {
		building.Upgrade ();			
	}


}
