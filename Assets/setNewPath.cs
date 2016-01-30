using UnityEngine;
using System.Collections;

public class setNewPath : MonoBehaviour {
	public Transform[] path;
	public bool startNewPath;
	private Ray ray;
	private int i = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (startNewPath == true && Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);
			if (hit.transform != null && hit.transform.gameObject.tag == "building") {
				i++;
				if (i < path.Length) {
					path [i] = hit.transform;
					Debug.Log (path);
				}
			}
		}
	}

	void OnMouseDown () {
		startNewPath = true;
	}
}
