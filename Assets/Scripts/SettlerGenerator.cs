using UnityEngine;
using System.Collections;

public class SettlerGenerator : MonoBehaviour {

	public GameObject settlerPrefab;
	public AudioClip arrivalSound;

	public Transform entrance;

	public float spawnTimeOfDay = 0.2f;

	public float settlersPerDay = 0.75f;

	public float day1Boost = 1;
	public float day2Boost = 1;

	Person GeneratePerson() {
		GameObject obj = Instantiate (settlerPrefab, entrance.position, entrance.rotation) as GameObject;
		Person person = obj.GetComponent<Person> ();
		person.bullpenLocation = transform.position;
		return person;
	}

	float _lastDay = -1;
	float _accumulatedSettlers = 0;
	void Update () {
		if (DayNightController.instance.daysElapsed != _lastDay &&  DayNightController.instance.TimeOfDayActual() > spawnTimeOfDay) {

			if (DayNightController.instance.daysElapsed == 0) {
				_accumulatedSettlers += day1Boost;
			}
			if (DayNightController.instance.daysElapsed == 1) {
				_accumulatedSettlers += day2Boost;
			}

			_accumulatedSettlers += settlersPerDay;

			int settlersToGenerate = Mathf.FloorToInt (_accumulatedSettlers);

			for(var i = 0; i < settlersToGenerate; i++) {
				GeneratePerson ();
				_accumulatedSettlers--;
			}

			if (settlersToGenerate > 0) {
				AudioSource.PlayClipAtPoint (arrivalSound, transform.position);
			}

			_lastDay = DayNightController.instance.daysElapsed;
		}
	}

}
