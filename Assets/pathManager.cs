using UnityEngine;
using System.Collections;

public class pathManager : MonoBehaviour {
	public bool startNewPath;
	private Ray ray;
	private int i = 0;
	private GameObject pathToSet;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);
			if (hit.transform != null && hit.transform.gameObject.tag == "Player") {
				i = 0;
				startNewPath = true;
				pathToSet = hit.transform.gameObject;
			}else if (hit.transform != null && startNewPath && hit.transform.gameObject.tag == "Building") {
				i++;
				if (i < pathToSet.GetComponent<setNewPath>().path.Length) {
					pathToSet.GetComponent<setNewPath>().path[i] = hit.transform;
					Debug.Log (pathToSet);
					if (i == 6) {
						startNewPath = false;
					}
				}
			}
		}
	}
}
