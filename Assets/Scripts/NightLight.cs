using UnityEngine;
using System.Collections;

public class NightLight : MonoBehaviour {

	public float randomness = 0.05f;

	Light light;
	float _sunrise;
	float _sunset;
	void Start() {
		light = GetComponent<Light> ();
		_sunrise = 0.3f + Random.value * randomness - randomness;
		_sunset = 0.7f + Random.value * randomness - randomness;
	}

	void Update () {
		if (DayNightController.instance.currentTimeOfDay < _sunrise || DayNightController.instance.currentTimeOfDay > _sunset) {
			light.enabled = true;
		
		} else {
			light.enabled = false;

		}
	}

}
