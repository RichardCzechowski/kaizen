using UnityEngine;
using System.Collections;

public class Prefabber : MonoBehaviour {

	public GameObject prefab;

	void Start () {
		GameObject obj = Instantiate (prefab, transform.position, transform.rotation) as GameObject;
		obj.transform.parent = transform;
	}

}
