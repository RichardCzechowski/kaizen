using UnityEngine;
using System.Collections;

public class SettlerGenerator : MonoBehaviour {

	public GameObject settlerPrefab;

	public Transform entrance;

	public float settlersPerDay = 0.75f;

	// Use this for initialization
	void Start () {
		GeneratePerson ();
	}

	Person GeneratePerson() {
		GameObject obj = Instantiate (settlerPrefab, entrance.position, entrance.rotation) as GameObject;
		Person person = obj.GetComponent<Person> ();
		person.bullpenLocation = transform.position;
		return person;
	}

	float _lastDay = 0;
	float _accumulatedSettlers = 0;
	void Update () {
		if (DayNightController.instance.daysElapsed != _lastDay) {

			_accumulatedSettlers += settlersPerDay;

			int settlersToGenerate = Mathf.FloorToInt (_accumulatedSettlers);

			for(var i = 1 ;i < settlersToGenerate; i++) {
				GeneratePerson ();
				_accumulatedSettlers--;
			}

			_lastDay = DayNightController.instance.daysElapsed;
		}
	}

}
