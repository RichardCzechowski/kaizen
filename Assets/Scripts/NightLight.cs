using UnityEngine;
using System.Collections;

public class NightLight : MonoBehaviour {

	public float randomness = 0.05f;

	Light _light;
	float _sunrise;
	float _sunset;
	void Start() {
		_light = GetComponent<Light> ();
		_sunrise = 0.3f + Random.value * randomness - randomness;
		_sunset = 0.7f + Random.value * randomness - randomness;
	}

	void Update () {
		if (DayNightController.instance.TimeOfDayIncludingPreview() < _sunrise || DayNightController.instance.TimeOfDayIncludingPreview() > _sunset) {
			_light.enabled = true;
		
		} else {
			_light.enabled = false;

		}
	}

}
