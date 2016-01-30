using UnityEngine;
using System.Collections;

public class OccupancyIndicator : MonoBehaviour {

	public Building building;

	public GameObject quadPrefab;

	public Texture2D emptySlotTexture;
	public Texture2D fullSlotTexture;

	public float spacing = 1;
	public float width = 1;

	QuadDisplay[] _quads;
	public Material materialTemplate;

	Material _emptyMaterial;

	void Start() {

		_emptyMaterial = new Material(materialTemplate);
		_emptyMaterial.mainTexture = emptySlotTexture;

		_quads = new QuadDisplay[building.capacity];
		for (var i = 0; i < building.capacity; i++) {
			Vector3 pos = transform.position + transform.right * i * spacing - transform.right * spacing * building.capacity / 2 + transform.right * spacing / 2;
			GameObject o = Instantiate (quadPrefab, pos, transform.rotation) as GameObject;
			o.transform.parent = transform;
			_quads [i] = o.GetComponent<QuadDisplay>();
			_quads [i].SetMaterial (_emptyMaterial);
		}

	}

	void Update() {
		
		for (var i = 0; i < building.capacity; i++) {
			var quad = _quads [i];
			if (i < building.occupants.Length) {
				var mat = new Material(materialTemplate);
				mat.mainTexture = fullSlotTexture;
				_quads [i].SetMaterial (mat);

			} else {
				quad.SetMaterial (_emptyMaterial);
			}
		}

	}

}
