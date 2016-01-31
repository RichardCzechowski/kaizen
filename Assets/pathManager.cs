using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class pathManager : MonoBehaviour {

	public AudioClip positiveSound;
	public AudioClip negativeSound;
	public AudioClip cancelSound;

	public Camera colorCamera;
	public ColorCorrectionCurves curves;

	public bool fancyEffects = false;

	bool startNewPath;
	Ray ray;
	int i = 0;
	Person _lastPerson;

	// Use this for initialization
	void Start () {
//		Camera.main.gameObject.getcompon
	}

	Building _lastBuilding;

	Person GetPerson(GameObject obj) {
		var person = obj.GetComponentInParent<Person> ();
		var proxy = obj.GetComponentInParent<PersonProxy> ();

		if (!person && proxy && proxy.person) {
			person = proxy.person;
		}

		return person;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);

			var person = GetPerson (hit.transform.gameObject);

			if (hit.transform != null && hit.transform.gameObject.tag == "Player" || person) {
				
				tutorial.instance.kickOffIntroducePath ();

				if (fancyEffects) {
					curves.enabled = true;
					colorCamera.enabled = true;
				}

				person.SetState (Person.State.settingPath);
				person.ClearPaths ();
				person.ShowPath ();

				AudioSource.PlayClipAtPoint (person.selectSound, Camera.main.transform.position);


				DayNightController.instance.BeginPreview (0);

				i = 0;
				startNewPath = true;
				_lastPerson = person;

				DayNightController.instance.BeginPreviewHours (DayNightController.instance.ShiftStartHour (i));

				_lastBuilding = null;

				i++;

			} else if (hit.transform != null && startNewPath && hit.transform.gameObject.tag == "Building") {

				person = _lastPerson;

				// Search the hit object or its parents
				Building building = hit.transform.gameObject.GetComponent<Building> ();
				if (!building) {
					building = hit.transform.parent.gameObject.GetComponent<Building> ();
				}

				if (building.type == Building.Type.Home) {
					tutorial.instance.kickOffIntroduceHome ();
				}else if(building.type == Building.Type.Play){
					tutorial.instance.kickOffIntroduceFood ();
				}else if(building.type == Building.Type.Work){
					tutorial.instance.kickOffIntroduceWork ();
				}

				if (building.Full () || (_lastBuilding && building.type == _lastBuilding.type)) {
					AudioSource.PlayClipAtPoint (negativeSound, Camera.main.transform.position);
				} else {
					
					AudioSource.PlayClipAtPoint (positiveSound, Camera.main.transform.position);

					person.Buildings () [i - 1] = building;

					if (i == person.Buildings ().Length) {
						DayNightController.instance.EndPreview ();
						startNewPath = false;
						person.FadeOutPath ();
						Invoke ("Walkabout", Time.deltaTime);

						if (fancyEffects) {
							curves.enabled = false;
							colorCamera.enabled = false;
						}

					} else {
						DayNightController.instance.BeginPreviewHours (DayNightController.instance.ShiftStartHour (i));
						i++;
					}

					_lastBuilding = building;
						
				}

			} else if (startNewPath) {

				AudioSource.PlayClipAtPoint (cancelSound, Camera.main.transform.position);

				DayNightController.instance.EndPreview ();
				startNewPath = false;
				if (_lastPerson) {
					_lastPerson.selected = false;
					Invoke ("Resetto", Time.deltaTime);

				}

			}


		}
	}

	void Walkabout() {
		_lastPerson.SetState (Person.State.walking);

	}

	void Resetto() {
		_lastPerson.SetState (Person.State.settingPath);

	}
}

