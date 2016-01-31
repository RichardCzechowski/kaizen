using UnityEngine;
using System.Collections;

public class ForSaleSign : MonoBehaviour {

	public Building building;

	Renderer _renderer;
	Collider _collider;
	void Start() {
		_renderer = GetComponent<Renderer> ();
		_collider = GetComponent<Collider> ();
		building = transform.parent.parent.GetComponent<Building> ();
	}

	void Update () {
	
		if (scoreManager.instance.currentScore >= building.upgradeCost) {
			_renderer.enabled = true;
			_collider.enabled = true;
		} else {
			_renderer.enabled = false;
			_collider.enabled = false;
		}

	}

	void OnMouseUp() {
		building.Upgrade ();			
	}


}
