using UnityEngine;
using System.Collections;

public class pathManager : MonoBehaviour {

	public AudioClip positiveSound;
	public AudioClip negativeSound;

	bool startNewPath;
	Ray ray;
	int i = 0;
	GameObject pathToSet;

	// Use this for initialization
	void Start () {
		
	}

	Building _lastBuilding;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);
			if (hit.transform != null && hit.transform.gameObject.tag == "Player") {

				DayNightController.instance.BeginPreview (0);

				i = 0;
				startNewPath = true;
				pathToSet = hit.transform.gameObject;

				var person = pathToSet.GetComponent<Person> ();
				person.selected = true;
				person.ClearPaths ();

				DayNightController.instance.BeginPreviewHours (DayNightController.instance.ShiftStartHour(i));

				_lastBuilding = null;

				i++;

			} else if (hit.transform != null && startNewPath && hit.transform.gameObject.tag == "Building") {

				var person = pathToSet.GetComponent<Person> ();
				// Search the hit object or its parents
				Building building = hit.transform.gameObject.GetComponent<Building> ();
				if (!building) {
					building = hit.transform.parent.gameObject.GetComponent<Building> ();
				}

				if (building.Full () || (_lastBuilding && building.type == _lastBuilding.type)) {
					AudioSource.PlayClipAtPoint (negativeSound, Camera.main.transform.position);
				} else {
					
					AudioSource.PlayClipAtPoint (positiveSound, Camera.main.transform.position);

					person.Buildings()[i - 1] = building;

					if (i == person.Buildings ().Length) {
						DayNightController.instance.EndPreview ();
						startNewPath = false;
						person.SetState (Person.State.walking);
						person.selected = false;
					} else {
						DayNightController.instance.BeginPreviewHours (DayNightController.instance.ShiftStartHour(i));
						i++;
					}

					_lastBuilding = building;
						
				}
					
			}
		}
	}
}
