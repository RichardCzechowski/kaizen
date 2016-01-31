using UnityEngine;
using System.Collections;

public class OccupancyIndicator : MonoBehaviour {

	Building building;

	public GameObject quadPrefab;

	public Texture2D emptySlotTexture;

	public float spacing = 1;
	public float width = 1;

	QuadDisplay[] _fgQuads;
	QuadDisplay[] _bgQuads;
	public Material materialTemplate;

	Material _emptyMaterial;

	void Start() {

		building = transform.parent.parent.GetComponent<Building> ();

		_emptyMaterial = new Material(materialTemplate);
		_emptyMaterial.mainTexture = emptySlotTexture;

		Rebuild ();
	}


	void Rebuild() {

		if (_fgQuads != null) {
			for (var i = 0; i < _fgQuads.Length; i++) {
				if (_fgQuads[i]) {
					Destroy (_fgQuads[i].gameObject);
				}
				if (_bgQuads[i]) {
					Destroy (_bgQuads[i].gameObject);
				}

			}
		}

		_fgQuads = new QuadDisplay[building.capacity];
		_bgQuads = new QuadDisplay[building.capacity];

		for (var i = 0; i < building.capacity; i++) {
			Vector3 pos = transform.position + transform.right * i * spacing - transform.right * spacing * building.capacity / 2 + transform.right * spacing / 2;

			GameObject fg = Instantiate (quadPrefab, pos, transform.rotation) as GameObject;
			GameObject bg = Instantiate (quadPrefab, pos, transform.rotation) as GameObject;

			fg.transform.parent = transform;
			fg.gameObject.name = "fg";
			bg.transform.parent = transform;
			bg.gameObject.name = "bg";

			_fgQuads [i] = fg.GetComponent<QuadDisplay>();
			_fgQuads [i].SetMaterial (_emptyMaterial);

			_bgQuads [i] = bg.GetComponent<QuadDisplay>();
			_bgQuads [i].SetMaterial (_emptyMaterial);
		}


	}

	void Update() {

		if (_fgQuads != null && building.capacity != _fgQuads.Length) {
			Rebuild ();
		}
		
		for (var i = 0; i < building.capacity; i++) {
			var fg = _fgQuads [i];
			var bg = _bgQuads [i];
			var occupants = building.OccupantsIncludingPreview ();
			if (i < occupants.Count) {
				var mat = new Material(materialTemplate);
				mat.mainTexture = occupants[i].portrait;
				fg.SetMaterial (mat);
				fg.gameObject.SetActive (true);

				var bgMat = new Material(_emptyMaterial);
				bgMat.color = occupants[i].color;
				bg.SetMaterial (bgMat);

			} else {
				bg.SetMaterial (_emptyMaterial);
				fg.gameObject.SetActive (false);
			}
		}

	}

}
