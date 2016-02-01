using UnityEngine;
using System.Collections;

public class NightEmitter : MonoBehaviour {

	public float randomness = 0.05f;

	ParticleSystem _light;
	float _sunrise;
	float _sunset;
	void Start() {
		_light = GetComponent<ParticleSystem> ();
		_sunrise = 0.3f + Random.value * randomness - randomness;
		_sunset = 0.7f + Random.value * randomness - randomness;
	}

	void Update () {
		if (DayNightController.instance.TimeOfDayIncludingPreview() < _sunrise || DayNightController.instance.TimeOfDayIncludingPreview() > _sunset) {
			var e = _light.emission;
			e.enabled = true;
			//_light.emission = e;
		
		} else {
			var e = _light.emission;
			e.enabled = false;
			//_light.emission = e;

		}
	}

}
